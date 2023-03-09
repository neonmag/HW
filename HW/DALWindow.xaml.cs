using HW.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DALWindow.xaml
    /// </summary>
    public partial class DALWindow : Window
    {
        private readonly DataContext dataContext;
        public ObservableCollection<Entity.Departments> DepartmentsList { get; set; }
        public ObservableCollection<Entity.Managers> ManagerList { get; set; }
        public SqlConnection _connection;

        public DALWindow()
        {
            InitializeComponent();
            dataContext = new DataContext();
            DepartmentsList = new(dataContext.Departments.GetAll());
            ManagerList = new(dataContext.Managers.GetAll());

            DataContext = this;
            _connection = new(App.ConnectionString);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void DepartmentsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entity.Departments department)
                {
                    DepartmentCrudWindow dialog = new DepartmentCrudWindow();
                    dialog.Department = department;
                    if (dialog.ShowDialog() == true)
                    {
                        dataContext.Departments.Update(dialog.Department);
                    }
                }
            }
        }
        private void AddDepartment_Btn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                dataContext.Departments.Add(dialog.Department);
            }
        }

        private void AddManagerButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                if (dataContext.Managers.Add(dialog.Manager))
                {
                    MessageBox.Show("Додано успішно");
                    ManagerList.Add(dialog.Manager);
                }
                else
                {
                    MessageBox.Show("Помилка додавання");
                }
            }
        }

        private void ManagersItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Managers manager)
                {
                    MessageBox.Show(manager.ToString());
                }
            }
        }
    }
}
