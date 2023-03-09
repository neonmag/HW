﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW.EFCore
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
