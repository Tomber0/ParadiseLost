using ParadiseLost.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost.ViewModels
{
    public class MessageShowModel
    {
        public string Id { get; set; }
        [MaxLength(250)]
        public User UserInvoker { get; set; }
        [MaxLength(250)]
        public Trip SelectedTrip { get; set; }
        public bool IsViewed { get; set; }
        [MaxLength(600)]
        public string MessageText { get; set; }
        [MaxLength(600)]
        public string MessageAnswerText { get; set; }

    }
}
