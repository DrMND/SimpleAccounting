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
using System.Linq;
using SimpleAccounting.Models;
using SimpleAccounting.Views;

namespace SimpleAccounting.Pages
{
    public partial class ExpensesPage : Page
    {
        private readonly AppDbContext _dbContext;

        public ExpensesPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadExpenses();
        }

        private void LoadExpenses()
        {
            ExpensesGrid.ItemsSource = _dbContext.Expenses
                .OrderByDescending(e => e.Date)
                .ToList();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditExpenseWindow();

            if (dialog.ShowDialog() == true && dialog.ResultExpense != null)
            {
                _dbContext.Expenses.Add(dialog.ResultExpense);
                _dbContext.SaveChanges();
                LoadExpenses();
                MessageBox.Show("Расход успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            if (ExpensesGrid.SelectedItem is Expense selected)
            {
                var dialog = new AddEditExpenseWindow(selected);

                if (dialog.ShowDialog() == true && dialog.ResultExpense != null)
                {
                    _dbContext.Expenses.Update(dialog.ResultExpense);
                    _dbContext.SaveChanges();
                    LoadExpenses();
                    MessageBox.Show("Расход успешно обновлён!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите расход для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            if (ExpensesGrid.SelectedItem is Expense selected)
            {
                var result = MessageBox.Show($"Удалить расход '{selected.Category}' на сумму {selected.Amount:N2} ₽?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Expenses.Remove(selected);
                    _dbContext.SaveChanges();
                    LoadExpenses();
                    MessageBox.Show("Расход удалён", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите расход для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}