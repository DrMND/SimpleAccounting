using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SimpleAccounting.Models;
using SimpleAccounting.Views;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SimpleAccounting.Views
{
    // Вспомогательный класс для отображения в таблице
    public class SalesReportItem
    {
        public DateTime Date { get; set; }
        public string ClientFullName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public partial class SalesReportWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public SalesReportWindow(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            // По умолчанию — текущий месяц
            var now = DateTime.Now;
            StartDatePicker.SelectedDate = new DateTime(now.Year, now.Month, 1);
            EndDatePicker.SelectedDate = now;

            // Автоматически формируем отчёт при открытии
            GenerateReport();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите период", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime startDate = StartDatePicker.SelectedDate.Value.Date;
            DateTime endDate = EndDatePicker.SelectedDate.Value.Date.AddDays(1); // Включаем последний день

            // Загружаем продажи за период с данными клиента
            var sales = _dbContext.Sales
                .Include(s => s.Client)
                .Where(s => s.Date >= startDate && s.Date < endDate)
                .OrderByDescending(s => s.Date)
                .Select(s => new SalesReportItem
                {
                    Date = s.Date,
                    ClientFullName = s.Client != null ? s.Client.FullName : "Без клиента",
                    TotalAmount = s.TotalAmount
                })
                .ToList();

            SalesGrid.ItemsSource = sales;

            // Считаем общую сумму
            decimal total = sales.Sum(s => s.TotalAmount);
            TotalSalesText.Text = $"{total:N2} ₽";
        }
        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = $"Продажи_{StartDatePicker.SelectedDate:dd.MM.yyyy}_{EndDatePicker.SelectedDate:dd.MM.yyyy}.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveDialog.FileName, false, new System.Text.UTF8Encoding(true)))
                {
                    // Заголовки
                    writer.WriteLine("Дата;Клиент;Сумма");

                    // Данные
                    foreach (SalesReportItem item in SalesGrid.ItemsSource)
                    {
                        writer.WriteLine($"{item.Date:dd.MM.yyyy};{item.ClientFullName};{item.TotalAmount}");
                    }
                }

                MessageBox.Show("Отчёт сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

