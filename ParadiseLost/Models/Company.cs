using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Company
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public Contact Contact { get; set; }
        [MaxLength(15)]
        public string Code { get; set; }
    }
}
