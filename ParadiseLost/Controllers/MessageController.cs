using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParadiseLost.Data;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;

namespace ParadiseLost.Controllers
{
    public class MessageController : Controller
    {
        ILogger<UserController> _logger;
        ApplicationDbContext _context;
        UserManager<User> _userManager;

        public MessageController(ILogger<UserController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }
        //
        // userpage -> messages
        public async Task <IActionResult> Index()
        {
            
            var allUserMessages = _context.Messages.ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();
            var currentUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser != null) 
            {
                var thisUserMessages = allUserMessages.Where(m => m.Invoker.Id == currentUser.Id);
                List<MessageShowModel> userMessages = new List<MessageShowModel>();
                foreach (var item in thisUserMessages)
                {
                    MessageShowModel message = new MessageShowModel { Id = item.Id, IsViewed = item.IsViewed, SelectedTrip = item.SelectedTrip };
                    userMessages.Add(message);
                }
                return View(userMessages);
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task <IActionResult> Create(string id) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Persons.Include(u => u.Contact).ThenInclude(c=> c.Location).FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser == null)
            {
                return NotFound();
            }
            var selTrip = await _context.Trips.Include(t=> t.Company).ThenInclude(c=> c.Contact).FirstOrDefaultAsync(t => t.Id == id);
            MessageCreateEditModel message = new MessageCreateEditModel()
            {
                MessageText = "",
                Answer = "",
                IsViewed = false,
                Id = selTrip.Id,
                SelectedTrip = selTrip,
                Reciver = selTrip.Company.Contact
                , Invoker = currentUser.Contact,
                InvokerId = currentUser.Contact.Id,
                ReciverId = selTrip.Company.Contact.Id,
                InvokerStreet = currentUser.Contact.Location.Street,
                InvokerCity = currentUser.Contact.Location.City
            };
            //Message mesg = new Message() { Id = message.Id,Text="",Invoker= currentUser.Contact, AnswerText="", SelectedTrip = selTrip, Reciver = selTrip.Company.Contact};
            //await _context.Messages.AddAsync(mesg);
            _context.SaveChanges();
            return View(message); 
        }
        //public IActionResult Edit() => View();
        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateEditModel messageModel) 
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _context.Persons.Include(u=> u.Contact).FirstOrDefaultAsync(u => u.Id == userId);
                if (userId == null)
                    return NotFound();
                if (messageModel.MessageText == null) 
                {
                    return RedirectToAction("Index", "Home");

                }
                var companyContact = await _context.Companies.Include(c=> c.Contact).FirstOrDefaultAsync(c => messageModel.ReciverId == c.Contact.Id);
                var selTrip = await _context.Trips.FirstOrDefaultAsync(c=> c.Id==messageModel.Id);
                //var message = await _context.Messages.Include(m => m.Invoker).Include(m => m.Reciver).Include(m => m.SelectedTrip).ThenInclude(t => t.Company).ThenInclude(t => t.Contact).FirstOrDefaultAsync(t=>t.Id == messageModel.Id);
                Message message = new Message()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = messageModel.MessageText,
                    Invoker = currentUser.Contact,
                    AnswerText = "",
                    SelectedTrip = selTrip,
                    Reciver = companyContact.Contact
                };
                await _context.Messages.AddAsync(message);

                if (currentUser != null)
                {
                        message.Text = messageModel.MessageText;
                        message.IsViewed = false;
                    message.Invoker = currentUser.Contact;
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    return RedirectToAction("Index","Home");
                }

            }
            return RedirectToAction("Create","Message");
        }
        public async Task<IActionResult> Edit(string id) 
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (!message.IsViewed)
            {
                if (message != null)
                {
                    var newMessageModel = new MessageCreateEditModel()
                    {
                        Id = message.Id,
                        Invoker = message.Invoker,
                        IsViewed = message.IsViewed,
                        MessageText = message.Text,
                        Reciver = message.Reciver,
                        SelectedTrip = message.SelectedTrip
                    };
                    return View(message);
                }

            }
            return RedirectToAction("Index", "Message");
        }
        public async Task<IActionResult> Edit(MessageCreateEditModel newMessage)//for user
        {
            if (ModelState.IsValid) 
            {

                var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == newMessage.Id);
                message.Text = newMessage.MessageText;

                return RedirectToAction("Index", "Message");
            }
            return RedirectToAction("Index", "Message");

        }
        public async Task<IActionResult> Delete(string id) 
        {
            if (id != null)
            {
                var message = await _context.Messages.FirstOrDefaultAsync(c => c.Id == id);
                var result = _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Message");
            }
            return RedirectToAction("Index", "Message");

        }
        public async Task<IActionResult> Answer(string id) 
        {
            if (id!=null) 
            {
                var message = await _context.Messages.Include(m => m.Invoker).ThenInclude(r=> r.Location).Include(m => m.Reciver).ThenInclude(r=> r.Location).Include(m => m.SelectedTrip).FirstOrDefaultAsync(t=> t.Id==id);
                var model = new MessageCreateEditModel()
                {
                Id = message.Id,
                Reciver = message.Reciver,
                Invoker=message.Invoker,
                MessageText=message.Text,
                SelectedTrip=message.SelectedTrip
                };
                return View(model);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Answer(MessageCreateEditModel messageModel)//(answer)for company
        {
            if (ModelState.IsValid)
            {

                var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageModel.Id);
                message.AnswerText = messageModel.Answer;
                message.IsViewed = true;
                _context.SaveChanges();
                return RedirectToAction("Index", "Company");//
            }
            return RedirectToAction("Index", "Message");
        }
    }
}
