using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;

namespace ParadiseLost.Controllers
{
    public class UserController : Controller
    {
        SignInManager<User> _signInManager;
        ILogger<UserController> _logger;
        UserManager<IdentityUser> _userManager;

        public UserController(SignInManager<User> signInManager, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        public async Task<IActionResult> Create(RegistrationModel userModel) 
        {
            
            if (ModelState.IsValid) 
            {
                var user = new IdentityUser { Email = userModel.Email, UserName = userModel.UserName };
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
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
            return View(userModel);
        }


        public async Task<IActionResult> Login(LoginModel userModel) 
        {
            if (ModelState.IsValid) { 
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                var result = await _signInManager.PasswordSignInAsync(userModel.UserName,userModel.Password,true,true);

                if (result.Succeeded)
                {
                    RedirectToAction("Index","Home");

                }
                else 
                {
                    ModelState.AddModelError("","Неправильный логин \\пароль");
                
                }
            
            }

            return View(userModel);
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