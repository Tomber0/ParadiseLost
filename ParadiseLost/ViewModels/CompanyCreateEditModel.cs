using ParadiseLost.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class CompanyCreateEditModel
    {
        public string Id { get; set; }
        //
        public string City { get; set; }
        public string Code { get; set; }
        public string Street { get; set; }
        //public string Coordinates { get; set; }
        //
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Укажите название!")]
        public string Name { get; set; }// also for ContactName
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
