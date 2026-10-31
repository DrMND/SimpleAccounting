using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleAccounting.Models
{
    public class AppDbContext : DbContext
    {
        // Каждое свойство DbSet — это будущая таблица в базе данных SQLite
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<User> Users { get; set; }


        // Метод настройки подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Говорим Entity Framework, что мы используем локальную БД SQLite
            // Файл базы данных будет называться "accounting.db" и автоматически создастся в папке с запущенной программой
            optionsBuilder.UseSqlite("Data Source=accounting.db");
        }
    }
}
