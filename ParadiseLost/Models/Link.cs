using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Link
    {

        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string UserId { get; set; }
        [MaxLength(50)]
        public string CompanyId { get; set; }


    }
}
