using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QueueControlServer.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using RestSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Web;
using RestSharp.Deserializers;
using Newtonsoft.Json.Linq;
using QueueControlServer.ViewModels;
using QueueControlServer.ViewModels.QueueItems;
using QueueControlServer.Interfaces;
using Microsoft.Extensions.Options;
using QueueControlServer.Data.Services;

namespace QueueControlServer.Controllers
{
    [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
    public class QueueItemsController : Controller
    {
     

        private readonly ApplicationDBContext _context;
        private readonly IHubContext<SignalServer> _signalHub;
        private readonly IOptions<MyAppSettings> _options;
        // private string UrlDecode;

        public QueueItemsController(ApplicationDBContext context, IHubContext<SignalServer> signalHub, IOptions<MyAppSettings> options)
        {
            _context = context;
            _signalHub = signalHub;
            _options = options;
        }

        // GET: QueueItems
        [Authorize(Roles = "admin,Terminal,Supervisor,LocalAdmin")]

        public async Task<IActionResult> Index()
        {

            List<Factory> factorys = _context.Factories.ToList();
            factorys.Insert(0, new Factory { OrganizationId = new Guid("5b8f190e-d679-498a-8e09-90cafd23683f"), OrganizationName = "Все" });
            List<Carrier> carriers = _context.Carriers.ToList();
            carriers.Insert(0, new Carrier { OrganizationId = new Guid("5b8f190e-d679-498a-8e09-90cafd23683f"), OrganizationName = "Все" });

          ;


            QueueItemsListViewModel viewModel = new QueueItemsListViewModel
            {

                QueueItems = await _context.QueueItems.ToListAsync(),
                Factories = new SelectList(factorys, "OrganizationId", "OrganizationName"),
                Carriers = new SelectList(carriers, "OrganizationId", "OrganizationName")
            };
            return View(viewModel);

        }

        public async Task<IActionResult> QueueItemsList(string QueueName, string Factory)
        {

            List<Factory> factorys = _context.Factories.Where(p => p.OrganizationName == Factory).ToList();
            // factorys.Insert(0, new Factory { OrganizationId = 0, OrganizationName = "Все" });
            List<Carrier> carriers = _context.Carriers.ToList();
            carriers.Insert(0, new Carrier { OrganizationId = new Guid("5b8f190e-d679-498a-8e09-90cafd23683f"), OrganizationName = "Все" });

            List<Queue> queues = _context.Queue.Where(p => p.QueueName == QueueName).ToList();
            //IQueryable<QueueItems> queueItems = _context.QueueItems;

            //  queueItems = queueItems.Where(p => p.Factory.OrganizationName == Factory);


            QueueItemsListViewModel viewModel = new QueueItemsListViewModel
            {
                QueueNames = new SelectList(queues, "QueueId", "QueueName"),
                QueueItems = await _context.QueueItems.ToListAsync(),
                Factories = new SelectList(factorys, "OrganizationId", "OrganizationName"),
                Carriers = new SelectList(carriers, "OrganizationId", "OrganizationName")
            };
            return View(viewModel);

        }

        [HttpGet]

       


       
        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        [HttpPost]
        public IActionResult GetQueueItems([FromServices] IGetUserClaim UserClaimService, string BrandName, string PackageName, string FactoryName, string CarrierName, string Search, string QueueName, List <string> QueueList)   // TODO будем передавать методу параметры и выводить списки в зависимости от их значений. ПЕредаём: 1) роль текущего пользователя, марку цемента, упаковку 
        {

            {                                                              
                string Role = UserClaimService.GetUserRole((ClaimsIdentity)User.Identity);


                //  Определение 
                if (Role != "admin")
                {
                    if (Role != "Carrier")
                    {
                        FactoryName = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
                        //  if (string.IsNullOrEmpty(FactoryName)) ;
                    }
                    if (Role != "Factory")
                    {
                        CarrierName = UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity);
                    }

                }

                IQueryable<QueueItems> queueItems = _context.QueueItems;
                // Ниже поиск по строке !
                {
                    if (!String.IsNullOrEmpty(Search))
                    {
                        var tempqueueItems = queueItems;
                        tempqueueItems = tempqueueItems.Where((p => p.CarNumber.Contains(Search)));
                        if (tempqueueItems.Count() == 0)
                        {
                            queueItems = queueItems.Where((p => p.DriverName.Contains(Search)));
                        }
                        else
                        {
                            queueItems = tempqueueItems;

                        }
                    }


                }

               /* if (Role != "admin") //   провера на администратора 
                {
                    queueItems = queueItems.Where(p => p.Factory.OrganizationName == FactoryName);
                }*/


                if (FactoryName != null && FactoryName != "Все")
                {
                    queueItems = queueItems.Where(p => p.FactoryName == FactoryName);
                }

                if (CarrierName != null && CarrierName != "Все")
                {
                    queueItems = queueItems.Where(p => p.CarrierName == CarrierName);
                }

               

                if (QueueName != null && QueueName != "Все")
                {
                    queueItems = queueItems.Where(p => p.QueueName == HttpUtility.UrlDecode(QueueName));
                }
               
               
                 if (QueueList.Count != 0)
                {
                    QueueList = JsonConvert.DeserializeObject<List<string>>(QueueList[0]);

                    IEnumerable<QueueItems> tempqueueItems = new List<QueueItems>();

                    foreach (var item in QueueList)
                    {
                        IEnumerable<QueueItems> queueItems1 = new List<QueueItems>();
                        queueItems1 = queueItems.Where(p => p.Queue.QueueId == Guid.Parse(item));
                        tempqueueItems = tempqueueItems.Concat(queueItems1);
                       
                    }
                  
                     queueItems = tempqueueItems.AsQueryable();

                }



                queueItems = from u in queueItems orderby u.OderLine select u;  // сортировка по порядку в очереди

                QueueItemsListViewModel viewModel = new QueueItemsListViewModel
                {
                    QueueItems = queueItems.ToList()

                    //   Factories =  new SelectList (factorry, "OrganizationName", "OrganizationId"),

                };

                return Ok(viewModel);
            }
            


        }



        // GET: QueueItems/Details/5
        public async Task<IActionResult> Details(Guid? QueueItemId)
        {
            if (QueueItemId == null)
            {

                return NotFound();
            }

            var queueItem = await _context.QueueItems
                .FirstOrDefaultAsync(m => m.QueueItemId == QueueItemId);
            if (queueItem == null)
            {
                return NotFound();
            }

            return View(queueItem);
        }

        // GET: QueueItems/Create
        public IActionResult Create()
        {
            // QueueItems c = _context.QueueItems.FirstOrDefault(m => m.Barecode == Barecode);

            return View();
        }
        public IActionResult CreateWithoutBarecode([FromServices] IGetUserClaim UserClaimService)
        {
            IEnumerable<Brand> brands = _context.Brands.ToList();
            ViewBag.Brand = new SelectList(brands, "GoodsParameterId", "BrandName");
            if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) == "admin")
            {
                IEnumerable<Queue> queues = _context.Queue.ToList();
                ViewBag.Queue = new SelectList(queues, "QueueId", "QueueName");
            }
            else
            {
                IEnumerable<Queue> queues = _context.Queue.Where(p => p.Factory.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)).ToList();
                ViewBag.Queue = new SelectList(queues, "QueueId", "QueueName");
            }

            return View();
        }


        [HttpPost]                    
                                       //   [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        // [Authorize(Roles = "admin,factory")]

        public async Task<IActionResult> Create([Bind("QueueItemsId,OderLine,CarNumber,Barecode,Factory,Brand")][FromServices] IGetUserClaim UserClaimService, QueueItems queueItem)  // стандартная генерация элемента в очереди, подразумевающая получение основных данных с сервера 1С 
        {

            if (ModelState.IsValid)
            {

            }

           
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == roleClaimType).ToList();


            {



                foreach (var tempitem in _context.QueueItems)
                {                                                      //HACK здесь  проверка на нахождение объекта в очереди. Вынести в отдельный класс, возможно в сервис.

                    if (queueItem.Barecode == tempitem.Barecode)
                    {
                        ModelState.AddModelError("Barecode", $"{tempitem.DriverName}, Вы уже зарегистрированны в очереди! Очередь:{tempitem.QueueName}." +
                            $" На данный момент Вы {tempitem.OderLine}-й в очереди. Если Вы ошиблись с очередью, то пожалуйсто отмените регистрацию " +
                            $"и попробуйте ещё раз.");
                        return Json($"Вы уже зарегистрированны!!! {tempitem.PackageName + tempitem.BrandName} Позиция {tempitem.OderLine}");
                    }

                }


                //TODO запрос данных и разбор массива.
                var addres = _options.Value.IntegrationAddress;
                var token = _options.Value.IntegrationToken;
                var PHPSESSID = _options.Value.PHPSESSID;

                var client = new RestClient($"{addres}={queueItem.Barecode}&token={token}"); //HACK вынести из контроллера в отдельный метод
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/plain");
                request.AddHeader("Cookie", $"PHPSESSID={PHPSESSID}");
                var body = @"This is expected to be sent back as part of response body.";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                try
                {
                    JObject.Parse(response.Content);
                }
                catch
                {
                    //   await Task.Delay(30000);
                    //  return PartialView();

                    return Json("ЗАЯВКА НЕ НАЙДЕНА! ");


                }

                /* if (response.Content.DefaultIfEmpty()==true)  // провкра на наличие данных в ответе от сайта
                 {

                    // Task.Delay(7000).Wait();
                     return Create(); // UNDONE дописать возврат модального окна с уведомлением!
                 }*/

                dynamic o = JObject.Parse(response.Content);  // UNDONE может вызывать исключение!
                dynamic h = null;
                foreach (JToken child in o.Children())
                {
                    var property = child as JProperty;
                    if (property != null)
                    {
                        h = property.Value;
                    }
                }

                if (h != null)
                {
                    //dynamic namej = h["VALUE"]["UF_TRANSPORTNOESREDS"];

                    queueItem.CarNumber = Convert.ToString(h["VALUE"]["UF_GOSNOMER"]);
                    queueItem.DriverName = Convert.ToString(h["VALUE"]["UF_VODITEL"]);
                    queueItem.PackageName = Convert.ToString(h["VALUE"]["UF_TIPPOGRUZKI"]);
                    queueItem.BrandName = Convert.ToString(h["VALUE"]["UF_MARKATSEMENTA"]);
                    queueItem.CarrierName = Convert.ToString(h["VALUE"]["ORGANIZATION"]);
                    queueItem.FactoryName = Convert.ToString(h["VALUE"]["PLACE_NAME"]);
                }



                queueItem.Pacakege = _context.Packages.Where(p => p.PackageName == queueItem.PackageName).FirstOrDefault();
                queueItem.Brand = _context.Brands.Where(p => p.BrandName == queueItem.BrandName).FirstOrDefault();
                queueItem.Factory = _context.Factories.Where(p => p.OrganizationName == queueItem.FactoryName).FirstOrDefault();

                #region Условия проверки наличия чередей с одинаковыми BrandName и PackageName

                if (_context.Queue.Where(p => p.Package.PackageName == queueItem.PackageName & p.Brand.BrandName == queueItem.BrandName & p.Factory == queueItem.Factory).Count() > 1)
                {
                    return Json("Ошибка! Очредей две!");
                }

                #endregion

                queueItem.Queue = _context.Queue.Where(p => p.Package.PackageName == queueItem.PackageName & p.Brand.BrandName == queueItem.BrandName & p.Factory == queueItem.Factory).FirstOrDefault();
                if (queueItem.Queue == null)
                {
                    return Json("ОЧЕРЕДИ УКАЗАННОЙ В ЗАЯВКЕ НЕТ!");
                }
                queueItem.Carriers = _context.Carriers.Where(p => p.OrganizationName == queueItem.CarrierName).FirstOrDefault();


                if (queueItem.Queue.Factory != queueItem.Factory)
                {
                    return Json("ОЧЕРЕДИ УКАЗАННОЙ В ЗАЯВКЕ НЕТ!");
                }


                var ff = _context.Queue.Where(q => q.Package.PackageName == queueItem.PackageName & q.Brand.BrandName == queueItem.BrandName);


                queueItem.QueueName = queueItem.Queue.QueueName;  // обработать исключение


                #region  Определение завода  (проверка на соответствие завода ) 

                if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) != "admin" & queueItem.FactoryName != UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity))
                {
                    return Json("В ЗАЯВКЕ УКАЗАН НЕ ЭТОТ ЗАВОД!");

                }

                #endregion

                #region   Порядок в очереди
                IQueryable<QueueItems> queueItems = _context.QueueItems;
                queueItems = queueItems.Where(p => p.Factory == queueItem.Factory);
                if (queueItem.Factory != null)
                {
                    queueItems = queueItems.Where(p => p.Factory == queueItem.Factory);
                }

                if (queueItem.Queue != null)
                {
                    queueItems = queueItems.Where(p => p.Queue == queueItem.Queue);
                }

                var queue = queueItems.ToList();
                queueItem.OderLine = queue.Count + 1;

                #endregion
                //   if (ModelState.IsValid)
                {
                    _context.Add(queueItem);
                    await _context.SaveChangesAsync();
                    await _signalHub.Clients.All.SendAsync("LoadQueueItems");
                }
                var ShippingAddress = _options.Value.ShippingAddress;
                var ShippingPHPSESSID= _options.Value.ShippingPHPSESSID;
                var client1c = new RestClient($"{ShippingAddress}={queueItem.Barecode}");
                client.Timeout = -1;
                var request1c = new RestRequest(Method.GET);
                request.AddHeader("Cookie", $"PHPSESSID={ShippingPHPSESSID}");
                IRestResponse response1c = client.Execute(request);

                return Json($"Вы зарегистрированны! На данный момент {queueItem.OderLine}-й");

            }
            //   return View(queueItem);
        }


        [HttpPost]                     // Передавать в метод параметры !!!
        //[ValidateAntiForgeryToken]  // HACK ?- вернуть полсле добавления jwt  токенов и тестов АПИ
        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
        public async Task<IActionResult> CreateWithoutBarecode([Bind("QueueItemsId,OderLine,CarNumber,Barecode,DriverName,Brand,Package,BrandName, QueueId, QueueName, Queue")][FromServices] IGetUserClaim UserClaimService, QueueItems queueItem)
        {


            foreach (var tempitem in _context.QueueItems)          // TODO
            {
                // hack ниже будет проверка на нахождение объекта в очереди. Она есть, переписать редирект на страницу с оповещением.
                /*  if (queueItem.Barecode == null || queueItem.Barecode.Length < 13 || queueItem.Barecode == tempitem.Barecode)  //  TODO условия валидации, хорошо бы переписать, пока так.
                  {
                      return RedirectToAction(nameof(Index));
                  }
                */
            }
            //queueItem.Factory = GetUserClaim("FactoryName");

            UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);


            // queueItem.Factory = _context.Factories.Where(p => p.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)).FirstOrDefault();
            // queueItem.FactoryName = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);

            queueItem.CarNumber = queueItem.CarNumber.ToUpper();

            queueItem.Queue = _context.Queue.Find(queueItem.QueueId);  // добавление ссылки на очередь в элемент очереди 
            queueItem.QueueName = _context.Queue.Find(queueItem.QueueId).QueueName;  //может вызывать исключение

            queueItem.Factory = queueItem.Queue.Factory;
            queueItem.FactoryName = queueItem.Queue.Factory.OrganizationName;

            queueItem.Barecode = null;

            queueItem.Carriers = _context.Carriers.Where(p => p.OrganizationName == "———").FirstOrDefault();


            var ff = queueItem.Queue;
            //   queueItem.QueueName = _context.Queue.Where(p => p.QueueId == queueItem.QueueId);
            // queueItem.Factory = _context.Factories.Find(GetUserClaim("FactoryName"));


            {  // определение номера элемента очереди
                IQueryable<QueueItems> queueItems = _context.QueueItems;
                queueItems = queueItems.Where(p => p.Factory == queueItem.Factory);

                if (queueItem.Factory.OrganizationName != null)
                {
                    queueItems = queueItems.Where(p => p.Factory.OrganizationName == queueItem.Factory.OrganizationName);
                }
                if (queueItem.Queue != null)
                {
                    queueItems = queueItems.Where(p => p.Queue == queueItem.Queue);
                }
                var queue = queueItems.ToList();
                queueItem.OderLine = queue.Count + 1;
            }

            // HACK ошибка валидации !!

            _context.Add(queueItem);
            await _context.SaveChangesAsync();
            await _signalHub.Clients.All.SendAsync("LoadQueueItems");

            return RedirectToAction("QueueItemsList", "QueueItems", $"?QueueName={queueItem.QueueName.ToString()}");  // Временно, будет модальное окно с подтверждалкой

        }

        // GET: QueueItems/Edit/5
        public async Task<IActionResult> Edit(Guid? QueueItemId)
        {


         

            if (QueueItemId == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems.FindAsync(QueueItemId);
            if (queueItem == null)
            {
                return NotFound();
            }
            return Json(queueItem);
        }

        public async Task<IActionResult> EditPartial(Guid? QueueItemId)
        {


            IEnumerable<Factory> factories = _context.Factories.ToList();                       // Только для админа
            ViewBag.Factory = new SelectList(factories, "OrganizationId", "OrganizationName");

            IEnumerable<Queue> queues = _context.Queue.ToList();
            ViewBag.Queues = new SelectList(queues, "QueueId", "QueueName");

            IEnumerable<Carrier> carriers = _context.Carriers.ToList();
            ViewBag.Carrier = new SelectList(carriers, "OrganizationId", "OrganizationName");

            if (QueueItemId == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems.FindAsync(QueueItemId);
            if (queueItem == null)
            {
                return NotFound();
            }
            return PartialView ("_EditPartial", queueItem);
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid QueueItemId, [Bind("QueueItemId,OderLine,CarNumber,Barecode,Factory,FactoryId,Brand,Queue,QueueId,DriverName,Carrier, CarrierId")][FromServices] IGetUserClaim UserClaimService, QueueItems queueItem)
        {

            var QueueItem_OLd = _context.QueueItems.AsNoTracking().Where(p => p.QueueItemId == QueueItemId).FirstOrDefault();


           
            if (QueueItemId != queueItem.QueueItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    #region  Изменение завода, переписать, т.к. вызовет полную перетасовку.
                    if (queueItem.FactoryId == Guid.Empty)
                    {
                        if (!(QueueItem_OLd.FactoryId==Guid.Empty))
                        {
                            queueItem.FactoryId = QueueItem_OLd.FactoryId;
                            queueItem.FactoryName = QueueItem_OLd.FactoryName;
                        }
                        else { 
                        ModelState.AddModelError("QueueItemId", "Не указан завод!");
                        return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        if (UserClaimService.GetUserRole((ClaimsIdentity)User.Identity) != "admin")
                        {
                            queueItem.Factory = _context.Factories.Where(p => p.OrganizationName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)).FirstOrDefault();
                            queueItem.FactoryName = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
                        }
                        else
                        {


                            queueItem.Factory = _context.Factories.Where(p => p.OrganizationId == queueItem.FactoryId).FirstOrDefault();




                        }
                    }
                    #endregion

                    if (queueItem.QueueId == Guid.Empty)
                    {
                        ModelState.AddModelError("QueueItemId", "Не указана очередь!");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        queueItem.Queue = _context.Queue.Find(queueItem.QueueId);
                        queueItem.QueueName = queueItem.Queue.QueueName;

                        var brands = _context.Queue.Include(u => u.Brand).ToList();  // HACK асинхронный метод может вызывать 
                        queueItem.BrandName = queueItem.Queue.Brand.BrandName;

                        var packages = _context.Queue.Include(u => u.Package).ToList();
                        queueItem.PackageName = queueItem.Queue.Package.PackageName;

                        var factory = _context.Queue.Include(u => u.Factory).ToList();
                        queueItem.Factory = _context.Factories.Find(queueItem.Queue.Factory.OrganizationId);
                        queueItem.FactoryName = queueItem.Factory.OrganizationName;
                    }


                    #region  установка перевозчика
                    if (queueItem.CarrierId == Guid.Empty)
                    {

                        if (!(QueueItem_OLd.CarrierId == Guid.Empty))
                        {
                            queueItem.CarrierId = QueueItem_OLd.CarrierId;
                            queueItem.CarrierName = QueueItem_OLd.CarrierName;
                        }
                        else
                        { 
                        ModelState.AddModelError("QueueItemId", "Не указан перевозчик!");
                        return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        queueItem.Carriers = _context.Carriers.Where(p => p.OrganizationId == queueItem.CarrierId).FirstOrDefault();
                        queueItem.CarrierName = _context.Carriers.Find(queueItem.CarrierId).OrganizationName;
                    }
                    #endregion
                    /*     #region  Проверка на изменение завода
                         if (QueueItem_OLd.FactoryId != queueItem.FactoryId)
                         {

                         }
                         #endregion

                         #region  Проверка на изменение перевозчика
                         if (QueueItem_OLd.CarrierId != queueItem.CarrierId)
                         {

                         }

                         #endregion*/

                    #region  Проверка на изменения очереди

                    if (QueueItem_OLd.QueueId != queueItem.QueueId)
                    {
                        var queueItems_old = _context.QueueItems.Where(p => p.QueueId == QueueItem_OLd.QueueId & p.OderLine > QueueItem_OLd.OderLine);
                        foreach (var queueItem_old in queueItems_old)
                        {
                            --queueItem_old.OderLine;
                        }
                        _context.QueueItems.Remove(queueItem);
                        _context.QueueItems.UpdateRange(queueItems_old);

                        if (queueItem.OderLine > _context.QueueItems.Where(p => p.QueueId == queueItem.QueueId).Count())  // проверка на превышение желаемого номера в оч. над кол-ыом элементов в очереди
                        {
                            queueItem.OderLine = 1+_context.QueueItems.Where(p => p.QueueId == queueItem.QueueId).Count();
                        }

                        var queueItems_new = _context.QueueItems.Where(p => p.QueueId == queueItem.QueueId & p.OderLine >= queueItem.OderLine);
                        foreach (var queueItem_new in queueItems_new)
                        {
                            ++queueItem_new.OderLine;
                        }

                    }

                    #endregion
                   // await _signalHub.Clients.All.SendAsync("LoadQueueItems");

                    #region Проверка на изменение позиции в очереди и перестроение очереди, если позиия была изменена, а очередь осталась преждней
                    else
                    {

                        if (QueueItem_OLd.OderLine != queueItem.OderLine)
                        {
                            if (queueItem.OderLine > _context.QueueItems.Where(p => p.QueueId == queueItem.QueueId).Count())  // проверка на превышение желаемого номера в оч. над кол-ыом элементов в очереди
                            {
                                queueItem.OderLine = +_context.QueueItems.Where(p => p.QueueId == queueItem.QueueId).Count();
                            }

                            if (queueItem.OderLine < QueueItem_OLd.OderLine)
                            {
                                var queueItems_old = _context.QueueItems.Where(p => p.QueueId == queueItem.QueueId & p.OderLine >= queueItem.OderLine & p.OderLine < QueueItem_OLd.OderLine & p.QueueItemId != queueItem.QueueItemId);
                                foreach (var queueItem_old in queueItems_old)
                                {
                                    ++queueItem_old.OderLine;

                                }
                                _context.QueueItems.UpdateRange(queueItems_old);
                            }
                            else
                            {
                                var queueItems_old = _context.QueueItems.Where(p => p.QueueId == queueItem.QueueId & p.OderLine > QueueItem_OLd.OderLine & p.OderLine <= queueItem.OderLine & p.QueueItemId != queueItem.QueueItemId);

                                foreach (var queueItem_old in queueItems_old)
                                {
                                    --queueItem_old.OderLine;

                                }
                                _context.QueueItems.UpdateRange(queueItems_old);
                            }

                        }
                    }
                    #endregion

                    _context.Update(queueItem);
                    await _context.SaveChangesAsync();
                    await _signalHub.Clients.All.SendAsync("LoadQueueItems");

                }



                catch (DbUpdateConcurrencyException)
                {
                    if (!QueueItemExists(queueItem.QueueItemId))
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
            return View(queueItem);

        }

        // GET: QueueItems/Delete/5
        public async Task<IActionResult> Delete(Guid? QueueItemId)
        {
            if (QueueItemId == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems
                .FirstOrDefaultAsync(m => m.QueueItemId == QueueItemId);
            if (queueItem == null)
            {
                return NotFound();
            }

            return View(queueItem);
        }
        [ActionName("DeleteConfirmed")]
        public async Task<IActionResult> Delete(string Barecode)  //UNDONE перегрузка запрос на удаление элемента по штрихкоду
        {
            if (Barecode == null)
            {
                return NotFound();
            }
            var queueItem = await _context.QueueItems
                .FirstOrDefaultAsync(m => m.Barecode == Barecode);
            if (queueItem == null)
            {
                return NotFound();
            }
            return View(queueItem);
        }

        public IActionResult DeleteRequest()
        {
            return View();
        }

        // POST: QueueItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,factory,Terminal,Supervisor")]
        //  [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("QueueItemsId,OderLine,CarNumber,Barecode,Factory,Brand")][FromServices] IGetUserClaim UserClaimService, QueueItems queueItem)
        {

            var temprole = UserClaimService.GetUserRole((ClaimsIdentity)User.Identity);

            IEnumerable<QueueItems> tempqueueItems;

            if (!(temprole == "admin"))
            {
                var tempClaimFactory = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
                var tempClaimCarrier = UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity);
                 tempqueueItems = _context.QueueItems.Where(p => p.FactoryName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity))
           .Union(_context.QueueItems.Where(p => p.FactoryName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)));
                
            }
            else
            {
                 tempqueueItems = _context.QueueItems;
            }

            // var tempClaimFactory = UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity);
            //  var tempClaimCarrier = UserClaimService.GetUserClaim("CarrierName", (ClaimsIdentity)User.Identity);


           // var tempqueueItems = _context.QueueItems.Where(p => p.FactoryName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity))
         //   .Union(_context.QueueItems.Where(p => p.FactoryName == UserClaimService.GetUserClaim("FactoryName", (ClaimsIdentity)User.Identity)));

      



            //  var tt = tempqueueItems.Union(tempqueueItems) ;


            var tempqueueItem = tempqueueItems.Where(p => p.QueueItemId == queueItem.QueueItemId).FirstOrDefault();

            _context.QueueItems.Remove(tempqueueItem);

            IQueryable<QueueItems> queueItems = _context.QueueItems.Where(p => (p.OderLine > tempqueueItem.OderLine)
             & (p.FactoryId == tempqueueItem.FactoryId) & (p.QueueId == tempqueueItem.QueueId));

            foreach (var item in queueItems)
            {
                item.OderLine--;
            }

            _context.QueueItems.UpdateRange(queueItems);

            _context.SaveChangesAsync();
            _signalHub.Clients.All.SendAsync("LoadQueueItems");
            return Json($"{tempqueueItem.CarNumber}, удалён из очереди!");
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Barecode)
        {

            var queueItem = _context.QueueItems.FirstOrDefault(m => m.Barecode == Barecode);

            if (queueItem == null || queueItem.Barecode == null)
            {
                return Json("Вы не зарегистрированны в очереди! ");
            }
            //queueItem = await _context.QueueItems.FindAsync(Barecode);
            _context.QueueItems.Remove(queueItem);  // UNDONE Вызывает исключение при неверном штрихкоде, - обработать 

            IQueryable<QueueItems> queueItems = _context.QueueItems.Where(p => (p.OderLine > queueItem.OderLine)
            & (p.FactoryId == queueItem.FactoryId) & (p.QueueId == queueItem.QueueId));

            foreach (var item in queueItems)
            {
                item.OderLine--;
            }

            _context.QueueItems.UpdateRange(queueItems);

            await _context.SaveChangesAsync();
            await _signalHub.Clients.All.SendAsync("LoadQueueItems");

            //  return RedirectToAction(nameof(Index));
            return Json($"{queueItem.CarNumber}, удалён из очереди!");
        }

        private bool QueueItemExists(Guid id)
        {
            return _context.QueueItems.Any(e => e.QueueItemId == id);
        }



        [AcceptVerbs("Get", "Post")]   //UNDONE Это пока не работает!!
        public IActionResult CheckBarecode(string Barecode)
        {
            QueueItems queueItem = new QueueItems();
            queueItem.Barecode = Barecode;
            if ((_context.QueueItems.Where(p => p.Barecode == Barecode)) == null)
                return Json(true);
            return Json(false);

        }




        [HttpGet]
        public IActionResult _CreateResultMessegePartial()
        {

            var model = new QueueItems();

            return PartialView("_CreateResultMessegePartial", model);
        }

        [HttpGet]
        public IActionResult _DeleteResultMessegePartial()
        {

            var model = new QueueItems();

            return PartialView("_DeleteResultMessegePartial", model);
        }


        /*  public class DynamicJsonDeserializer:IDeserializer
          {
              public string RootElement { get; set; }
              public string Namespace { get; set; }
              public string DateFormat { get; set; }

              public T Deserialize<T>(RestResponse response) where T : new()
              {
                  return JsonConvert.DeserializeObject<dynamic>(response.Content);
              }

          }  */


    }
}

