using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        IWebHostEnvironment _appEnvironment;
        public TripController(ILogger<UserController> logger, ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment appEnvironment) 
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _appEnvironment = appEnvironment;

        }
        public IActionResult Create() => View();
        public async Task<IActionResult> Panel(string id) 
        {
            var tripDb = await _context.Trips.Include(t => t.Company).ThenInclude(c=> c.Contact).Include(t => t.Image).Include(t=> t.Location).FirstOrDefaultAsync(t=> t.Id == id);

            var trip = new TripShowModel()
            {
                Id = tripDb.Id,
                Company = tripDb.Company,
                Description =tripDb.Description,
                Image = tripDb.Image,
                Location=tripDb.Location,
                Name= tripDb.Name
            };
            return View(trip);
        }

        //add only role + company
        //role = company
        [HttpPost]
        public async Task<IActionResult> Create(TripCreationEditModel trip) 
        {
            if (ModelState.IsValid) 
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Images imgs = null;
                var currentUser = await _context.Persons.Include(u=> u.Company).ThenInclude(c=> c.Contact).ThenInclude(c=> c.Location).FirstOrDefaultAsync(u => u.Id == userId);
                if (currentUser.Company != null)
                {
                    string path = "";
                    var imageFile = trip.Image;
                    if (imageFile != null)
                    {
                        path = "/img/Trip/" + imageFile.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        Images img = new Images() 
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = trip.Name,
                            Path = path 
                        };
                        imgs = img;
                        await _context.Images.AddAsync(img);
                    }
                    var currentUserCompany = currentUser.Company;
                    var result = await _context.Trips.AddAsync(new Trip
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = trip.Name,
                        Description = trip.Description,
                        Location = new Location
                        {
                            Id = Guid.NewGuid().ToString(),
                            City = trip.Location.City,
                            Street = trip.Location.Street
                        },
                        Tags = trip.Tags,
                        Visible = trip.Visible,
                        Company = currentUserCompany,
                        Image = imgs,
                    });
                }
                else
                {
                    return NotFound();
                }
                _logger.LogInformation($"Trp-created\n{trip.Id}");

                await _context.SaveChangesAsync();
                    //Trip existingTrip = await  _context.Trips.FirstOrDefaultAsync(t => t.)               
            }
            return RedirectToAction("Trips", "Company");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TripCreationEditModel trip) 
        {
            if (ModelState.IsValid)
            {
                var baseTrip = await _context.Trips.Include(t=> t.Image).Include(t=> t.Location).FirstOrDefaultAsync(t => t.Id == trip.Id);
                if (baseTrip != null) 
                {
                    baseTrip.Location.Street = trip.Location.Street;
                    baseTrip.Location.City = trip.Location.City;
                    baseTrip.Location.Coordinates = trip.Location.Coordinates;
                    baseTrip.Name = trip.Name;
                    baseTrip.Tags = trip.Tags;
                    baseTrip.Description = trip.Description;
                    baseTrip.Visible = trip.Visible;
                    if (trip.Image!=null)
                    {
                    var imageFile = trip.Image;
                    var path = "/img/Trip/" + imageFile.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                    baseTrip.Image.Path = path;
                    }
                    else
                    {
                        baseTrip.Image.Path = trip.Imagestr;
                    }

                }
            }
            _logger.LogInformation($"Trp-changed-\n{trip.Id}");

            _context.SaveChanges();
            return RedirectToAction("Trips", "Company");
        }
        public async Task<IActionResult> Edit(string id) 
        {
            if (id != null) 
            {
                var trip = await _context.Trips.Include(t=> t.Image).Include(t=> t.Location).FirstOrDefaultAsync(t => t.Id == id);
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
                        Id = trip.Id,
                        Imagestr
                        = trip.Image.Path,
                    };
                    return View(newTrip);
                }
                else 
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {
                var trip = await _context.Trips.Include(t=> t.Location).Include(t=> t.Image).FirstOrDefaultAsync(c => c.Id == id);

                var messages = _context.Messages.Include(m => m.SelectedTrip).Where(m => m.SelectedTrip == trip).ToArray();
                _context.Messages.RemoveRange(messages);
                var result = _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction("Trips", "Company");
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
