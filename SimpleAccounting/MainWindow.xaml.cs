using System.Windows;
using SimpleAccounting.Models;

namespace SimpleAccounting.Views
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly User _currentUser;

        public MainWindow(AppDbContext dbContext, User user)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _currentUser = user;
            Title = $"Бухгалтерия малого предприятия — {user.FullName} ({user.Role})";

            // Устанавливаем приветствие
            WelcomeText.Text = $"{user.FullName}";

            // Настраиваем видимость кнопок в зависимости от роли
            SetupRoleBasedUI();
        }

        // Пустой конструктор для Designer
        public MainWindow() : this(new AppDbContext(), new User { FullName = "Пользователь", Role = "Директор" })
        {
        }

        private void SetupRoleBasedUI()
        {
            string role = _currentUser.Role;

            if (role == "Бухгалтер")
            {
                // Бухгалтер НЕ видит управление пользователями
                UsersButton.Visibility = Visibility.Collapsed;
            }
            else if (role == "Директор")
            {
                // Директор видит всё (супер-админ)
            }
        }

        // Обработчики кнопок
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Товары' будет здесь", "Информация");
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Клиенты' будет здесь", "Информация");
        }
        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Сотрудники' будет здесь", "Информация");
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Пользователи' будет здесь", "Информация");
        }

        private void Income_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Доходы' будет здесь", "Информация");
        }

        private void Expenses_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Расходы' будет здесь", "Информация");
        }

        private void Invoices_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Счета' будет здесь", "Информация");
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Отчёты' будет здесь", "Информация");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Выйти из системы?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        }
    }
}