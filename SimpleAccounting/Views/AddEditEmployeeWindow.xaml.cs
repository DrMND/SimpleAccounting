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
    public partial class AddEditEmployeeWindow : Window
    {
        private readonly Employee? _existingEmployee;

        public Employee? ResultEmployee { get; private set; }

        public AddEditEmployeeWindow(Employee? employee = null)
        {
            InitializeComponent();
            _existingEmployee = employee;

            if (_existingEmployee != null)
            {
                // Режим редактирования
                TitleText.Text = "Редактирование сотрудника";
                FullNameTextBox.Text = _existingEmployee.FullName;
                PositionTextBox.Text = _existingEmployee.Position;
                SalaryTextBox.Text = _existingEmployee.Salary.ToString();
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
            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("ФИО обязательно для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                FullNameTextBox.Focus();
                return;
            }

            // Валидация должности
            if (string.IsNullOrWhiteSpace(PositionTextBox.Text))
            {
                MessageBox.Show("Должность обязательна для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PositionTextBox.Focus();
                return;
            }

            // Валидация оклада
            if (!decimal.TryParse(SalaryTextBox.Text.Replace('.', ','), out decimal salary) || salary < 0)
            {
                MessageBox.Show("Оклад должен быть положительным числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                SalaryTextBox.Focus();
                return;
            }

            // Создаём или обновляем сотрудника
            if (_existingEmployee == null)
            {
                // Новый сотрудник
                ResultEmployee = new Employee
                {
                    FullName = FullNameTextBox.Text.Trim(),
                    Position = PositionTextBox.Text.Trim(),
                    Salary = salary
                };
            }
            else
            {
                // Редактирование существующего
                _existingEmployee.FullName = FullNameTextBox.Text.Trim();
                _existingEmployee.Position = PositionTextBox.Text.Trim();
                _existingEmployee.Salary = salary;
                ResultEmployee = _existingEmployee;
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