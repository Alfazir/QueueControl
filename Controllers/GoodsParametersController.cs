using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QueueControlServer.Models;
using QueueControlServer.ViewModels.GoodsParameter;


namespace QueueControlServer.Controllers
{
    [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,LocalAdmin")]
    public class GoodsParametersController : Controller
    {
        private readonly ApplicationDBContext _context;
       // private readonly IHubContext<SignalServer> _signalHub;
        public GoodsParametersController(ApplicationDBContext context/* ,IHubContext<SignalServer> signalHub*/)
        {
            _context = context;
         //   _signalHub = signalHub;
        }

        // GET: GoodsParameters
        public async Task<IActionResult> Index()   
        {
            List<Brand> brands = _context.Brands.ToList();

            List<Package> packages = _context.Packages.ToList();

            GoodsParameterViewModel viewModel = new GoodsParameterViewModel
            {


                Brands = brands,
                Packages = packages
            };
            return View(viewModel);
            // return View(await _context.GoodsParameters.ToListAsync());
            // return View();
        }

        // GET: GoodsParameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsParameter = await _context.GoodsParameters
                .FirstOrDefaultAsync(m => m.GoodsParameterId == id);
            if (goodsParameter == null)
            {
                return NotFound();
            }

            return View(goodsParameter);
        }

        // GET: GoodsParameters/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateBrand()
        {
            return View();
        }
        public IActionResult CreatePackage()
        {
            return View();
        }

        // POST: GoodsParameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsParameterId")] GoodsParameter goodsParameter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goodsParameter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateBrand")]
        public async Task<IActionResult> Create([Bind("GoodsParameterId, BrandName")] Brand brand)
        {
           

            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreatePackage")]
        public async Task<IActionResult> Create([Bind("GoodsParameterId, PackageName")] Package package)
        {


            if (ModelState.IsValid)
            {
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // GET: GoodsParameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsParameter = await _context.GoodsParameters.FindAsync(id);
            if (goodsParameter == null)
            {
                return NotFound();
            }
            return View(goodsParameter);
        }

        // POST: GoodsParameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoodsParameterId")] GoodsParameter goodsParameter)
        {
            if (id != goodsParameter.GoodsParameterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsParameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsParameterExists(goodsParameter.GoodsParameterId))
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
            return View(goodsParameter);
        }

        // GET: GoodsParameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsParameter = await _context.GoodsParameters
                .FirstOrDefaultAsync(m => m.GoodsParameterId == id);
            if (goodsParameter == null)
            {
                return NotFound();
            }

            return View(goodsParameter);
        }

        // POST: GoodsParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsParameter = await _context.GoodsParameters.FindAsync(id);
            _context.GoodsParameters.Remove(goodsParameter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsParameterExists(int id)
        {
            return _context.GoodsParameters.Any(e => e.GoodsParameterId == id);
        }
    }
}
