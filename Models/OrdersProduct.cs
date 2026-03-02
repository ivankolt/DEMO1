using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class OrdersProduct
{
    public int Id { get; set; }

    public int? OrdersId { get; set; }

    public string? ProductsId { get; set; }

    public int? Qty { get; set; }

    public virtual Order? Orders { get; set; }

    public virtual Product? Products { get; set; }
}
