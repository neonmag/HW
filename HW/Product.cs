using System;
using System.Collections.Generic;

namespace HW;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public DateTime? DeleteDt { get; set; }
}
