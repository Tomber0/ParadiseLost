using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Company
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public Contact Contact { get; set; }
    }
}
