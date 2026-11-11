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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class EmployeesPage : Page
    {
        private readonly AppDbContext _dbContext;

        public EmployeesPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            EmployeesGrid.ItemsSource = _dbContext.Employees.ToList();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditEmployeeWindow();

            if (dialog.ShowDialog() == true && dialog.ResultEmployee != null)
            {
                _dbContext.Employees.Add(dialog.ResultEmployee);
                _dbContext.SaveChanges();
                LoadEmployees();
                MessageBox.Show("Сотрудник успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem is Employee selected)
            {
                var dialog = new AddEditEmployeeWindow(selected);

                if (dialog.ShowDialog() == true && dialog.ResultEmployee != null)
                {
                    _dbContext.Employees.Update(dialog.ResultEmployee);
                    _dbContext.SaveChanges();
                    LoadEmployees();
                    MessageBox.Show("Сотрудник успешно обновлён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem is Employee selected)
            {
                var result = MessageBox.Show($"Удалить сотрудника '{selected.FullName}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Employees.Remove(selected);
                    _dbContext.SaveChanges();
                    LoadEmployees();
                    MessageBox.Show("Сотрудник удалён", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}