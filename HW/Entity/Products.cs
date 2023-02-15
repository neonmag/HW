using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW.Entity
{
    public class Products
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public String Deleted { get; set; }
    }
}
