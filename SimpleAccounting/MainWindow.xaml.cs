using System.Windows;
using System.Windows.Controls;
using SimpleAccounting.Models;
using SimpleAccounting.Pages;

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

            // Настраиваем видимость кнопок в зависимости от роли
            SetupRoleBasedUI();

            // Показываем приветственную страницу при запуске
            MainFrame.Navigate(new WelcomePage(user.FullName));
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
                // Директор видит всё
            }
        }

        // Обработчики кнопок
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage(_dbContext));
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientsPage(_dbContext));
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeesPage(_dbContext));
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage(_dbContext));
        }

        private void Income_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new IncomePage(_dbContext));
        }

        private void Expenses_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ExpensesPage(_dbContext));
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportsPage(_dbContext));
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