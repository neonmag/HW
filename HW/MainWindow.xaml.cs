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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SqlClient;

namespace HW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            // !! Cтворення об'єкту не відкриває підключення
            _connection = new();
            // Головний параметр підключення - рядок підключення
            _connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\ADO\HW\HW\DB.mdf;Integrated Security=True";
        }

        private void InstallDepartaments_Click(object sender, RoutedEventArgs e)
        {
            // Команда - інструмент для виконання SQL запитів
            SqlCommand cmd = new();
            // Головні параметри команди - це:
            cmd.Connection = _connection; // підключення
            cmd.CommandText = @"CREATE TABLE Departments (
	                            Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                            Name		NVARCHAR(50) NOT NULL
                                ) ;";                                       // SQL запит(текст)
            // Виконання команди
            try
            {
                cmd.ExecuteNonQuery(); // Не вимагає результату
                MessageBox.Show("Create OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            cmd.Dispose(); // команда - некерований ресурс, вимагає утилізації
        }

        private void FillDepartaments_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new();
            // Головні параметри команди - це:
            cmd.Connection = _connection; // підключення
            cmd.CommandText = @"INSERT INTO Departments 
                                	( Id, Name )
                                VALUES 
                                	( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT отдел'		 	 ), 
                                	( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'		 ), 
                                	( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности'), 
                                	( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'		 ),
                                	( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'		 ), 
                                	( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Отдел продаж'		 ), 
                                	( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба' )";                                       // SQL запит(текст)
            // Виконання команди
            try
            {
                cmd.ExecuteNonQuery(); // Не вимагає результату
                MessageBox.Show("Create OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            cmd.Dispose(); // команда - некерований ресурс, вимагає утилізації
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                StatusConnection.Content = "Connected";
                StatusConnection.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
            {
                StatusConnection.Content = "Connected";
                StatusConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
