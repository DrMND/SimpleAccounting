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
using System.Text.RegularExpressions;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class AddEditProductWindow : Window
    {
        private readonly Product? _existingProduct;

        public Product? ResultProduct { get; private set; }

        public AddEditProductWindow(Product? product = null)
        {
            InitializeComponent();
            _existingProduct = product;

            if (_existingProduct != null)
            {
                // Режим редактирования
                TitleText.Text = "Редактирование товара";
                NameTextBox.Text = _existingProduct.Name;
                CategoryTextBox.Text = _existingProduct.Category;
                PriceTextBox.Text = _existingProduct.Price.ToString();
                QuantityTextBox.Text = _existingProduct.Quantity.ToString();
            }
        }

        // Разрешаем вводить только цифры и точку
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация названия
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Название товара обязательно для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                NameTextBox.Focus();
                return;
            }

            // Валидация цены (заменяем точку на запятую для русской локали)
            if (!decimal.TryParse(PriceTextBox.Text.Replace('.', ','), out decimal price) || price < 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return;
            }

            // Валидация остатка
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Остаток должен быть неотрицательным целым числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return;
            }

            // Создаём или обновляем товар
            if (_existingProduct == null)
            {
                // Новый товар
                ResultProduct = new Product
                {
                    Name = NameTextBox.Text.Trim(),
                    Category = CategoryTextBox.Text.Trim(),
                    Price = price,
                    Quantity = quantity
                };
            }
            else
            {
                // Редактирование существующего
                _existingProduct.Name = NameTextBox.Text.Trim();
                _existingProduct.Category = CategoryTextBox.Text.Trim();
                _existingProduct.Price = price;
                _existingProduct.Quantity = quantity;
                ResultProduct = _existingProduct;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}