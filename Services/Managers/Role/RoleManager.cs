using Data.Exceptions;
using Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using Services.DTOModels;
using Services.Managers.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Role
{
    public class RoleManager : IRoleManager
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public RoleDTO Create(RoleDTO newRole)
        {
            try
            {
                Data.Models.Role role = new Data.Models.Role();
                role = DTOUtil.MapRoleDTO(newRole);
                _unitOfWork.RoleRepository.Add(role);
                _unitOfWork.Save();
                RoleDTO roleDTO = DTOUtil.MapRoleToDTO(role);
                return roleDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("Role with specified Id was not found.");
            }
        }

        public RoleDTO Get(Guid Id)
        {
            try
            {
                Data.Models.Role? role = _unitOfWork.GetContext().Roles.Where(a => a.Id == Id).FirstOrDefault();
                if (role != null)
                {
                    RoleDTO roleDTO = DTOUtil.MapRoleToDTO(role);
                    return roleDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The role with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Unexpected error.");
            }
        }

        public List<RoleDTO> GetAll()
        {
            try
            {
                List<RoleDTO> rolesDTOList = new List<RoleDTO>();
                List<Data.Models.Role> roleList = _unitOfWork.GetContext().Roles.Where(a => a.Id != null).ToList();
                rolesDTOList = DTOUtil.MapRoleToDTOList(roleList);
                return rolesDTOList;
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Couldnt retrieve roles list.");
            }
        }

        public RoleDTO Update(RoleDTO role, Guid id)
        {
            try
            {
                Data.Models.Role? foundRole = _unitOfWork.GetContext().Roles.Where(a => a.Id == id).FirstOrDefault();
                if (foundRole != null)
                {
                    _unitOfWork.RoleRepository.Detach(foundRole);
                    Data.Models.Role updatedRole = DTOUtil.MapRoleDTO(role);
                    updatedRole.Id = id;
                    _unitOfWork.RoleRepository.Update(updatedRole);
                    _unitOfWork.Save();

                    RoleDTO roleDTO = DTOUtil.MapRoleToDTO(updatedRole);
                    return roleDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The role with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The role with the specified Id couldnt be updated.");
            }
        }
    }
}
