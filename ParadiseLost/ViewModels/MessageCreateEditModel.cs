using ParadiseLost.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class MessageCreateEditModel
    {
        public string Id { get; set; }
        [MaxLength(250)]
        public Trip SelectedTrip { get; set; }
        public bool IsViewed { get; set; }
        [MaxLength(600)]
        public string MessageText { get; set; }
        [MaxLength(250)]
        public User Invoker { get; set; }
        [MaxLength(250)]
        public Company Reciver { get; set; }
    }
}
