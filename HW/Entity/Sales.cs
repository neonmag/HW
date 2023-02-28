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
        public Guid Product_Id { get; set; }
        public double Quantity { get; set; }
        public Guid Manager_Id { get; set; }
        public DateTime? DeleteDt { get; set; }
        public Sales()
        {
            Id = Guid.NewGuid();
            SaleDate = DateTime.Now;
            Quantity = 1;
        }
        public Sales(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            SaleDate = reader.GetDateTime("SaleDate");
            Product_Id = reader.GetGuid("Product_Id");
            Quantity = reader.GetDouble("Quantity");
            Manager_Id = reader.GetGuid("Manager_Id");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value ? null : reader.GetDateTime("DeleteDt");
        }
    }
}
