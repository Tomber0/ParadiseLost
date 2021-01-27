using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
    public class CompanyController : Controller
    {
        ILogger<UserController> _logger;
        ApplicationDbContext _context;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<User> _signInManager;

        public CompanyController(SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager, ILogger<UserController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public async Task<IActionResult> Trips() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _context.Persons.Include(u=> u.Company).FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser != null)
            {
                var currentUserCompany = currentUser.Company;
                var compantTrips = await _context.Trips.Include(t=> t.Company).Include(t=> t.Image).Include(t=> t.Location).Where(t => t.Company.Id == currentUserCompany.Id).ToListAsync();
                var tripModel = new List<TripShowModel>();
                foreach (var item in compantTrips)
                {
                    var trip = new TripShowModel()
                    {
                        Id = item.Id,
                        Company = item.Company,
                        Description = item.Description,
                        Image = item.Image,
                        Location = item.Location,
                        Name = item.Name,
                        Tags = item.Tags
                    };
                    tripModel.Add(trip);
                }
                return View(tripModel);
            }
            else 
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        
        }
        [Authorize(Roles ="company")]
        public async Task <IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();

            var currentUser = await _context.Persons.Include(p => p.Contact).Include(p=> p.Company).ThenInclude(c=> c.Contact).FirstOrDefaultAsync(u => u.Id == userId);
            var a = User.IsInRole("company");
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (currentUser.Company.Id.Length ==0) 
            {
                return NotFound();
            }
            var allMessages = _context.Messages.Include(m => m.Invoker).ThenInclude(r=> r.Location).Include(m => m.Reciver).ThenInclude(r => r.Location).Include(m => m.SelectedTrip).Where(m => m.Reciver.Id == currentUser.Company.Contact.Id);
            var model = new List<MessageShowModel>();
            foreach (var item in allMessages)
            {
                var message = new MessageShowModel()
                {
                    Id = item.Id,
                    IsViewed = item.IsViewed,
                    MessageAnswerText = item.AnswerText,
                    MessageText = item.Text,
                    SelectedTrip = item.SelectedTrip,
                    UserInvoker = item.Invoker
                };
                model.Add(message);

            }
            return View(model);
        }
        [Authorize]
        public IActionResult Create() => View();
/*        [Authorize]
        public IActionResult Edit() => View();
*/
        [Authorize]
        public async Task<IActionResult> RegisterNewCompany(CompanyCreateEditModel company) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();

            var currentUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser != null)
            {
                //var currentUserCompany = currentUser.Company;
            }
            else
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                currentUser.Company = new Company
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = company.Name,
                    Description = company.Description,
                    Contact = new Contact
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = company.Name,
                        Email = company.Email,
                        Phone = company.Phone,
                        Location = new Location()
                        {
                            Id = Guid.NewGuid().ToString(),
                            City = company.City,
                            Street = company.Street
                        }
                    },
                    Code = GenerateCompanyCode()
                };
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);
                userRoles.Add("company");

                await _userManager.AddToRolesAsync(user, userRoles);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Company");

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CompanyCreateEditModel company) 
        {
            if (company != null)
            {
                var oldCompany = await _context.Companies.Include(c=>c.Contact).ThenInclude(c=> c.Location).FirstOrDefaultAsync(t => t.Id == company.Id);
                if (oldCompany != null) 
                {
                    oldCompany.Description = company.Description;
                    oldCompany.Contact.Phone = company.Phone;
                    oldCompany.Contact.Email = company.Email;
                    oldCompany.Name = company.Name;
                    oldCompany.Contact.Name = company.Name;
                    oldCompany.Contact.Location.Street = company.Street;
                    oldCompany.Contact.Location.City = company.City;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Company");

                }
                else 
                {
                    return NotFound();

                }
            }
            return RedirectToAction("Index", "Home");

        }
        public string GenerateCompanyCode() 
        {
            StringBuilder result = new StringBuilder();
            string letters = "ABCDEFGHIKLMNOPQRSTVXYZabcdefghijklmnopqrstuvwxyz!@#1234567890";
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                result.Append(letters[random.Next(0, letters.Length)]);
                //random.Next(0, letters.Length);
            }
            return result.ToString();
        }
        public async Task<IActionResult> EditC(string id) //
        {
            /*            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var currentUser = await _context.Persons.Include(p => p.Company).FirstOrDefaultAsync(u => u.Id == userId);
                        var companyId = await _context.Persons.Include(p => p.Company).ThenInclude(c => c.Contact).ThenInclude(c => c.Location).Where(currentUser.Company == );
            */
            if (id != null)
            {
                var company = await _context.Companies.Include(c=>c.Contact).ThenInclude(c=> c.Location).FirstOrDefaultAsync(c => c.Id == id);
                if (company != null)
                {
                    CompanyCreateEditModel newCompany = new CompanyCreateEditModel
                    {
                        City = company.Contact.Location.City,
                        Description = company.Description,
                        Email = company.Contact.Email,
                        Id = company.Id,
                        Name = company.Contact.Name,
                        Phone = company.Contact.Phone,
                        Street = company.Contact.Location.Street
                    };

                    return View(newCompany);
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="company")]
        public async Task<IActionResult> Edit() //
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Persons.Include(p => p.Company).FirstOrDefaultAsync(u => u.Id == userId);
            var id = currentUser.Company.Id;

            if (id != null)
            {
                var company = await _context.Companies.Include(c => c.Contact).ThenInclude(c => c.Location).FirstOrDefaultAsync(c => c.Id == id);
                if (company != null)
                {
                    CompanyCreateEditModel newCompany = new CompanyCreateEditModel
                    {
                        City = company.Contact.Location.City,
                        Description = company.Description,
                        Email = company.Contact.Email,
                        Id = company.Id,
                        Name = company.Name,
                        Phone = company.Contact.Phone,
                        Street = company.Contact.Location.Street,
                        Code = company.Code
                    };

                    return View(newCompany);
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Delete(string id)//
        {
            if (id != null) 
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c=> c.Id == id);
                var result = _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> CompanyLogin(UserCompanyLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    string code = model.Code;
                    if (code == null)
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        var userRoles = await _userManager.GetRolesAsync(user);
                        userRoles.Remove("company");

                        return RedirectToAction("Index", "Home");

                    }
                    var currentUser = await _context.Persons.
                            Include(u => u.Contact).
                            Include(u=> u.Company).
                        FirstOrDefaultAsync(u => u.Id == userId);
                    currentUser.Contact.Code = code;


                    var userTryingtoLoginCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Code == code);
                    if (userTryingtoLoginCompany != null)
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        var userRoles = await _userManager.GetRolesAsync(user);
                        userRoles.Add("company");
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        await _signInManager.SignInAsync(user, true);
                        await _userManager.AddToRolesAsync(user, userRoles);
                        if (user.Company == null)
                            user.Company = userTryingtoLoginCompany;

                        if (currentUser.Company.Id == userTryingtoLoginCompany.Id)
                        {
                            await _context.SaveChangesAsync();

                            return RedirectToAction("Index", "Company");
                        }
                        currentUser.Company = userTryingtoLoginCompany;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Company");
                    }
                    else 
                    {

                        var user = await _userManager.FindByIdAsync(userId);
/*
                        var userRoles = await _userManager.GetRolesAsync(user);

                        var newRoles = userRoles.Remove("company");

*/                        await _userManager.AddToRoleAsync(user, "company");

                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
