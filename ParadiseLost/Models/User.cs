using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class User : IdentityUser
    {
        //public override string Id { get => base.Id; set => base.Id = value; }        
        [MaxLength(50)]
        public Contact Contact { get; set; }
        [MaxLength(50)]
        public Company Company { get; set; }
    }
}
