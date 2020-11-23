using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using ParadiseLost.Data;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;

namespace ParadiseLost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string tags) 
        {
            var trips = await _context.Trips.ToListAsync();
            if (tags != null)
            {
                trips = await _context.Trips.Include(t => t.Image).Include(t => t.Company).ThenInclude(c => c.Contact).Include(t => t.Location).Where(t => t.Visible == true && (t.Tags.Contains(tags) || (t.Location.City.Contains(tags)) || (t.Location.Street.Contains(tags))|| (t.Description.Contains(tags))) ).ToListAsync();

            }
            else
            {
                trips = await _context.Trips.Include(t => t.Image).Include(t => t.Company).ThenInclude(c => c.Contact).Include(t => t.Location).Where(t => t.Visible == true).ToListAsync();

            }
            var listOfTrips = new List<TripShowModel>();
            foreach (var item in trips)
            {
                var trip = new TripShowModel() { Image = item.Image, Id = item.Id, Company = item.Company, Description = item.Description, Location = item.Location, Name = item.Name, Tags = item.Tags };
                listOfTrips.Add(trip);
            }

            return View(listOfTrips);
        }
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips.Include(t => t.Image).Include(t => t.Company).ThenInclude(c => c.Contact).Include(t => t.Location).Where(t=> t.Visible== true).ToListAsync();
            var listOfTrips = new List<TripShowModel>();
            foreach (var item in trips)
            {
                var trip = new TripShowModel() {Image = item.Image,Id =item.Id,Company = item.Company,Description = item.Description,Location = item.Location,Name = item.Name,Tags = item.Tags };
                listOfTrips.Add(trip);
            }
            
            return View(listOfTrips);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignIn() 
        {
            return View();
        }

        //Id=Guid.NewGuid().ToString("N")
        public async Task< IActionResult> Register(UserRegistrationModel user) 
        {
            if (ModelState.IsValid) 
            {
                //User user = 
            
            }
            return View(user);

            //return Content($"User email => {user.Email},,, User name =>{user.UserName}");
                //Redirect("~/Home/Index");
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
