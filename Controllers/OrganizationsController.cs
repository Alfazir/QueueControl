using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QueueControlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.Controllers
{
    [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
    public class OrganizationsController : Controller
    {

        private readonly ApplicationDBContext _context;
        private readonly IHubContext<SignalServer> _signalHub;

        public OrganizationsController(ApplicationDBContext context, IHubContext<SignalServer> signalHub)
        {
            _context = context;
            _signalHub = signalHub;

        }
        /*  public async Task<IActionResult> Index()
          {
              return View(await _context.Factories.ToListAsync());
          } */

        public async Task<IActionResult> Index(string factory, string name)        // HACK временно, переписать метод!!1
        {
            var Factories = await _context.Factories.ToListAsync();
            IQueryable<User> users = _context.Terminals.Include(p => p.Factory);

            if (factory != null)
            {
                users = users.Where(p => p.Id == factory);
            }
            List<Factory> factories = _context.Factories.ToList();

            


            return View(factories);
        }

        [HttpGet]
        public IActionResult GetOrganization(string Factory, string Brand, string Package)
        {
            IQueryable<Factory> Factorys = _context.Factories;

            return Ok(Factorys);
        }

        public IActionResult CreateFactory() => View();

        public IActionResult CreateCarrier() => View();





        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateFactory")]        // url
        public async Task<IActionResult> Create([Bind("OrganizationName, OrganizationId")] Factory factory)
        {

            IQueryable<Factory> Factorys = _context.Factories;
            _context.Add(factory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateCarrier")]
        public async Task<IActionResult> Create([Bind("OrganizationName")] Carrier carrier)  // HACK  перегрузка для добавления предприятия
        {

            IQueryable<Carrier> carriers = _context.Carriers;
            _context.Add(carrier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
