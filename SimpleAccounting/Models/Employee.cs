using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Employee
    {
        // Уникальный идентификатор сотрудника
        public int Id { get; set; }

        // Полное имя (ФИО)
        public string FullName { get; set; } = string.Empty;

        // Должность
        public string Position { get; set; } = string.Empty;

        // Оклад (ставка)
        public decimal Salary { get; set; }
    }
}
