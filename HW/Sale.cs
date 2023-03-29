using System;
using System.Collections.Generic;

namespace HW;

public partial class Sale
{
    public Guid Id { get; set; }

    public DateTime SaleDate { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid ManagerId { get; set; }

    public DateTime? DeleteDt { get; set; }
}
