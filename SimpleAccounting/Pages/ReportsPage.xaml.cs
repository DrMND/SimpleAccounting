using SimpleAccounting.Models;
using SimpleAccounting.Views;
using System.Windows;
using System.Windows.Controls;  

namespace SimpleAccounting.Pages
{
    public partial class ReportsPage : Page
    {
        private readonly AppDbContext _dbContext;

        public ReportsPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void InventoryReport_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new InventoryReportWindow(_dbContext);
            reportWindow.ShowDialog();
        }

        private void SalesReport_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new SalesReportWindow(_dbContext);
            reportWindow.ShowDialog();
        }

        private void ProfitReport_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ProfitReportWindow(_dbContext);
            reportWindow.ShowDialog();
        }
    }
}