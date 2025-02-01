using Data.Models;
using Services.DTOModels;

namespace Services.Managers.User
{
    public interface IUserManager
    {
        UserDTO Get(Guid Id);
        List<UserDTO> GetAll();
        UserDTO Create(UserDTO newUser);
        UserDTO Update(UserDTO userToUpdate, Guid id);
        UserDTO SoftDelete(Guid id);
        string Login(string auth);

        UserDTO UpdatePassword(Guid id, UserUpdatePasswordDTO userUpdatePasswordDTO);
    }
}
