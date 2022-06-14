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

namespace QueueControlServer.ViewModels.QueuesList
{
    

    public class QueuesList:ViewComponent
    {

        private readonly ApplicationDBContext _context;
        private readonly IGetUserClaim _UserClaimService;
       
        public QueuesList (ApplicationDBContext context, IGetUserClaim UserClaimService)
        {
            _context = context;
            _UserClaimService = UserClaimService;
        }
      

        [Authorize(Roles = "admin,user,factory,terminal")]
        public async Task<IViewComponentResult> InvokeAsync()   // HACK задел асин метод под автообновления SignalHab
        {

            _UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)UserClaimsPrincipal.Identity);
            var gg = _UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)UserClaimsPrincipal.Identity);
            var yy = _UserClaimService.GetUserRole((ClaimsIdentity)UserClaimsPrincipal.Identity);
            if (_UserClaimService.GetUserRole((ClaimsIdentity)UserClaimsPrincipal.Identity) == "admin")
            {
                var tempView = await _context.Queue.ToListAsync();
                return View(tempView);
            }

            #region выборка для перевозчиков
            else if (_UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "Supervisor")
            {
                IQueryable<QueueControlServer.Models.Queue> queues = _context.Queue;

                if (_UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity) != null)
                {
                    queues = queues.Where(p => p.Factory.OrganizationName == _UserClaimService
                    .GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity));

                 //List<Guid> FactoryQ = _UserClaimService
                //        .GetUserClaim("TerminalQueues", (ClaimsIdentity)User.Identity).Split(",").ToList().ConvertAll(Guid.Parse);


                }
                else if (_UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity) != null)
                {
                    var itemCarries = _context.QueueItems.Where(p => p.CarrierName == _UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity));

                    List<Guid> CarriesQ = new List<Guid>();
                    foreach (var item in itemCarries)
                    {
                        CarriesQ.Add(item.QueueId);
                    }
                    queues = queues.Where(p => CarriesQ.Contains(p.QueueId));

                }

                return View(queues);

            }

            #endregion


            else
            {
                var tempView = await _context.Queue.Where(p => p.Factory.OrganizationName == _UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)UserClaimsPrincipal.Identity)).ToListAsync();
                return View(tempView);
            }

           // var item =  _context.Queue.ToList();
          //  return View(item);  
        }

    }
}

