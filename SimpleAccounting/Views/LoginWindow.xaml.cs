using Microsoft.EntityFrameworkCore;
using SimpleAccounting.Models;
using System.Linq;
using System.Windows;

namespace SimpleAccounting.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public LoginWindow()
        {
            InitializeComponent();

            // Создаем контекст БД
            _dbContext = new AppDbContext();

            // ЭТА СТРОКА СОЗДАЁТ/ОБНОВЛЯЕТ БАЗУ АВТОМАТИЧЕСКИ
            _dbContext.Database.Migrate();

            // Создаем тестового пользователя (если таблица пустая)
            _dbContext.CreateTestUsers();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _dbContext.Users.FirstOrDefault(u =>
                u.Login == login && u.Password == password);

            if (user != null)
            {
                // Открываем главное окно и передаём пользователя
                var mainWindow = new MainWindow(_dbContext, user);
                mainWindow.Show();

                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                PasswordBox.Clear();
                PasswordBox.Focus();
            }
        }
    }
}