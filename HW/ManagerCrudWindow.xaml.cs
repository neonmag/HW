using HW.Entity;
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
    /// Interaction logic for ManagerCrudWindow.xaml
    /// </summary>
    public partial class ManagerCrudWindow : Window
    {
        public Entity.Managers? Manager;
        public ObservableCollection<Entity.Departments> departments;
        public ObservableCollection<Entity.Managers> managers;
        SqlConnection sqlConnection;

        public ManagerCrudWindow()
        {
            InitializeComponent();
            Manager = null;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(MyName.Text.Equals(String.Empty))
            {
                MessageBox.Show("Name is empty");
                MyName.Focus();
                return;
            }
            if(MySurname.Text.Equals(String.Empty))
            {
                MessageBox.Show("Surname is empty");
                MySurname.Focus();
                return;
            }
            if(MyMainDep.SelectedItem is null)
            {
                MessageBox.Show("Main department is empty");
                MyMainDep.Focus();
                return;
            }
            this.Manager.Surname = MySurname.Text;
            this.Manager.Name = MyName.Text;
            this.Manager.Secname = MySecondName.Text;
            this.Manager.IdMainDep = (MyMainDep.SelectedItem as Entity.Departments).Id;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                 $"Do you really want to remove: {Manager.Name} {Manager.Surname}",
                 "Delete field",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Manager = null;
                this.DialogResult = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Owner is Second owner)
            {
                DataContext = Owner;
                departments = owner.departments;    
                managers = owner.managers;    
            }
            DataContext = Owner;
            if(this.Manager is null)
            {
                Delete.IsEnabled = false;
                Manager = new Entity.Managers();
            }
            if (this.Manager is not null)
            {
                idOf.Text = Manager.Id.ToString();
                MyName.Text = Manager.Name;
                MySecondName.Text = Manager.Secname;
                MySurname.Text = Manager.Surname;
                MySecondDep.SelectedItem =
                     departments
                    .Where(d => d.Id == this.Manager.IdSecDep)
                    .FirstOrDefault();
                MyChief.SelectedItem =
                    managers
                    .Where(m => m.Id == this.Manager.IdChief)
                    .FirstOrDefault();
                MyMainDep.SelectedItem =
                     departments
                    .Where(d => d.Id == this.Manager.IdMainDep)
                    .First();
                Delete.IsEnabled = true;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MySecondDep.SelectedIndex = -1;
        }

        private void ClearChief_Click(object sender, RoutedEventArgs e)
        {
            MyChief.SelectedIndex = -1;
        }
    }
}
