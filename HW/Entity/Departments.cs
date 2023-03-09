using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HW.Entity
{
    public class Departments
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? Deleted { get; set; }
        public Departments()
        {
            Id = Guid.NewGuid();
            Name = null!;
        }
        public Departments(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Deleted = reader.GetValue("DeleteDt") == DBNull.Value ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return $"{Id.ToString()[..4]} {Name}";
        }
    }
}
