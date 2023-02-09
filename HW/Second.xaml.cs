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
        public SqlConnection _connection;
        public Second()
        {
            InitializeComponent();
            departments = new();
            products = new();
            managers = new();
            DataContext = this;
            _connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name FROM Departments D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Entity.Departments
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)

                    });
                }
                reader.Close();
                cmd.Dispose();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            try
            {
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name, Price FROM Products D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Entity.Products
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)
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
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name, Surname FROM Managers D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    managers.Add(new Entity.Managers
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2)
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
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(departments[FirstView.SelectedIndex].Name + "\n" + departments[FirstView.SelectedIndex].Id);
        }

        private void SecondView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(products[SecondView.SelectedIndex].Name + "\n" + products[SecondView.SelectedIndex].Id);
        }

        private void ThirdView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(managers[ThirdView.SelectedIndex].Name + " " + managers[ThirdView.SelectedIndex].Surname + "\n" + managers[ThirdView.SelectedIndex].Id);
        }
    }
}
