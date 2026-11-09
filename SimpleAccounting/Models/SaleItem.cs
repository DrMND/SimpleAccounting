using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // Сколько штук продали
        public int Quantity { get; set; }

        // Цена за единицу на момент продажи
        public decimal Price { get; set; }

        // Сумма по этой позиции (Quantity * Price)
        public decimal TotalPrice => Quantity * Price;
    }
}