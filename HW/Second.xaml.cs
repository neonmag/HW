using HW.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace HW
{
    /// <summary>
    /// Interaction logic for Second.xaml
    /// </summary>
    public partial class Second : Window
    {
        public ObservableCollection<Entity.Departments> departments { get; set; }
        public ObservableCollection<Entity.Products> products { get; set; }
        public ObservableCollection<Entity.Managers> managers { get; set; }
        public ObservableCollection<Sales> sales { get; set; }

        public SqlConnection _connection;
        public DepartmentCrudWindow departmentCrudWindow;
        public ProductCrudWindow productCrudWindow;
        public Second()
        {
            InitializeComponent();
            departments = new();
            products = new();
            managers = new();
            DataContext = this;
            sales = new ObservableCollection<Sales>();
            _connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _connection.Open();
            LoadDepartments();
            LoadProducts();
            LoadManagers();
            LoadSales();
        }
        public void LoadSales()
        {
            SqlCommand cmd = new() { Connection = _connection };
            try
            {
                cmd.CommandText = "SELECT S.* FROM Sales S WHERE DeleteDt IS NULL";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new Entity.Sales(reader));
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Window will be closed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }
        public void LoadDepartments()
        {
            SqlCommand cmd = new() { Connection = _connection };
            try
            {
                cmd.CommandText = "SELECT D.* FROM Departments D WHERE D.DeleteDt IS NULL";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new(reader));
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Window will be closed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }

        public void LoadManagers()
        {
            SqlCommand cmd = new() { Connection = _connection };
            try
            {
                cmd.CommandText = "SELECT M.* FROM Managers M WHERE DeleteDt IS NULL";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    managers.Add(new Entity.Managers(reader));
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Window will be closed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }

        public void LoadProducts()
        {
            SqlCommand cmd = new() { Connection = _connection };
            try
            {
                cmd.CommandText = "SELECT P.* FROM Products P WHERE P.DeleteDt IS NULL";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Entity.Products(reader));
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Window will be closed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
        }
        private void ExecuteCommand(SqlCommand command, string commandName)
        {
            try//виконання команди
            {
                command.ExecuteNonQuery(); // NonQuery - без повернення результату
                MessageBox.Show(
                    commandName + " successfully complete",
                    commandName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            command.Dispose();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entity.Departments department)
                {
                    departmentCrudWindow = new();
                    departmentCrudWindow.Department = department;
                    if (departmentCrudWindow.ShowDialog() == true)
                    {
                        if (departmentCrudWindow.Department is null) //Delete
                        {
                            string command =
                                @"UPDATE Departments
                                  SET DeleteDt = CURRENT_TIMESTAMP
                                  WHERE Id = @id; ";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", department.Id);
                            ExecuteCommand(cmd, $"Delete: {department.Name}");
                            departments.Clear();
                            LoadDepartments();
                        }
                        else // Update
                        {
                            //MessageBox.Show(department.ToString());                            
                            string command =
                                @"UPDATE Departments
                                  SET Name = @name
                                  WHERE Id=@id;";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", department.Id);
                            cmd.Parameters.AddWithValue("@name", department.Name);
                            ExecuteCommand(cmd, "Update Department Name");
                            departments.Clear();
                            LoadDepartments();
                        }
                    }
                }
            }
        }

        private void SecondView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entity.Products product)
                {
                    productCrudWindow = new();
                    productCrudWindow.Product = product;
                    if (productCrudWindow.ShowDialog() == true)
                    {
                        if (productCrudWindow.Product is null) //Delete
                        {
                            string command =
                                @"UPDATE Products
                                  SET DeleteDt = CURRENT_TIMESTAMP
                                  WHERE Id = @id; ";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", product.Id);
                            ExecuteCommand(cmd, $"Delete: {product.Name}");
                            products.Clear();
                            LoadProducts();
                        }
                        else // Update
                        {
                            string command =
                                @"UPDATE Products
                                SET Name = @name, Price = @price
                                WHERE Id=@id;";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", product.Id);
                            cmd.Parameters.AddWithValue("@name", product.Name);
                            cmd.Parameters.AddWithValue("@price", product.Price);
                            ExecuteCommand(cmd, "Update Department");
                            products.Clear();
                            LoadProducts();
                        }
                    }
                }
            }
        }

        private void ThirdView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entity.Managers manager)
                {
                    ManagerCrudWindow dialog = new()
                    {
                        Owner = this,
                        Manager = manager
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Manager is null) //Delete
                        {
                            string command =
                                @"UPDATE Managers
                                  SET DeleteDt = CURRENT_TIMESTAMP
                                  WHERE Id = @id; ";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", manager.Id);
                            ExecuteCommand(cmd, $"Delete: {manager.Name} {manager.Surname}");
                            managers.Clear();
                            LoadManagers();
                        }
                        else // Update
                        {
                            string command =
                                @"UPDATE Managers 
                                SET 
                                Surname = @surname,
                                Name = @name, 
                                Secname = @secname, 
                                Id_main_dep = @IdMainDep, 
                                Id_sec_dep = @IdSecDep, 
                                Id_chief = @IdChief
                                WHERE Id = @id;";

                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", manager.Id);
                            cmd.Parameters.AddWithValue("@surname", manager.Surname);
                            cmd.Parameters.AddWithValue("@name", manager.Name);
                            cmd.Parameters.AddWithValue("@secname", manager.Secname);
                            cmd.Parameters.AddWithValue("@IdMainDep", manager.IdMainDep);
                            if (manager.IdSecDep != null)
                                cmd.Parameters.AddWithValue("@IdSecDep", manager.IdSecDep);
                            else
                                cmd.Parameters.AddWithValue("@IdSecDep", DBNull.Value);

                            if (manager.IdChief != null)
                                cmd.Parameters.AddWithValue("@IdChief", manager.IdChief);
                            else
                                cmd.Parameters.AddWithValue("@IdChief", DBNull.Value);

                            ExecuteCommand(cmd, "Update Manager");
                            managers.Clear();
                            LoadManagers();
                        }
                    }

                }
            }
        }

        private void SecondView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ProductCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            { 
                if (dialog.Product is not null)
                {
                    String sql = "INSERT INTO Products(Id,Name,Price) VALUES (@id,@name,@price)";
                    using SqlCommand cmd = new(sql, _connection);
                    cmd.Parameters.AddWithValue("@id", dialog.Product.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.Product.Name);
                    cmd.Parameters.AddWithValue("@price", dialog.Product.Price);
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Insert OK");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    // Не рекомендовано
                    //String sql = $"INSERT INTO Product(Id, Name, Price)"
                    //    + $"VALUES(`{productCrudWindow.Product.Id}`, N`{productCrudWindow.Product.Name}`, {productCrudWindow.Product.Price}";
                    //using SqlCommand cmd = new(sql, _connection);
                    //try
                    //{
                    //    cmd.ExecuteNonQuery();
                    //    MessageBox.Show("Insert OK");
                    //}
                    //catch(Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                }
            }
        }

        private void SalesView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Sales sale)
                {
                    SalesCrudWindow dialog = new() { Owner = this, Sale = sale };
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Sale is null)
                        {
                            string command =
                               @"UPDATE Sales
                                  SET DeleteDt = CURRENT_TIMESTAMP
                                  WHERE Id = @id; ";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", sale.Id);
                            var product = products.Where(p => p.Id == sale.ProductId).FirstOrDefault();
                            ExecuteCommand(cmd, $"Delete sale info about: {product.Name}");
                            sales.Clear();
                            LoadSales();
                        }
                        else
                        {
                            string command =
                               @"UPDATE Sales
                                 SET
                                 Quantity = @quantity, 
                                 Product_Id = @product_id,
                                 Manager_Id = @manager_id 
                                 WHERE Id = @id;";
                            using SqlCommand cmd = new(command, _connection);
                            cmd.Parameters.AddWithValue("@id", dialog.Sale.Id);
                            cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                            cmd.Parameters.AddWithValue("@product_id", dialog.Sale.ProductId);
                            cmd.Parameters.AddWithValue("@manager_id", dialog.Sale.ManagerId);
                            ExecuteCommand(cmd, "Update Sale");
                            sales.Clear();
                            LoadSales();
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SalesCrudWindow dialog = new SalesCrudWindow() { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Sale is not null)
                {
                    string sql = "INSERT INTO Sales(Id, SaleDate, ProductId, Quantity, ManagerId) VALUES(@id, @saleDt, @productId, @quantity, @managerId)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@saleDt", dialog.Sale.SaleDate);
                    cmd.Parameters.AddWithValue("@productId", dialog.Sale.ProductId);
                    cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                    cmd.Parameters.AddWithValue("@managerId", dialog.Sale.ManagerId);
                    cmd.Parameters.AddWithValue("@id", dialog.Sale.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Продаж успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
