using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QueueControlServer.Interfaces;
using QueueControlServer.Models;
using QueueControlServer.Services;
using QueueControlServer.ViewModels.Queue;
using QueueControlServer.ViewModels.QueuesList;

namespace QueueControlServer.Controllers
{
    [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
    public class QueuesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QueuesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Queues
        [Authorize(Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Queue.ToListAsync());

        }



        [Authorize(Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public async Task<IActionResult> QueuesList([FromServices] IGetUserClaim UserClaimService)
        {
            UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
            var gg = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
            var yy = UserClaimService.GetUserRole((ClaimsIdentity)User.Identity);
            if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "admin")
            {
                var tempView = await _context.Queue.ToListAsync();
                return View(tempView);
            }
            else
            {
                var tempView = await _context.Queue.Where(p => p.Factory.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)).ToListAsync();
                return View(tempView);
            }


            // return View(tempView);
        }




        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public async Task<IActionResult> QueuesPackages([FromServices] IGetUserClaim UserClaimService)  // Выборка Упаковок
        {
            UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);

            var QueuesPackage = new List<Package>();

            if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "admin")
            {

                var tempView = await _context.Queue.ToListAsync();
                foreach (var p in tempView)
                {
                    _context.Queue.Include(u => u.Package).Include(u => u.Package);
                    QueuesPackage.Add(p.Package);
                }
                return View(QueuesPackage.Distinct());
            }
            else
            {

                var tempView = await _context.Queue.Where(p => p.Factory.OrganizationName == UserClaimService
                .GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)).Include(u => u.Package).ToListAsync();

                if (UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity) != null)
                {
                   // tempView = (List<Queue>)tempView.Where(p => p.Factory.OrganizationName == UserClaimService
                   // .GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));

                    List<Guid> FactoryQ = UserClaimService
                           .GetUserClaim("TerminalQueues", (ClaimsIdentity)User.Identity).Split(",").ToList().ConvertAll(Guid.Parse);

                    tempView = tempView.Where(p => FactoryQ.Contains(p.QueueId)).ToList();
                }


                foreach (var p in tempView)
                {

                    QueuesPackage.Add(p.Package);
                }
                return View(QueuesPackage.Distinct());
            }

        }

        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public async Task<IActionResult> QueuesBrands([FromServices] IGetUserClaim UserClaimService, string PackageName)
        {

            if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "admin")
            {
                return View();
            }
            else
            {
                var tempView = await _context.Queue.Where(p => p.Factory.OrganizationName == UserClaimService
                .GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity) & p.Package.PackageName == PackageName).Include(u => u.Brand).ToListAsync();
               
                if (UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity) != null)
                {
                    // tempView = (List<Queue>)tempView.Where(p => p.Factory.OrganizationName == UserClaimService
                    // .GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));

                    List<Guid> FactoryQ = UserClaimService
                           .GetUserClaim("TerminalQueues", (ClaimsIdentity)User.Identity).Split(",").ToList().ConvertAll(Guid.Parse);

                    tempView = tempView.Where(p => FactoryQ.Contains(p.QueueId)).ToList();
                }
                return View(tempView);
            }
        }


        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        [HttpPost]
        public IActionResult GetQueue([FromServices] IGetUserClaim UserClaimService, string Factory, string FactoryName)
        {

            IQueryable<Queue> queues = _context.Queue;
            //.Where(p => p.Factory.OrganizationName== UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));  // Undone дорисовать фильтор для своих очередей 
            if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "admin")
            {

            }
            else if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "Supervisor")
            {
                if (UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)!=null)
                {
                    queues = queues.Where(p => p.Factory.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));
                }
                else if (UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity) != null)
                {
                   var itemCarries = _context.QueueItems.Where(p => p.CarrierName == UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity));

                    List<Guid> CarriesQ = new List<Guid>();
                   foreach (var item in itemCarries)
                    {
                        CarriesQ.Add(item.QueueId);
                    }
                  queues=queues.Where(p=> CarriesQ.Contains(p.QueueId));

                }



            }
            else
            {
                queues = queues.Where(p => p.Factory.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));

            }  // Undone дорисовать фильтор для своих очередей 





            if (FactoryName != null && FactoryName != "Все")
            {
                queues = queues.Where(p => p.Factory.OrganizationName == FactoryName);
            }

           /* else
            {
                queues = queues.Where(p => p.Factory.OrganizationName == FactoryName);   // HACK пока лищённое смысла условие, дописать

            }*/

            _context.Queue.Include(p => p.Factory.OrganizationName);

            var tempqueues = queues.ToList();
          //  tempqueues.Insert(0, new Queue { QueueName = "Все", QueueId = Guid.Empty });

            QueuesList viewModel = null; // new QueuesList
            /* {
                 Queue = tempqueues,
                 //  Queue.Insert(0, new Queue { QueueId = 0, QueueName = "Все" });

                 //   Factories =  new SelectList (factorry, "OrganizationName", "OrganizationId"),

             };*/


            //return Ok(viewModel);

            return Json(tempqueues);
        }




        // GET: Queues/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _context.Queue
                .FirstOrDefaultAsync(m => m.QueueId == id);
            if (queue == null)
            {
                return NotFound();
            }

            return View(queue);
        }

        // GET: Queues/Create
        public IActionResult Create()
        {
            IEnumerable<Factory> factories = _context.Factories.ToList();
            ViewBag.Factory = new SelectList(factories, "OrganizationId", "OrganizationName");

            IEnumerable<Brand> brands = _context.Brands.ToList();
            ViewBag.Brand = new SelectList(brands, "GoodsParameterId", "BrandName");

            IEnumerable<Package> packages = _context.Packages.ToList();
            ViewBag.Package = new SelectList(packages, "GoodsParameterId", "PackageName");
            return View();
        }

        // POST: Queues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQueueViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Factory = _context.Factories.Find(model.OrganizationId);
                model.Brand = _context.Brands.Find(model.GoodsParameterId);
                model.Package = _context.Packages.Find(model.PackageParameterId);

                model.QueueName = model.Brand.BrandName + model.Package.PackageName;


                // формирование имени очереди. 
                {
                    IQueryable<Queue> queues = _context.Queue;
                    queues = queues.Where(q => q.Factory == model.Factory);
                    queues = queues.Where(q => q.Brand == model.Brand);
                    queues = queues.Where(q => q.Package == model.Package);
                    var tempName = queues.Count() + 1;

                    model.QueueName = model.Brand.BrandName + " " + model.Package.PackageName + " № " + tempName;
                }


                Queue queue = new Queue { QueueName = model.QueueName, Factory = model.Factory, Brand = model.Brand, Package = model.Package };

                _context.Add(queue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Queues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _context.Queue.FindAsync(id);
            if (queue == null)
            {
                return NotFound();
            }
            return View(queue);
        }

        // POST: Queues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("QueueId,QueueName")] Queue queue)
        {
            if (id != queue.QueueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(queue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueueExists(queue.QueueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(queue);
        }

        // GET: Queues/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _context.Queue
                .FirstOrDefaultAsync(m => m.QueueId == id);
            if (queue == null)
            {
                return NotFound();
            }

            return View(queue);
        }

        // POST: Queues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var queue = await _context.Queue.FindAsync(id);
            _context.Queue.Remove(queue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueueExists(Guid id)
        {
            return _context.Queue.Any(e => e.QueueId == id);
        }

    }
}
