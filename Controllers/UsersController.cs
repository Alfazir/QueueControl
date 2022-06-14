using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QueueControlServer.Models;
using QueueControlServer.ViewModels.Users;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using QueueControlServer.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CustomIdentityApp.Controllers
{


    [Authorize(AuthenticationSchemes = MVCJwtTokens.AuthSchemes, Roles = "admin,Terminal,Supervisor,LocalAdmin")]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;     
        private readonly ApplicationDBContext _context;  // контекст БД

        public UsersController(UserManager<User> userManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create()
        {
            IEnumerable<Factory> factories = _context.Factories.ToList();

            ViewBag.Factory = new SelectList(factories, "OrganizationId", "OrganizationName"); // список всех заводов
            return View();
        }

        public IActionResult CreateLocalAdmin()  // HACK убрать как будут view component Пока так.

        {
            IEnumerable<Organization> organizations = _context.Organizations.ToList();

            foreach (Organization organization in organizations)
            {
                if (_context.Factories.Where(p => p.OrganizationId == organization.OrganizationId).Count() > 0)

                {
                    organization.OrganizationName = "(Завод) " + organization.OrganizationName;
                }
                else
                {
                    organization.OrganizationName = "(Перевозчик) " + organization.OrganizationName;
                }
            }
            ViewBag.Organization = new SelectList(organizations, "OrganizationId", "OrganizationName");
            return View();
        }




        public IActionResult CreateTerminal()  // HACK убрать как будут view component Пока так.

        {
            IEnumerable<Factory> factories = _context.Factories.ToList();

            IEnumerable<Queue> queues=_context.Queue.ToList();

            ViewBag.Factory = new SelectList(factories, "OrganizationId", "OrganizationName"); // список всех заводов

            var item = _context.Queue.ToList();
            CreateTerminalViewModel m1 = new CreateTerminalViewModel();
            m1.Queues = item.Select(vm=> new CheckBoxItem()
                { 
                  Id= vm.QueueId,
                  Title= vm.QueueName+" : "+vm.Factory.OrganizationName,
                  IsChecked = false

                }).ToList(); 
            

            return View(m1);
        }

        public IActionResult CreateSupervisor()  // HACK убрать как будут view component Пока так.

        {
            IEnumerable<Organization>organizations = _context.Organizations.ToList();

            foreach (Organization organization in organizations)
            {
                if (_context.Factories.Where(p=>p.OrganizationId==organization.OrganizationId).Count()>0 )

                {
                    organization.OrganizationName = "(Завод) " + organization.OrganizationName;
                }
                else
                {
                    organization.OrganizationName= "(Перевозчик) " +organization.OrganizationName;
                }
             }
            ViewBag.Organization = new SelectList(organizations, "OrganizationId", "OrganizationName");
            return View();
        }

        public IActionResult CreateDriver()  // HACK убрать как будут view component Пока так.

        {
            IEnumerable<Carrier> carriers = _context.Carriers.ToList();
            ViewBag.Carrier = new SelectList(carriers, "OrganizationId", "OrganizationName"); // список всех перевозчиков
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateUser")]
        public async Task<IActionResult> Create(CreateUserViewModel model)  // создание "базового пользователя"
        {
            if (ModelState.IsValid)
            {
               // var fact = _context.Factories.ToList(); // HACH Здесь будет добавление завода
              //  var tempFact =fact.FirstOrDefault(fact => fact.Name==model.FactoryName);
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year, FactoryName = model.FactoryName};
                var result = await _userManager.CreateAsync(user, model.Password);


                _ = await _userManager.AddClaimAsync(user, new Claim("FactoryName", model.FactoryName));


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
            return View(model);
        }

        // UNDONE ЗДесь будет перегрузка метода Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateLocalAdmin")]
        public async Task<IActionResult> CreateLocalAdmin(CreateLocalAdminViewModel model)  // создание терминала 
        {
            if (ModelState.IsValid)
            {

                IdentityResult result;

                model.Carrier = _context.Carriers.Find(model.OrganizationId);

                if (model.Carrier == null)
                {
                    model.Factory = _context.Factories.Find(model.OrganizationId);
                    LocalAdmin supervisor = new LocalAdmin { Email = model.Email, UserName = model.Email, Organization = model.Factory };
                    result = await _userManager.CreateAsync(supervisor, model.Password);
                    _ = await _userManager.AddClaimAsync(supervisor, new Claim("FactoryName", model.Factory.OrganizationName));
                    _ =  _userManager.AddToRoleAsync(supervisor, "LocalAdmin");
                }

                else
                {
                    model.Carrier = _context.Carriers.Find(model.OrganizationId);
                    LocalAdmin supervisor = new LocalAdmin { Email = model.Email, UserName = model.Email, Organization = model.Carrier };
                    result = await _userManager.CreateAsync(supervisor, model.Password);
                    _ = await  _userManager.AddClaimAsync(supervisor, new Claim("CarrierName", model.Carrier.OrganizationName));
                    _ = await _userManager.AddToRoleAsync(supervisor, "LocalAdmin");
                }




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
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateTerminal")]
        public async Task<IActionResult> CreateTerminal(CreateTerminalViewModel model)  // создание терминала 
        {
            if (ModelState.IsValid)
            {
                model.Factory = _context.Factories.Find(model.OrganizationId);
                Terminal terminal = new Terminal { Email = model.Email, UserName = model.Email, Factory = model.Factory };
                var result = await _userManager.CreateAsync(terminal, model.Password);


                _ = await _userManager.AddClaimAsync(terminal, new Claim("FactoryName", model.Factory.OrganizationName));
                _=  await _userManager.AddToRoleAsync(terminal, "Terminal");

                 List<string> TerminalQueues = new List<string>();
                //model.Queues.Where(p=>p.IsChecked==true)
                foreach ( var item in model.Queues)
                {
                    if (item.IsChecked== true)
                    {
                        TerminalQueues.Add(item.Id.ToString());
                    }
                }
                
                _ = await _userManager.AddClaimAsync(terminal, new Claim("TerminalQueues", string.Join(",",TerminalQueues)));

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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateSupervisor")]
        public async Task<IActionResult> CreateSupervisor(CreateSupervisorViewModel model)  // создание терминала 
        {
            if (ModelState.IsValid)
            {

                IdentityResult result;

                model.Carrier = _context.Carriers.Find(model.OrganizationId);

                if (model.Carrier == null)
                {
                    model.Factory = _context.Factories.Find(model.OrganizationId);
                    Supervisor supervisor = new Supervisor { Email = model.Email, UserName = model.Email, Organization = model.Factory };
                    result = await _userManager.CreateAsync(supervisor, model.Password);
                    _ = await _userManager.AddClaimAsync(supervisor, new Claim("FactoryName", model.Factory.OrganizationName));
                    _ =await _userManager.AddToRoleAsync(supervisor, "Supervisor");
                }

                else
                {
                    model.Carrier = _context.Carriers.Find(model.OrganizationId);
                    Supervisor supervisor = new Supervisor { Email = model.Email, UserName = model.Email, Organization = model.Carrier };
                    result = await _userManager.CreateAsync(supervisor, model.Password);
                    _ =await _userManager.AddClaimAsync(supervisor, new Claim("CarrierName", model.Carrier.OrganizationName));
                    _ =await _userManager.AddToRoleAsync(supervisor, "Supervisor");
                }
             

            

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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateDriver")]
        public async Task<IActionResult> Create(CreateDriverViewModel model)  // создание "базового пользователя"  
        {
            if (ModelState.IsValid)
            {
                model.Carrier = _context.Carriers.Find(model.OrganizationId);
                Driver driver = new Driver { Email = model.Email, UserName = model.Email, FactoryName = model.FactoryName };
                var result = await _userManager.CreateAsync(driver, model.Password);


                _= await _userManager.AddClaimAsync(driver, new Claim("CarrierName", model.Carrier.OrganizationName));
                _ =await  _userManager.AddToRoleAsync(driver, "Driver");

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
            return View(model);
        }








        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Year = user.Year, FactoryName = user.FactoryName };








            return View(model);
        }

        public async Task<IActionResult> EditTerminal(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            IEnumerable<Factory> factories = _context.Factories.ToList();
            ViewBag.Factory = new SelectList(factories, "OrganizationId", "OrganizationName");

            var item = _context.Queue.ToList();


            EditTerminalViewModel model = new EditTerminalViewModel { Id = user.Id, Email = user.Email, Year = user.Year, FactoryName = user.FactoryName };

            model.Queues = item.Select(vm => new CheckBoxItem()
            {
                Id = vm.QueueId,
                Title = vm.QueueName,
                IsChecked = false

            }).ToList();
            return View (model);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Year = model.Year;
                    user.FactoryName = model.FactoryName;

                    var TempUserClaim = await _userManager.GetClaimsAsync(user);
                    var hh = TempUserClaim.FirstOrDefault(TempClaim => TempClaim.Type == "FactoryName");
                    if (hh != null)
                    {
                        await _userManager.RemoveClaimAsync(user, hh);
                    }
                    
                    await _userManager.AddClaimAsync(user, new Claim("FactoryName", model.FactoryName));

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // User gg = await _userManager.FindByIdAsync(user.Id);
                        //  var Identity = new ClaimsIdentity(User.Identity);
                        //  await _userManager.RemoveClaimAsync(gg, Identity.FindFirst("FactoryName"));
                        //  await _userManager.AddClaimAsync(user, new Claim("FactoryName", model.FactoryName));


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
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                _ = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                    await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
