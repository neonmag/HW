using HW.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HW
{
    /// <summary>
    /// Логика взаимодействия для ProductCrudWindow.xaml
    /// </summary>
    /// 
    public partial class ProductCrudWindow : Window
    {
        public Entity.Products Product { get; set; }
        public SqlConnection sqlConnection;
        public ProductCrudWindow()  
        {
            InitializeComponent();
            Product = null!;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Product is null)
            {
                Delete.IsEnabled = false;
                Product = new Products { Id = Guid.NewGuid() };
                idOf.Text = Product.Id.ToString();
            }
            else
            {
                idOf.Text = Product.Id.ToString();
                MyName.Text = Product.Name;
                MyPrice.Text = Product.Price.ToString();
                Delete.IsEnabled = true;

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyName.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            //else if (MyName.Text == Product.Name && Product != null)
            //{
            //    this.DialogResult = false;
            //    return;
            //}
            Product.Name = MyName.Text;
            Product.Price = Convert.ToDouble(MyPrice.Text);
            String sql = "UPDATE Products SET Name=(@name), Price=(@price) WHERE Id=(@id)";
            using SqlCommand cmd = new(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@name", Product.Name);
            cmd.Parameters.AddWithValue("@price", Product.Price);
            cmd.Parameters.AddWithValue("@id", Product.Id);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Insert OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Do you really want to remove: {Product.Name}",
                "Delete field",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Product = null;
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
