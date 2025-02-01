using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public decimal? Money { get; set; }

    public DateTimeOffset? DateTime { get; set; }

    public bool IsBill { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
