﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Trip
    {

        //поля для путёвки:
        //идентификатор: str
        //название: str  
        //описание: str
        [Key]
        [MaxLength(255)]
        public string Id  { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(950000)]
        public string Description { get; set; }
        [MaxLength(250)]
        public Location Location { get; set; }
        public Company TripCompany { get; set; }

        public bool Visible { get; set; }
        [MaxLength(3000)]
        public string Tags { get; set; }

    }
}
