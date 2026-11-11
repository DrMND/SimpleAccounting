using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class ClientsPage : Page
    {
        private readonly AppDbContext _dbContext;

        public ClientsPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadClients();
        }

        private void LoadClients()
        {
            ClientsGrid.ItemsSource = _dbContext.Clients.ToList();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditClientWindow();

            if (dialog.ShowDialog() == true && dialog.ResultClient != null)
            {
                _dbContext.Clients.Add(dialog.ResultClient);
                _dbContext.SaveChanges();
                LoadClients();
                MessageBox.Show("Клиент успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Client selected)
            {
                var dialog = new AddEditClientWindow(selected);

                if (dialog.ShowDialog() == true && dialog.ResultClient != null)
                {
                    _dbContext.Clients.Update(dialog.ResultClient);
                    _dbContext.SaveChanges();
                    LoadClients();
                    MessageBox.Show("Клиент успешно обновлён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Client selected)
            {
                var result = MessageBox.Show($"Удалить клиента '{selected.FullName}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Clients.Remove(selected);
                    _dbContext.SaveChanges();
                    LoadClients();
                    MessageBox.Show("Клиент удалён", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}