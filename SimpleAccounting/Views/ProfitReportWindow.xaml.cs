using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SimpleAccounting.Models;
using SimpleAccounting.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SimpleAccounting.Views
{
    public class ProfitItem
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public partial class ProfitReportWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public ProfitReportWindow(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            var now = DateTime.Now;
            StartDatePicker.SelectedDate = new DateTime(now.Year, now.Month, 1);
            EndDatePicker.SelectedDate = now;

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
            DateTime endDate = EndDatePicker.SelectedDate.Value.Date.AddDays(1);

            // Сначала загружаем данные в память (.ToList()), потом суммируем
            var sales = _dbContext.Sales
                .Where(s => s.Date >= startDate && s.Date < endDate)
                .ToList();
            decimal totalIncome = sales.Sum(s => s.TotalAmount);

            var expenses = _dbContext.Expenses
                .Where(e => e.Date >= startDate && e.Date < endDate)
                .ToList();
            decimal totalExpenses = expenses.Sum(e => e.Amount);

            // Налог УСН 6% от доходов
            decimal taxRate = 0.06m;
            decimal tax = totalIncome * taxRate;

            // Чистая прибыль
            decimal netProfit = totalIncome - totalExpenses - tax;

            // Формируем список показателей
            var profitItems = new List<ProfitItem>
            {
                new ProfitItem { Name = "Доходы (продажи)", Amount = totalIncome },
                new ProfitItem { Name = "Расходы", Amount = -totalExpenses },
                new ProfitItem { Name = $"Налог УСН 6%", Amount = -tax },
                new ProfitItem { Name = "Чистая прибыль", Amount = netProfit }
            };

            ProfitGrid.ItemsSource = profitItems;

            // Отображаем итоговую прибыль
            NetProfitText.Text = $"{netProfit:N2} ₽";

            // Цвет в зависимости от прибыли/убытка
            if (netProfit >= 0)
            {
                NetProfitText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                NetProfitText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = $"Прибыль_{StartDatePicker.SelectedDate:dd.MM.yyyy}_{EndDatePicker.SelectedDate:dd.MM.yyyy}.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveDialog.FileName, false, new System.Text.UTF8Encoding(true)))
                {
                    // Заголовки
                    writer.WriteLine("Показатель;Сумма");

                    // Данные
                    foreach (ProfitItem item in ProfitGrid.ItemsSource)
                    {
                        writer.WriteLine($"{item.Name};{item.Amount}");
                    }
                }

                MessageBox.Show("Отчёт сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
