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
using System;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel userModel)
        {
            if (ModelState.IsValid)
            {
                User oldUser = await _userManager.FindByIdAsync(userModel.Id);
                if (oldUser != null)
                {
                    oldUser.Email = userModel.Email;
                    oldUser.UserName = userModel.UserName;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }

            }
            return View(userModel);

        }
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                
            
            }
            return View(model);
        }
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var currentUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id == userId);

            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            UserEditModel userModel = new UserEditModel { Email = user.Email, UserName = user.UserName };
            return View(userModel);
        }
        public async Task<IActionResult> Profile() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Persons
                .Include(u => u.Contact)
                    .ThenInclude(c => c.Location)
                .FirstOrDefaultAsync(m => m.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            var userModel = new UserOuterProfileChangeViewModel()
            {
                City = user.Contact.Location.City,
                Name = user.Contact.Name,
                Phone = user.Contact.Phone,
                SName = user.Contact.SName,
                Street = user.Contact.Location.Street,
                Id = user.Id
            };
            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserOuterProfileChangeViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.Persons
                    .Include(u => u.Contact)
                        .ThenInclude(c => c.Location)
                    .FirstOrDefaultAsync(m => m.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }

                user.Contact.Location.City = model.City;
                user.Contact.Name = model.Name;
                user.Contact.Phone =model.Phone;
                user.Contact.SName = model.SName;
                user.Contact.Location.Street = model.Street;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index","User");

            }
            return View(model);
        }
        public async Task<IActionResult> CompanyLogin() => View();

        public async Task<IActionResult> Index()
        {
            //var currentUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id == userId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           // var userL = await _context.Persons.Join()
            var user = await _context.Persons
                .Include(u => u.Contact)
                    .ThenInclude(c=> c.Location)
                .FirstOrDefaultAsync(m => m.Id == userId);


            //var user = users.FirstOrDefault(m=> m.Id == userId);

 /*           if (user.Contact == null) 
            {
                user.Contact = new Contact 
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    Code = "",
                    Location = new Location()
                    {
                        Id = Guid.NewGuid().ToString(),
                        City = "",
                        Coordinates = "",
                        Street = ""
                    }
                        ,
                    Name = "",
                    Phone = "",
                    SName = ""
                };
            }
 */           
            await _context.SaveChangesAsync();
            if (user == null)
            {
                return NotFound();
            }
            var userModel = new UserOuterProfileChangeViewModel()
            {
                City = user.Contact.Location.City,
                Name = user.Contact.Name,
                Phone = user.Contact.Phone,
                SName = user.Contact.SName,
                Street = user.Contact.Location.Street,
                Id = user.Id
            };
            return View(userModel);
        }
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
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                User iUser = await _context.Persons.FirstOrDefaultAsync(u => userModel.Email == u.Email);
                if (iUser == null)
                {
                    User user = new Models.User{ Email = userModel.Email, UserName = userModel.UserName,Contact = new Contact()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = userModel.Email,
                        Code = "",
                        Location = new Location()
                        {
                            Id = Guid.NewGuid().ToString(),
                            City = "",
                            Coordinates = "",
                            Street = ""
                        }
                        ,
                        Name = "",
                        Phone = "",
                        SName = ""
                    }
                    };
                    var result = await _userManager.CreateAsync(user, userModel.Password);


                    if (result.Succeeded)
                    {
                        //if create account is successfull
                        //add to database

                        //_context.Persons.Add(user);

                        //await _context.Contacts.AddAsync(new Contact() {Id = Guid.NewGuid().ToString(),Email = user.Email});
                        var cUser = await _context.Persons.FirstOrDefaultAsync(u => u.Id ==  user.Id);
/*                        cUser.Contact = new Contact()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = user.Email,
                            Code = "",
                            Location = new Location()
                            {
                                Id = Guid.NewGuid().ToString(),
                                City = "",
                                Coordinates = "",
                                Street = ""
                            }
                        ,
                            Name = "",
                            Phone = "",
                            SName = ""
                        };
*/
                        
                        _context.SaveChanges();
                        await _signInManager.PasswordSignInAsync(userModel.UserName, userModel.Password, true, true);
                        return RedirectToAction("Index","User");
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
                    return RedirectToAction("Index", "User");
                }
                else 
                {
                    ModelState.AddModelError("","Неправильный логин \\пароль");              
                }            
            }
            return RedirectToAction("Login","User");
        }
        [Authorize]
        public async Task<IActionResult> Usermenu() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var currentUser = await _context.Persons
                                    .Include(u => u.Contact)

                        .FirstOrDefaultAsync(u => u.Id == userId);

                var userMessages = _context.Messages.Include(m=>m.Invoker).Include(m=>m.Reciver).Include(m=> m.SelectedTrip).Where(m => m.Invoker.Id == currentUser.Contact.Id);
                if (userMessages != null)
                {

                    List<MessageShowModel> messages = new List<MessageShowModel>();
                    foreach (var item in userMessages)
                    {
                        MessageShowModel mess = new MessageShowModel()
                        {
                            Id = item.Id,
                            IsViewed = item.IsViewed,
                            MessageAnswerText = item.AnswerText,
                            MessageText = item.Text,
                            SelectedTrip = item.SelectedTrip,
                            UserInvoker = item.Invoker
                        };
                        messages.Add(mess);
                    }
                    return View(messages);
                }
            }
            return View();
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