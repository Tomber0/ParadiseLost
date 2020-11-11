using ParadiseLost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class CompanyCreateEditModel
    {
        public string Id { get; set; }
        //
        public string City { get; set; }
        public string Street { get; set; }
        public string Coordinates { get; set; }
        //
        public string Name { get; set; }// also for ContactName
        public string SName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
