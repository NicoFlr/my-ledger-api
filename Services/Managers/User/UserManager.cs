using Data.Exceptions;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.DTOModels;
using Services.Managers.User;

namespace Services.Managers.User
{
    public class UserManager : IUserManager
    {
        private readonly UnitOfWork _unitOfWork;

        public UserManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDTO Create(UserDTO newUser)
        {
            try
            {
                Data.Models.User user = new Data.Models.User();
                user = DTOUtil.MapUserDTO(newUser);
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();
                UserDTO userDTO = DTOUtil.MapUserToDTO(user);
                return userDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("User with specified Id was not found.");
            }
        }

        public UserDTO Get(Guid Id)
        {
            try
            {
                Data.Models.User? user = _unitOfWork.GetContext().Users.Include(u=>u.Role).Where(a => a.Id == Id && a.IsDeleted == false).FirstOrDefault();
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
                    _unitOfWork.UserRepository.Update(updatedUser);
                    _unitOfWork.Save();

                    UserDTO userDTO = DTOUtil.MapUserToDTO(updatedUser);
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
    }
}
