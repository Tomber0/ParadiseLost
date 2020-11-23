using ParadiseLost.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class TripShowModel
    {
        [Required]
        [MaxLength(255)]
        public string Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(950000)]
        public string Description { get; set; }
        [MaxLength(250)]
        public Location Location { get; set; }
        public Company Company { get; set; }
        [MaxLength(100)]
        public string Tags { get; set; }
        public Images Image { get; set; }
    }
}
