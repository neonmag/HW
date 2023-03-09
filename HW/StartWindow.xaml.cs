using System;
using System.Collections.Generic;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace HW
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void OpenFirst_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OpenSecond_Click(object sender, RoutedEventArgs e)
        {
            Second second = new Second();
            second.Show();
            this.Close();
        }

        private void OpenThird_Click(object sender, RoutedEventArgs e)
        {
            DALWindow third = new DALWindow();
            third.Show();
            this.Close();
        }

        private void EF_Click(object sender, RoutedEventArgs e)
        {
            EFWindow third = new EFWindow();
            third.Show();
            this.Close();
        }
    }
}
