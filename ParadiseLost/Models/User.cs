using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class User : IdentityUser
    {
        //public override string Id { get => base.Id; set => base.Id = value; }
        public Contact UserContact { get; set; }
        public Company Company { get; set; }
    }
}
