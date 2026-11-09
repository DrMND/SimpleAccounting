using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Sale
    {
        public int Id { get; set; }

        // Дата и время продажи
        public DateTime Date { get; set; } = DateTime.Now;

        // ID клиента (может быть null, если продажа "в никуда" / за наличные без клиента)
        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        // Общая сумма продажи
        public decimal TotalAmount { get; set; }

        // Список товаров в этой продаже
        public List<SaleItem> Items { get; set; } = new();
    }
}