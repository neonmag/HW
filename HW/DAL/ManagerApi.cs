using HW.Entity;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Accessibility;

namespace HW.DAL
{
    internal class ManagerApi
    {
        private readonly SqlConnection _connection;
        private readonly DataContext _dataContext;
        private List<Entity.Managers> list;
        public ManagerApi(SqlConnection connection, DataContext dataContext)
        {
            _connection = connection;
            _dataContext = dataContext;
            list = null!;
        }
        public bool Add(Managers managers)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"INSERT INTO Managers (Id, Name) VALUES( @id, @name)";
                cmd.Parameters.AddWithValue("@id", managers.Id);
                cmd.Parameters.AddWithValue("@name", managers.Name);
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
        public bool Update(Managers managers)
        {
            try
            {
                String sql = "UPDATE Managers SET Name=(@name), DeleteDt=(@DeleteDt) WHERE Id=(@id)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@name", managers.Name);
                cmd.Parameters.AddWithValue("@DeleteDt", managers.Deleted is null ? DBNull.Value : managers.Deleted);
                cmd.Parameters.AddWithValue("@id", managers.Id);


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
                String sql = "UPDATE Managers SET DeleteDt=(@DeleteDt) WHERE Id=(@id)";
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
        public List<Entity.Managers> GetAll(bool includeDeleted = true)
        {
            if (list is not null) { return list; }

            list = new();
            try
            {   
                string query = "SELECT * FROM Managers m";
                if (!includeDeleted) query += " WHERE m.DeleteDt IS NULL";
                using SqlCommand cmd = new(query, _connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new(reader) { dataContext = _dataContext });
                }
            }
            catch (Exception ex)
            {
                String msg =
                    DateTime.Now + ": " +
                    this.GetType().Name + "::" +
                    System.Reflection.MethodBase.GetCurrentMethod()?.Name + " " +
                    ex.Message;

                App.Logger.Log(msg, "SEVERE");
            }
            return list;
        }
    }
}
