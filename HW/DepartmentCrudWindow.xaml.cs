using HW.Entity;
using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace HW
{
    /// <summary>
    /// Interaction logic for DepartmentCrudWindow.xaml
    /// </summary>
    public partial class DepartmentCrudWindow : Window
    {
        public Entity.Departments Department { get; set; }
        SqlConnection sqlConnection;
        public DepartmentCrudWindow(SqlConnection sqlConnection)
        {
            InitializeComponent();
            Department = null!;
            this.sqlConnection = sqlConnection;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Department is null)
            {
                Delete.IsEnabled = false;
                Department = new Departments { Id = Guid.NewGuid() };
                idOf.Text = Department.Id.ToString();
            }
            else
            {
                idOf.Text = Department.Id.ToString();
                Depart.Text = Department.Name;
                Delete.IsEnabled = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Depart.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            //else if (MyName.Text == Product.Name && Product != null)
            //{
            //    this.DialogResult = false;
            //    return;
            //}
            Department.Name = Depart.Text;
            String sql = "UPDATE Departments SET Name=(@name) WHERE Id=(@id)";
            using SqlCommand cmd = new(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@name", Department.Name);
            cmd.Parameters.AddWithValue("@id", Department.Id);

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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete?", "Delete message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Department.Deleted = DateTime.Now.ToString();
                MessageBox.Show(Department.Deleted.ToString());
                String sql = "UPDATE Departments SET DeleteDt=(@Deleted) WHERE Id=(@id)";
                using SqlCommand cmd = new(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@Deleted", Department.Deleted);
                cmd.Parameters.AddWithValue("@id", Department.Id);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insert OK");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }
    }
}
