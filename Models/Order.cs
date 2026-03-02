using System;
using System.Collections.Generic;

namespace Den.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? OrdersDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? PickuppointId { get; set; }

    public int? UsersId { get; set; }

    public string? Code { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual Pickuppoint? Pickuppoint { get; set; }

    public virtual OrdersStatus? Status { get; set; }

    public virtual User? Users { get; set; }
}
