using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class IncomePage : Page
    {
        private readonly AppDbContext _dbContext;

        public IncomePage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadSales();
        }

        private void LoadSales()
        {
            // Загружаем продажи вместе с данными клиента (Include)
            SalesGrid.ItemsSource = _dbContext.Sales
                .Include(s => s.Client)
                .OrderByDescending(s => s.Date)
                .ToList();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSaleWindow(_dbContext);

            if (dialog.ShowDialog() == true || dialog.SaleSaved)
            {
                LoadSales();
                MessageBox.Show("Продажа успешно оформлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {
            if (SalesGrid.SelectedItem is Sale selected)
            {
                var result = MessageBox.Show($"Удалить продажу №{selected.Id} от {selected.Date:dd.MM.yyyy}?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем связанные позиции продажи (SaleItems)
                    var items = _dbContext.SaleItems.Where(i => i.SaleId == selected.Id).ToList();
                    _dbContext.SaleItems.RemoveRange(items);

                    // Удаляем саму продажу
                    _dbContext.Sales.Remove(selected);
                    _dbContext.SaveChanges();

                    LoadSales();
                    MessageBox.Show("Продажа удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите продажу для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}