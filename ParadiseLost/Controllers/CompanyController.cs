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
    public class CompanyController : Controller
    {
        ILogger<UserController> _logger;
        ApplicationDbContext _context;
        UserManager<User> _userManager;

        public CompanyController(ILogger<UserController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() => View();
        public IActionResult Edit() => View();

        public async Task<IActionResult> RegisterNewCompany(CompanyCreateEditModel company) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id == userId);
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
                var result = await _context.Companies.AddAsync(new Company
                {
                    Name = company.Name,
                    Description = company.Description,
                    Contact = new  Contact
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = company.Email,
                        Phone = company.Phone
                    },
                    Id = Guid.NewGuid().ToString()
                }) ;
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditCompany(CompanyCreateEditModel company) 
        {
            if (company != null)
            {
                var oldCompany = await _context.Companies.FirstOrDefaultAsync(t => t.Id == company.Id);
                if (oldCompany != null) 
                {
                    oldCompany.Description = company.Description;
                    oldCompany.Contact.Phone = company.Phone;
                    oldCompany.Contact.Email = company.Email;
                    oldCompany.Name = company.Name;
                    oldCompany.Contact.Location.Street = company.Street;
                    oldCompany.Contact.Location.City = company.City;
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    return NotFound();

                }
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> EditCompany(string id) 
        {
            if (id != null) 
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c=> c.Id == id);
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

        public async Task<IActionResult> DeleteCompany(string id)
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

    }
}
