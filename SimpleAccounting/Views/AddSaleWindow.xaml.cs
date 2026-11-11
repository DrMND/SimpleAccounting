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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class AddSaleWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public bool SaleSaved { get; private set; }

        public AddSaleWindow(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            // Загружаем клиентов и товары
            ClientComboBox.ItemsSource = _dbContext.Clients.ToList();
            ProductComboBox.ItemsSource = _dbContext.Products.ToList();

            // По умолчанию — сегодняшняя дата
            DatePicker.SelectedDate = System.DateTime.Now;

            // Подписываемся на изменения для автоподсчёта суммы
            ProductComboBox.SelectionChanged += ProductComboBox_SelectionChanged;
            QuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
        }

        // Разрешаем вводить только цифры
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // При выборе товара — пересчитываем сумму
        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecalculateAmount();
        }

        // При изменении количества — пересчитываем сумму
        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecalculateAmount();
        }

        // Автоподсчёт суммы = цена × количество
        private void RecalculateAmount()
        {
            if (ProductComboBox.SelectedItem is Product selectedProduct &&
                int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
            {
                decimal total = selectedProduct.Price * quantity;
                AmountTextBox.Text = total.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация даты
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация клиента
            if (ClientComboBox.SelectedItem is not Client selectedClient)
            {
                MessageBox.Show("Выберите клиента", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация товара
            if (ProductComboBox.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Выберите товар", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация количества
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return;
            }

            // Проверяем остаток на складе
            if (quantity > selectedProduct.Quantity)
            {
                MessageBox.Show($"Недостаточно товара на складе. Остаток: {selectedProduct.Quantity} шт.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация суммы
            if (!decimal.TryParse(AmountTextBox.Text.Replace('.', ','), out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму продажи", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountTextBox.Focus();
                return;
            }

            // Создаём продажу
            var sale = new Sale
            {
                Date = DatePicker.SelectedDate.Value,
                ClientId = selectedClient.Id,
                TotalAmount = amount
            };

            // Создаём позицию продажи
            var saleItem = new SaleItem
            {
                ProductId = selectedProduct.Id,
                Quantity = quantity,
                Price = selectedProduct.Price,
                Sale = sale
            };
            _dbContext.SaleItems.Add(saleItem);
            // Уменьшаем остаток товара
            selectedProduct.Quantity -= quantity;

            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            SaleSaved = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SaleSaved = false;
            Close();
        }
    }
}