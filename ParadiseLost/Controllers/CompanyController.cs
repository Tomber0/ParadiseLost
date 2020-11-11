using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ParadiseLost.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() => View();
        public IActionResult Edit() => View();


        public Task<IActionResult> RegisterNewCompany() { }
        public Task<IActionResult> EditCompany() { }
        public Task<IActionResult> EditCompany(string id) { }

    }
}
