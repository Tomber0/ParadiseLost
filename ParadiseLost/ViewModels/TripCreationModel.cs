using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class TripCreationModel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Укажите название!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указано описание!")]
        public string Description { get; set; }
        public string Location { get; set; }

        public bool Visible { get; set; }

        public string Tags { get; set; }

    }
}
