using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Expense
    {
        // Уникальный идентификатор расхода
        public int Id { get; set; }

        // Дата совершения операции
        public DateTime Date { get; set; } = DateTime.Now;

        // Сумма расхода
        public decimal Amount { get; set; }

        // Категория / Статья расходов (например: Аренда, Закупка сырья, Коммуналка)
        public string Category { get; set; } = string.Empty;

        // Дополнительный комментарий
        public string Description { get; set; } = string.Empty;
    }
}
