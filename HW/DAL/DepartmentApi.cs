using HW.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HW.DAL
{
    internal class DepartmentApi
    {
        private readonly SqlConnection _connection;

        public DepartmentApi(SqlConnection connection)
        {
            _connection = connection;
        }
        public bool Add(Departments department)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"INSERT INTO Departments (Id, Name) VALUES( @id, @name)";
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Parameters.AddWithValue("@name", department.Name);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
                return false;
            }
        }
        public bool Update(Departments department)
        {
            try
            {
                String sql = "UPDATE Departments SET Name=(@name), DeleteDt=(@DeleteDt) WHERE Id=(@id)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@name", department.Name);
                cmd.Parameters.AddWithValue("@DeleteDt", department.Deleted is null ? DBNull.Value : department.Deleted);
                cmd.Parameters.AddWithValue("@id", department.Id);


                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
                return false;
            }
        }
        public bool Delete(Departments department)
        {
            try
            {
                String sql = "UPDATE Departments SET DeleteDt=(@DeleteDt) WHERE Id=(@id)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@DeleteDt", department.Deleted is null ? DBNull.Value : department.Deleted);
                cmd.Parameters.AddWithValue("@id", department.Id);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
                return false;
            }
        }
        public List<Entity.Departments> GetAll()
        {
            var list = new List<Entity.Departments>();
            try
            {
                _connection.Open();

                using SqlCommand cmd = new("SELECT * FROM Departments", _connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                    list.Add(new Departments(reader));

            }
            catch (Exception ex)
            {
                string msg = DateTime.Now + ": " + this.GetType().Name + "::" + MethodBase.GetCurrentMethod()?.Name + " " + ex.Message;

                App.Logger.Log(msg, "SEVERE");
            }
            return list;
        }
    }
}
