using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Tax
    {
        // Уникальный идентификатор налоговой записи
        public int Id { get; set; }

        // Налоговый период (например: "1 квартал 2026", "Год 2026")
        public string Period { get; set; } = string.Empty;

        // Сумма исчисленного налога
        public decimal Amount { get; set; }

        // Тип налога (например: УСН Доходы, Страховые взносы)
        public string Type { get; set; } = string.Empty;
    }
}
