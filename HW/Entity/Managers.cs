using HW.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW.Entity
{
    public class Managers
    {
        public Guid Id { get; set; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Secname { get; set; }
        public Guid IdMainDep { get; set; }   // Guid - ValueType, вживається для полів з модифікатором NOT NULL
        public Guid? IdSecDep { get; set; }    // Якщо NULL не заборонений, то Guid не підходить, потрібен Guid?
        public Guid? IdChief { get; set; }     // Guid? - скорочення від Nullable<Guid>
        public string Deleted { get; set; }

        public Managers()
        {
            Id = Guid.NewGuid();
            Surname = null!;
            Name = null!;
            Secname = null!;
        }
        public Managers(DbDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Surname = reader.GetString("Surname");
            Name = reader.GetString("Name");
            Secname = reader.GetString("Secname");
            IdMainDep = reader.GetGuid("Id_main_dep");
            IdSecDep = reader.GetValue("Id_sec_dep") == DBNull.Value
                          ? null
                          : reader.GetGuid("Id_sec_dep");
            IdChief = reader.IsDBNull("Id_chief")
                          ? null
                          : reader.GetGuid("Id_chief");
            Deleted = reader.IsDBNull("DeleteDt")
                          ? null
                          : reader.GetString("DeleteDt");
        }
        public override string ToString()
        {
            return $"{Id.ToString()[..4]} {Surname} {Name} {Secname} {IdMainDep} {IdSecDep} {IdChief}";
        }

        //////////////////// Navigation Properties ////////////////

        internal DataContext? dataContext;

        public Entity.Departments? MainDep
        {
            get
            {
                return dataContext?
                    .Departments
                    .GetAll()
                    .Find(d => d.Id == this.IdMainDep);
            }
        }
    }
}
