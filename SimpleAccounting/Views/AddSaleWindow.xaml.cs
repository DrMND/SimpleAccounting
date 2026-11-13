using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class AddSaleWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly Sale? _existingSale;
        private SaleItem? _existingSaleItem;

        public bool SaleSaved { get; private set; }

        // Конструктор для новой продажи
        public AddSaleWindow(AppDbContext dbContext) : this(dbContext, null)
        {
        }

        // Конструктор для редактирования
        public AddSaleWindow(AppDbContext dbContext, Sale? sale)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _existingSale = sale;

            // Загружаем клиентов и товары
            ClientComboBox.ItemsSource = _dbContext.Clients.ToList();
            ProductComboBox.ItemsSource = _dbContext.Products.ToList();

            if (_existingSale != null)
            {
                // Режим редактирования
                Title = "Редактирование продажи";
                DatePicker.SelectedDate = _existingSale.Date;

                // Выбираем клиента
                ClientComboBox.SelectedItem = _dbContext.Clients.FirstOrDefault(c => c.Id == _existingSale.ClientId);

                // Загружаем позицию продажи
                _existingSaleItem = _dbContext.SaleItems.FirstOrDefault(si => si.SaleId == _existingSale.Id);

                if (_existingSaleItem != null)
                {
                    // Выбираем товар
                    ProductComboBox.SelectedItem = _dbContext.Products.FirstOrDefault(p => p.Id == _existingSaleItem.ProductId);
                    QuantityTextBox.Text = _existingSaleItem.Quantity.ToString();
                }

                AmountTextBox.Text = _existingSale.TotalAmount.ToString();
            }
            else
            {
                // Режим создания
                DatePicker.SelectedDate = System.DateTime.Now;
            }

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

            if (_existingSale == null)
            {
                // Создаём новую продажу
                var sale = new Sale
                {
                    Date = DatePicker.SelectedDate.Value,
                    ClientId = selectedClient.Id,
                    TotalAmount = amount
                };

                var saleItem = new SaleItem
                {
                    ProductId = selectedProduct.Id,
                    Quantity = quantity,
                    Price = selectedProduct.Price,
                    Sale = sale
                };

                // Уменьшаем остаток товара
                selectedProduct.Quantity -= quantity;

                _dbContext.Sales.Add(sale);
            }
            else
            {
                // Редактируем существующую продажу

                // Если изменился товар или количество — корректируем остатки
                if (_existingSaleItem != null)
                {
                    var oldProduct = _dbContext.Products.FirstOrDefault(p => p.Id == _existingSaleItem.ProductId);

                    if (oldProduct != null && oldProduct.Id != selectedProduct.Id)
                    {
                        // Товар изменился — возвращаем старый остаток
                        oldProduct.Quantity += _existingSaleItem.Quantity;
                    }
                    else if (oldProduct != null)
                    {
                        // Тот же товар — возвращаем старое количество
                        oldProduct.Quantity += _existingSaleItem.Quantity;
                    }
                }

                // Обновляем продажу
                _existingSale.Date = DatePicker.SelectedDate.Value;
                _existingSale.ClientId = selectedClient.Id;
                _existingSale.TotalAmount = amount;

                // Обновляем позицию
                if (_existingSaleItem != null)
                {
                    _existingSaleItem.ProductId = selectedProduct.Id;
                    _existingSaleItem.Quantity = quantity;
                    _existingSaleItem.Price = selectedProduct.Price;
                }

                // Уменьшаем новый остаток
                selectedProduct.Quantity -= quantity;
            }

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