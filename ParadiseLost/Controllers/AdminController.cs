using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParadiseLost.Models;

namespace ParadiseLost.Controllers
{
    public class AdminController : Controller
    {
        UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList()) ;
        }

        public IActionResult Create()=> View();
    }
}
