using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ParadiseLost.Models;

namespace ParadiseLost.Controllers
{
    public class UserController : Controller
    {

        UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        public async Task<IActionResult> Create(IdentityUser userModel) 
        {
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

        
        
        }



    }
}
