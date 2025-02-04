using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOModels
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }

        public decimal? Money { get; set; }

        public DateTimeOffset? DateTime { get; set; }

        public bool IsBill { get; set; }

        public Guid? CategoryId { get; set; }

        public CategoryDTO? Category { get; set; }

        public List<UserDTO>? Users { get; set; }
    }
}
