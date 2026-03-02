using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class User
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public int? PersonsId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Person? Persons { get; set; }

    public virtual Role? Role { get; set; }
}
