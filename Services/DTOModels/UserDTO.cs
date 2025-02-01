using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOModels
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string? Password { get; set; }

        public Guid RoleId { get; set; }
        public bool IsDeleted { get; set; }

        public RoleDTO? Role { get; set; } = null!;

        public List<TransactionDTO>? Transactions { get; set; }

    }
}
