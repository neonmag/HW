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
            try
            {
                _connection.Open();
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name, DeleteDt FROM Departments D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Entity.Departments
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Deleted = reader.GetString(2)
                    });
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            try
            {
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name, Price, DeleteDt FROM Products D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new(reader));
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            try
            {
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name, Surname, Secname, Id_main_dep,Id_sec_dep, Id_chief, DeleteDt FROM Managers D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    managers.Add(new Entity.Managers
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Secname = reader.GetString(3),
                        IdMainDep = reader.GetGuid(4),
                        IdSecDep = reader.GetValue(5) == DBNull.Value
                        ? null
                        : reader.GetGuid(5),
                        IdChief = reader.GetValue(6) == DBNull.Value
                        ? null
                        : reader.GetGuid(6),
                        Deleted = reader.GetString(7)
                    }) ;
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            try
            {
                using SqlCommand cmd = new() { Connection = _connection };

                cmd.CommandText = "SELECT S.* FROM Sales S";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    sales.Add(new Sales(reader));

                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            departmentCrudWindow = new(_connection);
            departmentCrudWindow.Department = departments[FirstView.SelectedIndex];
            departmentCrudWindow.ShowDialog();
        }

        private void SecondView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            productCrudWindow = new(_connection);
            productCrudWindow.Product = products[SecondView.SelectedIndex];
            productCrudWindow.ShowDialog();
        }

        private void ThirdView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListView item)
            {
                if(item.SelectedItem is Entity.Managers manager)
                {
                    ManagerCrudWindow dialog = new(_connection) { Owner = this, Manager = manager };
                    dialog.ShowDialog();
                }
            }
        }

        private void SecondView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ProductCrudWindow dialog = new(_connection);
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
                    SalesCrudWindow dialog = new SalesCrudWindow() { Owner = this };
                    dialog.Sale = sale;
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Sale is not null)
                        {
                            String sql = "UPDATE Sales SET SaleDate=(@sale_dt), Product_Id=(@product), Quantity=(@quantity), Manager_Id=(@manager), DeleteDt=(@delete) WHERE Id=(@id)";
                            using SqlCommand cmd = new(sql, _connection);
                            cmd.Parameters.AddWithValue("@sale_dt", dialog.Sale.SaleDate);
                            cmd.Parameters.AddWithValue("@product", dialog.Sale.Product_Id);
                            cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                            cmd.Parameters.AddWithValue("@manager", dialog.Sale.Manager_Id);
                            cmd.Parameters.AddWithValue("@delete", dialog.Sale.DeleteDt is null ? DBNull.Value : dialog.Sale.DeleteDt);
                            cmd.Parameters.AddWithValue("@id", dialog.Sale.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Зміни збережено!");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SalesCrudWindow dialog = new SalesCrudWindow() { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Sale is not null)
                {
                    string sql = "INSERT INTO Sales(Id, SaleDate, Product_Id, Quantity, Manager_Id) VALUES(@id, @saleDt, @productId, @quantity, @managerId)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@saleDt", dialog.Sale.SaleDate);
                    cmd.Parameters.AddWithValue("@productId", dialog.Sale.Product_Id);
                    cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                    cmd.Parameters.AddWithValue("@managerId", dialog.Sale.Manager_Id);
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
