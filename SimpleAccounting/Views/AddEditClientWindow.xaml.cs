using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class AddEditClientWindow : Window
    {
        private readonly Client? _existingClient;

        public Client? ResultClient { get; private set; }

        public AddEditClientWindow(Client? client = null)
        {
            InitializeComponent();
            _existingClient = client;

            if (_existingClient != null)
            {
                TitleText.Text = "Редактирование клиента";
                FullNameTextBox.Text = _existingClient.FullName;
                PhoneTextBox.Text = _existingClient.Phone;
                EmailTextBox.Text = _existingClient.Email;
                INNTextBox.Text = _existingClient.INN;
                AddressTextBox.Text = _existingClient.Address;
            }
        }

        // Разрешаем вводить только цифры в телефон
        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Разрешаем вводить только цифры в ИНН
        private void INN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация ФИО/Названия
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("ФИО / Название обязательно для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                FullNameTextBox.Focus();
                return;
            }

            // Валидация телефона (обязательно)
            string phone = PhoneTextBox.Text.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Телефон обязателен для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return;
            }
            if (phone.Length != 11)
            {
                MessageBox.Show("Телефон должен содержать ровно 11 цифр (например, 89991234567)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return;
            }

            // Валидация Email (обязательно)
            string email = EmailTextBox.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email обязателен для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            // Строгая проверка email: буквы/цифры/знаки @ буквы/цифры . буквы/цифры
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(email))
            {
                MessageBox.Show("Некорректный формат Email (пример: user@example.com)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            // Валидация ИНН (обязательно)
            string inn = INNTextBox.Text.Trim();
            if (string.IsNullOrEmpty(inn))
            {
                MessageBox.Show("ИНН обязателен для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                INNTextBox.Focus();
                return;
            }
            if (inn.Length != 10 && inn.Length != 12)
            {
                MessageBox.Show("ИНН должен содержать 10 или 12 цифр", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                INNTextBox.Focus();
                return;
            }

            // Создаём или обновляем клиента
            if (_existingClient == null)
            {
                ResultClient = new Client
                {
                    FullName = FullNameTextBox.Text.Trim(),
                    Phone = phone,
                    Email = email,
                    INN = inn,
                    Address = AddressTextBox.Text.Trim()
                };
            }
            else
            {
                _existingClient.FullName = FullNameTextBox.Text.Trim();
                _existingClient.Phone = phone;
                _existingClient.Email = email;
                _existingClient.INN = inn;
                _existingClient.Address = AddressTextBox.Text.Trim();
                ResultClient = _existingClient;
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