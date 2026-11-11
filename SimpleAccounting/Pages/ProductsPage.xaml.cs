using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class ProductsPage : Page
    {
        private readonly AppDbContext _dbContext;

        public ProductsPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsGrid.ItemsSource = _dbContext.Products.ToList();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditProductWindow();

            if (dialog.ShowDialog() == true && dialog.ResultProduct != null)
            {
                _dbContext.Products.Add(dialog.ResultProduct);
                _dbContext.SaveChanges();
                LoadProducts();
                MessageBox.Show("Товар успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                var dialog = new AddEditProductWindow(selected);

                if (dialog.ShowDialog() == true && dialog.ResultProduct != null)
                {
                    // Товар уже обновлён в диалоге, нужно только сохранить
                    _dbContext.Products.Update(dialog.ResultProduct);
                    _dbContext.SaveChanges();
                    LoadProducts();
                    MessageBox.Show("Товар успешно обновлён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                var result = MessageBox.Show($"Удалить товар '{selected.Name}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Products.Remove(selected);
                    _dbContext.SaveChanges();
                    LoadProducts();
                    MessageBox.Show("Товар удалён", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}