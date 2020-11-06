using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class UserEditModel
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Укажите почту")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}
