using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public DateTime? Deleted { get; set; }
        public Products()
        {
            Name = null!;
            Deleted = null;
        }
        public Products(SqlDataReader reader)
        {
            Id = reader.GetGuid(0);
            Name = reader.GetString(1);
            Price = reader.GetDouble(2);
            Deleted = reader.GetValue("DeleteDt") == DBNull.Value
            ? null
            : reader.GetDateTime(3);
        }
    }
}
