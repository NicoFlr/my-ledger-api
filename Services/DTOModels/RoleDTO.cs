using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOModels
{
    public class RoleDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public List<UserDTO>? Users { get; set; }
    }
}
