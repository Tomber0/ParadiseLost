using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.Models
{
    public class Message
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(250)]
        public Contact Invoker { get; set; }
        [MaxLength(250)]
        public Contact Reciver { get; set; }
        [MaxLength(250)]
        public Trip SelectedTrip { get; set; }
        public bool IsViewed { get; set; }
        [MaxLength(600)]
        public string Text { get; set; }
        [MaxLength(600)]
        public string AnswerText { get; set; }
    }
}
