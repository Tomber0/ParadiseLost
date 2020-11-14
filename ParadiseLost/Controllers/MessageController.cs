using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParadiseLost.Data;
using ParadiseLost.Models;

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


        public IActionResult Index()
        {
            return View();
        }



    }
}
