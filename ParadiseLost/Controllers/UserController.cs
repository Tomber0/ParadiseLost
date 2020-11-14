using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using ParadiseLost.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ParadiseLost.Controllers
{
    public class UserController : Controller
    {
        SignInManager<User> _signInManager;
        ILogger<UserController> _logger;
        UserManager<User> _userManager;
        ApplicationDbContext _context;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, 
            ILogger<UserController> logger, ApplicationDbContext context
            )
        {
            
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();
        public IActionResult Login() => View();
        //@Html.TextBoxFor(m => m.Password)
        //@Html.TextBoxFor(m => m.ComfirmPassword)
        //
        //
        public IActionResult Registrtion() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewUser(UserRegistrationModel userModel) 
        {
            if (ModelState.IsValid)
            {
                User iUser = await _context.Persons.FirstOrDefaultAsync(u => userModel.Email == u.Email);
                if (iUser == null)
                {
                    User user = new Models.User{ Email = userModel.Email, UserName = userModel.UserName };
                    var result = await _userManager.CreateAsync(user, userModel.Password);


                    if (result.Succeeded)
                    {
                        //if create account is successfull
                        //add to database
                                                
                        //_context.Persons.Add(user);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);

                            _logger.LogError($"error in creating new user, Code: {error.Code},\tDescription: {error.Description}");
                        }
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        private async Task Authenticate(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user.Email)

            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplictionCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> LoginIntoAccount(UserLoginModel userModel) 
        {
            if (ModelState.IsValid) { 
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                var result = await _signInManager.PasswordSignInAsync(userModel.UserName,userModel.Password,true,true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    ModelState.AddModelError("","Неправильный логин \\пароль");              
                }            
            }
            return RedirectToAction("Login","User");
        }
    }
}
/*
 
             if (ModelState.IsValid) 
            {
                var user = new IdentityUser { Email = userModel.Email ,UserName = userModel.UserName};
                var result = await _userManager.CreateAsync(user,userModel.PasswordHash);

                if (result.Succeeded) 
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(userModel);

        
        
 
 */