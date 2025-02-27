﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOModels
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public List<TransactionDTO>? Transactions { get; set; }
    }
}
