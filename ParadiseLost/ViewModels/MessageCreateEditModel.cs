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
        public string SelectedTripId { get; set; }
        public bool IsViewed { get; set; }
        [MaxLength(600)]
        public string MessageText { get; set; }
        [MaxLength(70)]
        public Contact Invoker { get; set; }
        public string InvokerId { get; set; }

        [MaxLength(70)]
        public Contact Reciver { get; set; }
        public string ReciverId { get; set; }
        public string ReciverName { get; set; }
        public string Answer { get; set; }

        public string InvokerStreet { get; set; }
        public string InvokerCity { get; set; }
    }
}
