using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Product
    {
        // Уникальный идентификатор (Entity Framework автоматически сделает его Primary Key в SQLite)
        public int Id { get; set; }

        // Название товара
        public string Name { get; set; } = string.Empty;

        // Цена товара (для денег всегда используем decimal, чтобы не было ошибок округления)
        public decimal Price { get; set; }

        // Количество на складе
        public int Quantity { get; set; }

        // Категория товара
        public string Category { get; set; } = string.Empty;
    }
}
