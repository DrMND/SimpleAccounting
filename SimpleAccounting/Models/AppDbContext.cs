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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=accounting.db");
        }

        // Добавь этот метод
        public void CreateTestUser()
        {
            // Проверяем, есть ли уже пользователи
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Login = "admin",
                    Password = "123",
                    FullName = "Администратор",
                    Role = "Admin"
                });
                SaveChanges();
            }
        }
    }
}