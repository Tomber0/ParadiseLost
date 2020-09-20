using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Contact
    {
        
        
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string SName { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(250)]
        public string Email { get; set; }
        public Location ConatactLocation { get; set; }
            



    }
}
