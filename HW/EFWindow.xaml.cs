using HW.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EFWindow.xaml
    /// </summary>
    public partial class EFWindow : Window
    {
        internal EfContext efContext { get; set; } = new();
        private ICollectionView depListView;
        private static readonly Random random = new Random();

        public EFWindow()
        {
            InitializeComponent();
            this.DataContext = efContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            efContext.Departments.Load();
            depList.ItemsSource = efContext
                .Departments
                .Local
                .ToObservableCollection();
            //отримання посилання на depList, але як інтерфейс ICollectionView
            depListView = CollectionViewSource.GetDefaultView(depList.ItemsSource);
            depListView.Filter = //Predicate<object>
                obj => (obj as Department)?.DeleteDt == null;
            UpdateMonitor();
        }

        public void UpdateMonitor()
        {
            MonitorBlock.Text = "Departments: " +
                efContext.Departments.Count().ToString();
            MonitorBlock.Text += "\nProducts: " +
                efContext.Products.Count().ToString();
            MonitorBlock.Text += "\nManagers: " +
                efContext.Managers.Count().ToString();
            MonitorBlock.Text += "\nSales: " +
                efContext.Sales.Count().ToString();


        }


        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                //dialog.Department -- інша сутність, треба замінити під EF
                efContext.Departments.Add(
                    new Department()
                    {
                        Name = dialog.Department.Name,
                        Id = dialog.Department.Id
                    }
                    );
                // !! Додавання даних до контексту не додає їх до БД -- планування додавання
                efContext.SaveChanges(); // внесення змін до БД

                MonitorBlock.Text += "\nDepartments: " +
                    efContext.Departments.Count().ToString();
            }
        }


        #region DOUBLE_CLICKS
        private void DepartmentItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Department department)
                {
                    DepartmentCrudWindow dialog = new();
                    dialog.Department = new Entity.Departments()
                    {
                        Id = department.Id,
                        Name = department.Name
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Department is null)
                        {
                            department.DeleteDt = DateTime.Now;
                            depListView.Filter = DepartmentsDeletedFilter;
                            efContext.SaveChanges();
                        }
                        else
                        {
                            department.Name = dialog.Department.Name;
                            depList.Items.Refresh();
                            efContext.SaveChanges();
                        }
                    }

                }
            }
        }
        #endregion

        private bool DepartmentsDeletedFilter(object item)
        {
            if (item is Department department)
                return department.DeleteDt == null;
            return false;
        }

        private void ShowAllDepsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            depListView.Filter = null;// скидаємо фільтр -- показує усі дані

            ((GridView)depList.View) // Властивості Visivle для колонок ListView немає, тому
                .Columns[2]          // приховування/відображення через встановлення Width
                .Width = Double.NaN; // Double.NaN - автоматичне визначення
        }

        private void ShowAllDepsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //depListView.Filter = //Predicate<object>
            //    obj => (obj as Department)?.DeleteDt == null;
            depListView.Filter = DepartmentsDeletedFilter;
            ((GridView)depList.View).Columns[2].Width = 0;
        }

        private void UpdateDailyStatistic()
        {
            #region General stats
            // Checks
            var todaySales = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today);
            SalesChecks_Lbl.Content = todaySales.Count().ToString();

            // General quantity
            var todayQuantity = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today).Sum(s => s.Quantity);
            SalesPcs_Lbl.Content = todayQuantity;

            // Time of first sale
            var startMoment = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today).Min(s => s.SaleDate);
            StartMoment_Lbl.Content = startMoment.ToString("HH:mm:ss");

            // Time of last sale
            var finishMoment = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today).Max(s => s.SaleDate);
            FinishMoment_Lbl.Content = finishMoment.ToString("HH:mm:ss");

            // Best check by quantity
            var bestCheckByQuantity = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today).Max(s => s.Quantity);
            BestPcs_Lbl.Content = bestCheckByQuantity;

            // Average quantity by check
            var averageByQuantity = efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today).Average(s => s.Quantity);
            AvgPcs_Lbl.Content = averageByQuantity.ToString("0.00");

            // Deleted count
            var deletedCount = efContext.Sales.Where(s => s.DeleteDt.HasValue).Where(s => s.DeleteDt.Value.Date == DateTime.Today).Count();
            DeletedCount_Lbl.Content = deletedCount;
            #endregion
            #region Products stats
            // Best product by checks count
            var bestProductByChecks = efContext.Products
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  p => p.Id,
                  s => s.ProductId,
                  (p, sales) => new
                  {
                      Name = p.Name,
                      Cnt = sales.Count()
                  }
                ).OrderByDescending(g => g.Cnt).First();

            BestProduct_Lbl.Content = bestProductByChecks.Name + " -- " + bestProductByChecks.Cnt;

            // Best product by quantity
            var bestProductByQuantity = efContext.Products
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  p => p.Id,
                  s => s.ProductId,
                  (p, sales) => new
                  {
                      Name = p.Name,
                      Quantity = sales.Sum(s => s.Quantity)
                  }
                ).OrderByDescending(g => g.Quantity).First();

            BestProductByItems_Lbl.Content = bestProductByQuantity.Name + " -- " + bestProductByQuantity.Quantity;

            // Best product by money
            var bestProductByMoney = efContext.Products
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  p => p.Id,
                  s => s.ProductId,
                  (p, sales) => new
                  {
                      Name = p.Name,
                      Money = sales.Sum(s => s.Quantity) * p.Price
                  }
                ).OrderByDescending(g => g.Money).First();

            BestProductByMoney_Lbl.Content = bestProductByMoney.Name + " -- " + bestProductByMoney.Money;
            #endregion
            #region Managers stats
            // Best manager by checks
            var queryMan = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  m => m.Id,
                  s => s.ManagerId,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales.Count()
                  }
                ).OrderByDescending(g => g.Cnt)
                .First();

            BestManager_Lbl.Content = queryMan.Manager.Surname + " " + queryMan.Manager.Name + "---" + queryMan.Cnt;

            // Top 3
            var queryTop3 = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  m => m.Id,
                  s => s.ManagerId,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales.Sum(s => s.Quantity)
                  }
                ).OrderByDescending(g => g.Cnt).Take(3);

            int i = 1;

            foreach (var item in queryTop3)
            {
                Top3ManagersByItems_Lbl.Content +=
                    $"{i} - {item.Manager.Surname} {item.Manager.Name[0]}. -- {item.Cnt}\n";
            }

            // Best manager by money
            var queryBestManByMoney = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                  m => m.Id,
                  s => s.ProductId,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales
                            .Join(efContext.Products,
                            s => s.ProductId,
                            p => p.Id,
                            (sale, product) => sale.Quantity * product.Price).Sum()
                  }
                ).OrderByDescending(g => g.Cnt).First();

            BestManagerByMoney_Lbl.Content = $"{queryBestManByMoney.Manager.Surname} {queryBestManByMoney.Manager.Name[0]}. -- {queryBestManByMoney.Cnt} UAH";
            #endregion
            #region Departments stats

            #region Another option but cant translate
            //var departmentsStats = efContext.Departments.ToList()
            //    .GroupJoin(
            //    efContext.Managers,
            //    dep => dep.Id,
            //    man => man.IdMainDep,
            //    (dep, managers) => new
            //    {
            //        Department = dep,
            //        Cnt = managers
            //            .GroupJoin(
            //                efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today),
            //                m => m.Id,
            //                s => s.Manager_Id,
            //                (m, sales) => new
            //                {
            //                    ManCount = sales.Count()
            //                }
            //            ).Sum(c => c.ManCount)
            //    }
            //    ).OrderByDescending(dep => dep.Cnt);
            #endregion

            // Can't translate without ToList
            var departmentsStats = efContext.Departments.ToList()
                .GroupJoin(
                efContext.Managers
                .GroupJoin(
                efContext.Sales.Where(s => s.SaleDate.Date == DateTime.Today),
                manager => manager.Id,
                sale => sale.ManagerId,
                (manager, sales) => new
                {
                    Manager = manager,
                    Cnt = sales.Count(),
                    Sum = sales.Sum(s => s.Quantity)
                }),
                d => d.Id,
                m => m.Manager.IdMainDep,
                (dep, managers) => new
                {
                    Department = dep,
                    Cnt = managers.Sum(m => m.Cnt),
                    Sum = managers.Sum(m => m.Sum)
                }).OrderByDescending(d => d.Cnt);


            foreach (var department in departmentsStats)
            {
                DepartmentsStats_Lbl.Content += $"{department.Department.Name} -- {department.Cnt} -- {department.Sum}\n";
            }


            #endregion
        }


        private void GenerateSales_Btn_Click(object sender, RoutedEventArgs e)
        {

            double maxPrice = efContext.Products.Max(p => p.Price);
            int manCnt = efContext.Managers.Count();
            int prodCnt = efContext.Products.Count();

            for (int i = 0; i < 100; i++)
            {
                int indexM = random.Next(manCnt);
                Manager manager = efContext.Managers.Skip(indexM).First();

                int indexP = random.Next(prodCnt);
                Product product = efContext.Products.Skip(indexP).First();

                DateTime moment = DateTime.Today.AddSeconds(random.Next(0, 86400));

                int max = Convert.ToInt32(20 * (1 - product.Price / maxPrice) + 2);
                int quantity = random.Next(1, max);

                efContext.Sales.Add(new Sale()
                {
                    Id = Guid.NewGuid(),
                    ManagerId = manager.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    SaleDate = moment
                });
            }
            //MessageBox.Show(manager.Surname + " " + manager.Name + "\n" + product.Name + "\n" + moment.ToString() + "\n" + product.Price + " -- " + quantity.ToString());

            efContext.SaveChanges();
            UpdateMonitor();
            UpdateDailyStatistic();
        }
    }
}
