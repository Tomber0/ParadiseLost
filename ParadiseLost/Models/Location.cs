using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Location
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(50)]
        public string Coordinates { get; set; }


    }
}
