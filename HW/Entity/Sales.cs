using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HW.Entity
{
    public class Sales
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Sales()
        {
            Id = Guid.NewGuid();
            Quantity = 1;
            SaleDate = DateTime.Now;
        }

        public Sales(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            SaleDate = (DateTime)reader.GetValue("SaleDate");
            ProductId = reader.GetGuid("ProductId");
            Quantity = reader.GetInt32("Quantity");
            ManagerId = reader.GetGuid("ManagerId");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value
                ? null
                : (DateTime)reader.GetValue("DeleteDt");
        }
    }
}
