using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class UserOuterProfileChangeViewModel
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string SName { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
    }
}
