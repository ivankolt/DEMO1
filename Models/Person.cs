using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
