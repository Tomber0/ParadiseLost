using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParadiseLost.ViewModels
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}
