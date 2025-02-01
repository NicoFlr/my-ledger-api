using Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Role
{
    public interface IRoleManager
    {
        RoleDTO Get(Guid Id);
        List<RoleDTO> GetAll();
        RoleDTO Create(RoleDTO newRole);
        RoleDTO Update(RoleDTO roleToUpdate, Guid id);
    }
}
