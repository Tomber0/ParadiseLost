using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParadiseLost.Models;
using ParadiseLost.ViewModels;

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
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel userModel) 
        {
            if (ModelState.IsValid) 
            {
                User oldUser = await _userManager.FindByIdAsync(userModel.Id);
                //User.HasClaim(ClaimTypes.Role)
                if (oldUser != null) 
                {
                    oldUser.Email = userModel.Email;
                    oldUser.UserName = userModel.UserName;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded) 
                    {
                        return RedirectToAction("Index","Admin");
                    }
                }

            }
            return View(userModel);

        }
        public async Task<IActionResult> Edit(string id) 
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            UserEditModel userModel = new UserEditModel {Email = user.Email, UserName = user.UserName };
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationModel userModel)
        {
            User userThatMayExist =  await _userManager.FindByEmailAsync(userModel.Email);
            if (userThatMayExist == null)
            {
                if (ModelState.IsValid)
                {
                    User user = new User { Email = userModel.Email, UserName = userModel.UserName };
                    var result = await _userManager.CreateAsync(user, userModel.Password);


                    if (result.Succeeded) 
                    {
                        RedirectToAction("Index");
                    }
                    //UserEditModel userModel = new UserEditModel { Email = user.Email, UserName = user.UserName };
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }


            }
            return View(userModel);
        }


        [HttpPost]
        public async Task <IActionResult> Delete(string id) 
        {
            if (ModelState.IsValid)
            {
                User oldUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(oldUser);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Create()=> View();
    }
}
