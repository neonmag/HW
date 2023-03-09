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
using System.Windows.Threading;
using System.Xml.Linq;

namespace HW
{
    /// <summary>
    /// Interaction logic for DepartmentCrudWindow.xaml
    /// </summary>
    public partial class DepartmentCrudWindow : Window
    {
        //Обмінне поле - передається з викликаючого вікна
        public Entity.Departments Department { get; set; }

        private bool SaveButtonState;
        private bool inputWasChaged;
        private bool stringIsEmpty;
        private DispatcherTimer timer;
        public DepartmentCrudWindow()
        {
            InitializeComponent();
            Department = null;
            BaseOptions();
        }

        private void BaseOptions()
        {
            SaveButtonState = true;
            inputWasChaged = false;
            stringIsEmpty = false;

            timer = new();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(CheckNameField);
            timer.Start();
        }

        #region WINDOW_EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Department is null)  // режим додавання (Create)
            {
                Department = new Entity.Departments();
                Department.Id = Guid.NewGuid();
            }
            else // режим редагування чи видалення (Update or Delete)
            {
                idOf.Text = Department.Id.ToString();
                Depart.Text = Department.Name;
                Delete.IsEnabled = true;
            }
            idOf.Text = Department.Id.ToString();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Stop();
        }
        #endregion
        //ПЕРЕВІРКА НА ВВЕДЕННЯ ДАНИХ
        #region CONDITIONS
        private void CheckNameField(object sender, EventArgs args)
        {
            if (Depart.Text == Department.Name)
            {
                SaveButtonState = false;
                inputWasChaged = false;
            }
            else
            {
                inputWasChaged = true;
                if (!stringIsEmpty)
                {
                    SaveButtonState = true;
                }
            }

            if (Depart.Text.Trim() == String.Empty)
            {
                SaveButtonState = false;
                stringIsEmpty = true;
            }
            else
            {
                stringIsEmpty = false;
                if (inputWasChaged)
                {
                    SaveButtonState = true;
                }
            }
        }


        #endregion

        #region BUTTONS_EVENTS
        private void SaveButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!SaveButtonState)
            {
            }
        }
        private void SaveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!SaveButtonState)
            {
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveButtonState)
            {
                Department.Name = Depart.Text;
                this.DialogResult = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Do you really want to remove: {Department.Name}",
                "Delete field",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Department = null;
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // те що поверне ShowDialog
        }

        #endregion

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
