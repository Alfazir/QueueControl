using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QueueControlServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



namespace QueueControlServer.Controllers
{
    
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
      [Authorize(Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public IActionResult Index()
        { 
            if (HttpContext.User.IsInRole("Supervisor"))
            {
                return Redirect("/Home/IndexSupervizor");
            }
            if (HttpContext.User.IsInRole("Terminal"))
            {
                return View();
            }
            if (HttpContext.User.IsInRole("admin"))
            {
                return Redirect("/Home/IndexSupervizor");
            }

                return View();
        }

        [Authorize(Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public IActionResult IndexSupervizor()
        {
            return View();
        }


        public IActionResult IndexAdministration()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
