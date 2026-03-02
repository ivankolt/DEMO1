using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class Product
{
    public string Articule { get; set; } = null!;

    public int? ProductsTypeId { get; set; }

    public string? Unit { get; set; }

    public decimal? Price { get; set; }

    public int? SuppliersId { get; set; }

    public int? ManufacturesId { get; set; }

    public int? GenderId { get; set; }

    public int? Discount { get; set; }

    public int? Qty { get; set; }

    public string? Description { get; set; }

    public string? ImgPth { get; set; }

    public virtual Gender? Gender { get; set; }

    public virtual Manufacture? Manufactures { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual Supplier? Suppliers { get; set; }
}
