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
    public partial class AddEditExpenseWindow : Window
    {
        private readonly Expense? _existingExpense;

        public Expense? ResultExpense { get; private set; }

        public AddEditExpenseWindow(Expense? expense = null)
        {
            InitializeComponent();
            _existingExpense = expense;

            if (_existingExpense != null)
            {
                // Режим редактирования
                TitleText.Text = "Редактирование расхода";
                DatePicker.SelectedDate = _existingExpense.Date;
                CategoryTextBox.Text = _existingExpense.Category;
                AmountTextBox.Text = _existingExpense.Amount.ToString();
                DescriptionTextBox.Text = _existingExpense.Description;
            }
            else
            {
                // По умолчанию — сегодняшняя дата
                DatePicker.SelectedDate = System.DateTime.Now;
            }
        }

        // Разрешаем вводить только цифры и точку
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
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

            // Валидация категории
            if (string.IsNullOrWhiteSpace(CategoryTextBox.Text))
            {
                MessageBox.Show("Категория обязательна для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CategoryTextBox.Focus();
                return;
            }

            // Валидация суммы
            if (!decimal.TryParse(AmountTextBox.Text.Replace('.', ','), out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Сумма должна быть положительным числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountTextBox.Focus();
                return;
            }

            // Создаём или обновляем расход
            if (_existingExpense == null)
            {
                ResultExpense = new Expense
                {
                    Date = DatePicker.SelectedDate.Value,
                    Category = CategoryTextBox.Text.Trim(),
                    Amount = amount,
                    Description = DescriptionTextBox.Text.Trim()
                };
            }
            else
            {
                _existingExpense.Date = DatePicker.SelectedDate.Value;
                _existingExpense.Category = CategoryTextBox.Text.Trim();
                _existingExpense.Amount = amount;
                _existingExpense.Description = DescriptionTextBox.Text.Trim();
                ResultExpense = _existingExpense;
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