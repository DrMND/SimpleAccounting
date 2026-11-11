using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class UsersPage : Page
    {
        private readonly AppDbContext _dbContext;

        public UsersPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersGrid.ItemsSource = _dbContext.Users.ToList();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditUserWindow();

            if (dialog.ShowDialog() == true && dialog.ResultUser != null)
            {
                // Проверяем уникальность логина
                if (_dbContext.Users.Any(u => u.Login == dialog.ResultUser.Login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _dbContext.Users.Add(dialog.ResultUser);
                _dbContext.SaveChanges();
                LoadUsers();
                MessageBox.Show("Пользователь успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selected)
            {
                var dialog = new AddEditUserWindow(selected);

                if (dialog.ShowDialog() == true && dialog.ResultUser != null)
                {
                    // Проверяем уникальность логина (если логин изменился)
                    if (selected.Login != dialog.ResultUser.Login &&
                        _dbContext.Users.Any(u => u.Login == dialog.ResultUser.Login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    _dbContext.Users.Update(dialog.ResultUser);
                    _dbContext.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("Пользователь успешно обновлён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selected)
            {
                // Нельзя удалить самого себя
                if (selected.Login == "director" || selected.Login == "buh")
                {
                    MessageBox.Show("Нельзя удалить системного пользователя", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить пользователя '{selected.FullName}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Users.Remove(selected);
                    _dbContext.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("Пользователь удалён", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}