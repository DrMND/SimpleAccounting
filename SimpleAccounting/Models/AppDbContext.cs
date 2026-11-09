using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SimpleAccounting.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<User> Users { get; set; }
                public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=accounting.db");
        }

        public void CreateTestUsers()
        {
            if (!Users.Any(u => u.Login == "buh"))
            {
                Users.Add(new User
                {
                    Login = "buh",
                    Password = "123",
                    FullName = "Иванова Мария Петровна",
                    Role = "Бухгалтер"
                });
            }

            if (!Users.Any(u => u.Login == "director"))
            {
                Users.Add(new User
                {
                    Login = "director",
                    Password = "123",
                    FullName = "Петров Алексей Сергеевич",
                    Role = "Директор"
                });
            }

            SaveChanges();
        }
    }
}