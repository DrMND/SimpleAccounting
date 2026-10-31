using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAccounting.Models
{
    public class User
    {
        // Уникальный идентификатор пользователя
        public int Id { get; set; }

        // Логин для входа в систему
        public string Login { get; set; } = string.Empty;

        // Пароль (в учебном проекте храним строкой, для простоты)
        public string Password { get; set; } = string.Empty;

        // Роль пользователя: Администратор, Бухгалтер, Директор
        public string Role { get; set; } = string.Empty;
    }
}
