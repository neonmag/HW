using HW.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SalesCrudWindow.xaml
    /// </summary>
    public partial class SalesCrudWindow : Window
    {
        public Sales? Sale { get; set; }
        private ObservableCollection<Managers> OwnerManagers;
        private ObservableCollection<Products> OwnerProducts;
        public SalesCrudWindow()
        {
            InitializeComponent();
            Sale = null!;
            OwnerManagers = null!;
            OwnerProducts = null!;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Впевнені, що хочете вилучити продаж?", "Вилучення", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.OK)
            {
                Sale.DeleteDt = DateTime.Now;
            }
            else
            {
                return;
            }

            DialogResult = true;
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Sale is null)
                return;

            if (Quantity_TxtBx.Text.Equals(String.Empty))
            {
                MessageBox.Show("Необхідно ввести кількість");
                Quantity_TxtBx.Focus();
                return;
            }
            int cnt;
            try
            {
                cnt = Convert.ToInt32(Quantity_TxtBx.Text);
            }
            catch
            {
                MessageBox.Show("Кількість не розпізнана (очікується число)");
                Quantity_TxtBx.Focus();
                return;
            }

            if (Product_CmbBx.SelectedItem is null)
            {
                MessageBox.Show("Необхідно вибрати товар");
                Product_CmbBx.Focus();
                return;
            }

            if (ManagerId_CmbBx.SelectedItem is null)
            {
                MessageBox.Show("Необхідно вибрати продавця");
                Product_CmbBx.Focus();
                return;
            }

            Sale.Quantity = cnt;

            if (Product_CmbBx.SelectedItem is Products product)
                Sale.ProductId = product.Id;
            else
                MessageBox.Show("Product_CmbBx.SelectedItem CAST Error");

            if (ManagerId_CmbBx.SelectedItem is Managers manager)
                Sale.ManagerId = manager.Id;
            else
                MessageBox.Show("ManagerId_CmbBx.SelectedItem CAST Error");

            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is Second owner)
            {
                DataContext = Owner;
                OwnerManagers = owner.managers;
                OwnerProducts = owner.products;
            }
            else
            {
                MessageBox.Show("Owner is not OrmWindow");
                Close();
            }

            if (Sale is null)
            {
                Sale = new Sales();
                Delete_Btn.IsEnabled = false;
            }
            else
            {
                Product_CmbBx.SelectedItem =
                    OwnerProducts
                    .Where(d => d.Id == Sale.ProductId)
                    .First();
                ManagerId_CmbBx.SelectedItem =
                    OwnerManagers
                    .Where(d => d.Id == Sale.ManagerId)
                    .First();
                Delete_Btn.IsEnabled = true;
            }

            Id_TxtBx.Text = Sale.Id.ToString();
            SaleDate_TxtBx.Text = Sale.SaleDate.ToString();
            Quantity_TxtBx.Text = Sale.Quantity.ToString();
        }
    }
}
