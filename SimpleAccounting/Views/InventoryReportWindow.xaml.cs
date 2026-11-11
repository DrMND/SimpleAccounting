using Microsoft.Win32;
using SimpleAccounting.Models;
using SimpleAccounting.Views;
using System.IO;
using System.Linq;
using System.Windows;

namespace SimpleAccounting.Views
{
    // Вспомогательный класс для отображения в таблице
    public class InventoryItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue => Price * Quantity;
    }

    public partial class InventoryReportWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public InventoryReportWindow(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadInventory();
        }

        private void LoadInventory()
        {
            // Загружаем товары и преобразуем в InventoryItem
            var inventory = _dbContext.Products
                .Select(p => new InventoryItem
                {
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .ToList();

            InventoryGrid.ItemsSource = inventory;

            // Считаем общую сумму
            decimal total = inventory.Sum(i => i.TotalValue);
            TotalAmountText.Text = $"{total:N2} ₽";
        }
        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = "Остатки_на_складе.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Заголовки
                    writer.WriteLine("Название;Категория;Цена;Остаток;Сумма");

                    // Данные
                    foreach (InventoryItem item in InventoryGrid.ItemsSource)
                    {
                        writer.WriteLine($"{item.Name};{item.Category};{item.Price};{item.Quantity};{item.TotalValue}");
                    }
                }

                MessageBox.Show("Отчёт сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

