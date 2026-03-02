using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class Gender
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
