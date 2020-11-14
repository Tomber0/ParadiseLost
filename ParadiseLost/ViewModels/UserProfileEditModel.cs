using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class UserProfileEditModel
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Укажите почту")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        //[Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Введенные пароли отличаются")]
        public string ComfirmPassword { get; set; }

        public string Name { get; set; }
        [MaxLength(250)]
        public string SName { get; set; }
        
        [MaxLength(50)]
        //
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

    }
}
