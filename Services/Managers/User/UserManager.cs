using Data.Exceptions;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.DTOModels;
using Services.Managers.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Managers.User
{
    public class UserManager : IUserManager
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        const string BASIC_AUTHORIZATION_SCHEME = "Basic";

        public UserManager(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public UserDTO Create(UserDTO newUser)
        {
            try
            {
                Data.Models.User user = new Data.Models.User();
                user = DTOUtil.MapUserDTO(newUser);
                user.Password = HashPassword(newUser.Password, out var salt);
                string saltString = Convert.ToHexString(salt);
                user.Password = user.Password + "|" + saltString;
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();
                UserDTO userDTO = Get(user.Id);
                return userDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("User with specified Id was not found.");
            }
        }

        private string HashPassword(string password, out byte[] salt)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            int keySize = int.Parse(_configuration["SecretSettings:KeySize"]);
            int iterations = int.Parse(_configuration["SecretSettings:Iterations"]);

            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        public UserDTO Get(Guid Id)
        {
            try
            {
                Data.Models.User? user = _unitOfWork.GetContext().Users.Include(u=>u.Role).Include(u=>u.Transactions).Where(a => a.Id == Id && a.IsDeleted == false).FirstOrDefault();
                if (user != null)
                {
                    UserDTO userDTO = DTOUtil.MapUserToDTO(user);
                    return userDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The user with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Unexpected error.");
            }
        }

        public List<UserDTO> GetAll()
        {
            try
            {
                List<UserDTO> usersDTOList = new List<UserDTO>();
                List<Data.Models.User> userList = _unitOfWork.GetContext().Users.Include(u => u.Role).Where(a => a.IsDeleted == false).ToList();
                usersDTOList = DTOUtil.MapUserToDTOList(userList);
                return usersDTOList;
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Couldnt retrieve users list.");
            }
        }

        public UserDTO Update(UserDTO user, Guid id)
        {
            try
            {
                Data.Models.User? foundUser = _unitOfWork.GetContext().Users.Where(a => a.Id == id).FirstOrDefault();
                if (foundUser != null)
                {
                    _unitOfWork.UserRepository.Detach(foundUser);
                    Data.Models.User updatedUser = DTOUtil.MapUserDTO(user);
                    updatedUser.Id = id;
                    updatedUser.Password = foundUser.Password;
                    _unitOfWork.UserRepository.Update(updatedUser);
                    _unitOfWork.Save();

                    UserDTO userDTO = Get(updatedUser.Id);
                    return userDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The user with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The user with the specified Id couldnt be updated.");
            }
        }

        public UserDTO UpdatePassword(Guid id, UserUpdatePasswordDTO userUpdatePasswordDTO)
        {
            try
            {
                Data.Models.User? foundUser = _unitOfWork.GetContext().Users.Where(a => a.Id == id).FirstOrDefault();
                if (foundUser != null)
                {
                    _unitOfWork.UserRepository.Detach(foundUser);

                    foundUser = UpdateUserPassword(foundUser, userUpdatePasswordDTO);
                    _unitOfWork.UserRepository.Update(foundUser);
                    _unitOfWork.Save();

                    UserDTO userDTO = Get(foundUser.Id);
                    return userDTO;
                }
                else
                {
                    throw new EntityNotFoundError("User's password could not be updated because user wasn't found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Unexpected error. The user with the specified Id couldnt be updated.");
            }
        }

        private Data.Models.User UpdateUserPassword(Data.Models.User foundUser, UserUpdatePasswordDTO userUpdatePasswordDTO)
        {
            if (foundUser.IsDeleted == true)
            {
                throw new EntityNotFoundError("User with specified Id was not found.");
            }

            string[] foundUserCredential = foundUser.Password.Split(new[] { '|' }, 2);
            string foundUserPassword = foundUserCredential[0];
            string foundUserPasswordSalt = foundUserCredential[1];

            bool isNewPasswordTheSame = VerifyPassword(userUpdatePasswordDTO.NewPassword, foundUserPassword, Convert.FromHexString(foundUserPasswordSalt));
            bool iscurrentPasswordValid = VerifyPassword(userUpdatePasswordDTO.CurrentPassword, foundUserPassword, Convert.FromHexString(foundUserPasswordSalt));

            if (iscurrentPasswordValid)
            {
                if (!isNewPasswordTheSame)
                {
                    foundUser.Password = HashPassword(userUpdatePasswordDTO.NewPassword, out var salt);
                    string saltString = Convert.ToHexString(salt);
                    foundUser.Password = foundUser.Password + "|" + saltString;
                }
                else
                {
                    throw new UnprocessableContentException("New password cannot be the same as the previous one");
                }
            }
            else
            {
                throw new BadRequestException("Current password is incorrect");
            }

            return foundUser;
        }

        public UserDTO SoftDelete(Guid id)
        {
            try
            {
                Data.Models.User? userToDelete = _unitOfWork.GetContext().Users.Where(a => a.Id == id).FirstOrDefault();
                if (userToDelete != null)
                {
                    UserDTO userDTO = DTOUtil.MapUserToDTO(userToDelete);
                    userToDelete.IsDeleted = true;
                    _unitOfWork.UserRepository.Update(userToDelete);
                    _unitOfWork.Save();
                    return userDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The user with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The user with the specified Id couldnt be deleted.");
            }
        }

        public string Login(string auth)
        {
            try
            {
                string[] decodeCredentials = DecodeCredentials(auth);
                var email = decodeCredentials[0];
                var password = decodeCredentials[1];

                Data.Models.User? user = _unitOfWork.GetContext().Users.Include(u => u.Role).Where(user => user.Email.Equals(email)).FirstOrDefault();
                if (user != null)
                {
                    string[] userCredential = user.Password.Split(new[] { '|' }, 2);
                    string userPassword = userCredential[0];
                    string userPasswordSalt = userCredential[1];

                    if (!VerifyPassword(password, userPassword, Convert.FromHexString(userPasswordSalt)))
                    {
                        throw new UserAuthorizationException("Unauthorized user.");
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim("firstName",user.FirstName),
                        new Claim("lastName",user.LastName),
                        new Claim("email", user.Email),
                        new Claim("roleId", user.RoleId.ToString()),
                        new Claim("userId",user.Id.ToString()),
                        new Claim(ClaimTypes.Role,user.Role.Name),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var header = new JwtHeader(signingCredentials);
                    var payload = new JwtPayload
                    (
                        _configuration["AuthSettings:Audience"],
                        _configuration["AuthSettings:Issuer"],
                        claims,
                        DateTime.Now,
                        DateTime.UtcNow.AddMinutes(double.Parse(_configuration["AuthSettings:AccessExpireMinutes"]))
                    );
                    var token = new JwtSecurityToken(header, payload);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new UserAuthorizationException("Unauthorized user");
                }
            }
            catch (SystemException)
            {
                throw new UserAuthorizationException("Unauthorized user");
            }
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            int keySize = int.Parse(_configuration["SecretSettings:KeySize"]);
            int iterations = int.Parse(_configuration["SecretSettings:Iterations"]);

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        private static string[] DecodeCredentials(string auth)
        {
            auth = auth.Substring(BASIC_AUTHORIZATION_SCHEME.Length).Trim();
            byte[] data = System.Convert.FromBase64String(auth);
            string[] decodeCredentials = System.Text.ASCIIEncoding.ASCII.GetString(data).Split(new[] { ':' }, 2);
            return decodeCredentials;
        }
    }
}
