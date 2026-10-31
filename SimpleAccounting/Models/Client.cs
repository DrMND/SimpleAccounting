using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class Client
    {
        // Уникальный идентификатор клиента
        public int Id { get; set; }

        // ФИО клиента или Название организации
        public string FullName { get; set; } = string.Empty;

        // Номер телефона
        public string Phone { get; set; } = string.Empty;

        // Электронная почта
        public string Email { get; set; } = string.Empty;

        // ИНН (важно для бухгалтерии малого предприятия)
        public string INN { get; set; } = string.Empty;

        // Физический или юридический адрес
        public string Address { get; set; } = string.Empty;
    }
}
