using System.Windows;
using System.Windows.Controls;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class AddEditUserWindow : Window
    {
        private readonly User? _existingUser;

        public User? ResultUser { get; private set; }

        public AddEditUserWindow(User? user = null)
        {
            InitializeComponent();
            _existingUser = user;

            if (_existingUser != null)
            {
                TitleText.Text = "Редактирование пользователя";
                LoginTextBox.Text = _existingUser.Login;
                PasswordBox.Password = _existingUser.Password;
                FullNameTextBox.Text = _existingUser.FullName;

                // Выбираем роль в ComboBox
                foreach (ComboBoxItem item in RoleComboBox.Items)
                {
                    if (item.Content.ToString() == _existingUser.Role)
                    {
                        RoleComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                // По умолчанию — Бухгалтер
                RoleComboBox.SelectedIndex = 1;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация логина
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                MessageBox.Show("Логин обязателен для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                LoginTextBox.Focus();
                return;
            }

            // Валидация пароля
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Пароль обязателен для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("ФИО обязательно для заполнения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                FullNameTextBox.Focus();
                return;
            }

            // Валидация роли
            if (RoleComboBox.SelectedItem is not ComboBoxItem selectedRole)
            {
                MessageBox.Show("Выберите роль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string role = selectedRole.Content.ToString();

            // Создаём или обновляем пользователя
            if (_existingUser == null)
            {
                ResultUser = new User
                {
                    Login = LoginTextBox.Text.Trim(),
                    Password = PasswordBox.Password,
                    FullName = FullNameTextBox.Text.Trim(),
                    Role = role
                };
            }
            else
            {
                _existingUser.Login = LoginTextBox.Text.Trim();
                _existingUser.Password = PasswordBox.Password;
                _existingUser.FullName = FullNameTextBox.Text.Trim();
                _existingUser.Role = role;
                ResultUser = _existingUser;
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