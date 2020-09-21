using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ParadiseLost.Controllers
{
    public class AdminController : Controller
    {
        UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager) 
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users?.ToList()) ;
            return View();
        }
    }
}
