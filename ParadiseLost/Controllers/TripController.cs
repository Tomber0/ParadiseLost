using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParadiseLost.Data;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;

namespace ParadiseLost.Controllers
{
    public class TripController : Controller
    {
        ILogger<UserController> _logger;
        ApplicationDbContext _context;
        UserManager<User> _userManager;

        public TripController(ILogger<UserController> logger, ApplicationDbContext context, UserManager<User> userManager) 
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }
        public IActionResult Create() 
        {
            
            return View();
        }
        public IActionResult Edit() => View();

        [HttpPost]
        public async Task<IActionResult> CreateNew(TripCreationEditModel trip) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Persons.FirstOrDefaultAsync(u=> u.Id == userId);
            if (currentUser != null)
            {
                var currentUserCompany = currentUser.Company;
            }
            else 
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                var result = await _context.Trips.AddAsync(new Trip {
                    Id = Guid.NewGuid().ToString(),
                    Name = trip.Name, Description = trip.Description,
                    Location = new Location
                    { Id = Guid.NewGuid().ToString(),
                        City = trip.Location.City, Street = trip.Location.Street
                    },
                    Tags = trip.Tags,
                    Visible = trip.Visible,
                    //TripCompany =
                }) ;

                await _context.SaveChangesAsync();
                //Trip existingTrip = await  _context.Trips.FirstOrDefaultAsync(t => t.)               
            }
            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> EditTrip(TripCreationEditModel trip) 
        {
            if (ModelState.IsValid)
            {
                var baseTrip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == trip.Id);
                if (baseTrip != null) 
                {
                    baseTrip.Location.Street = trip.Location.Street;
                    baseTrip.Location.City = trip.Location.City;
                    baseTrip.Location.Coordinates = trip.Location.Coordinates;
                    baseTrip.Name = trip.Name;
                    baseTrip.Tags = trip.Tags;
                    baseTrip.Description = trip.Description;
                    baseTrip.Visible = trip.Visible;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> EditTrip(string id) 
        {
            if (id != null) 
            {
                var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id);
                if (trip != null) 
                {
                    TripCreationEditModel newTrip = new TripCreationEditModel
                    {
                        Description = trip.Description,
                        Location = new Location
                        {
                            City = trip.Location.City,
                            Coordinates = trip.Location.Coordinates,
                            Street = trip.Location.Street,
                            Id = trip.Location.Id
                        },
                        Name = trip.Name,
                        Tags = trip.Tags,
                        Visible = trip.Visible,
                        Id = trip.Id
                    };
                    return View(trip);
                }
                else 
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
