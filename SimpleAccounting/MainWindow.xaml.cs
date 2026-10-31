using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            Title = $"Бухгалтерия малого предприятия — {_currentUser.FullName}";
        }

        // Пустой конструктор для Designer
        public MainWindow() : this(new AppDbContext(), new User { FullName = "Пользователь" })
        {
        }

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

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Продажи' будет здесь", "Информация");
        }

        private void Expenses_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел 'Расходы' будет здесь", "Информация");
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