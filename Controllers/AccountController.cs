using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QueueControlServer.ViewModels;
using QueueControlServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QueueControlServer.ViewModels.API;
using System.Linq;


//using javax.crypto;

namespace QueueControlServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        public AccountController(ApplicationDBContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Driver user = new Driver { Email = model.Email, UserName = model.Email, Year = model.Year };

                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);

                // устанавливаем клайм




                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        //[AllowAnonymous]
        //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                #region

               /* var user = await _userManager.FindByNameAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                    };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = AuthOptions.GetSymmetricSecurityKey();

                    var token = new JwtSecurityToken(
                     issuer: AuthOptions.ISSUER,
                     audience: AuthOptions.AUDIENCE,
                     expires: DateTime.Now.AddHours(3),
                     claims: authClaims,
                     signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                       );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                    {
                        return  Unauthorized();

                    }
               }*/
                    #endregion

                

                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task <IActionResult> CreateToken ([FromBody] JwtTokenViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.UserName);
                var signInResult= await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (signInResult.Succeeded)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MVCJwtTokens.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



                    // добавление в масив 
                  var userIdentity = (ClaimsIdentity)User.Identity;
                    // var ClaimName =_context.Users.



                    // Определим клеймы пользователя
                    // Роли
                    List<string> ListRoles = new List<string>();

                    var UserRoleId=  _context.UserRoles.Where(p => p.UserId == user.Id).ToList();

                    foreach (var RoleId in  UserRoleId)
                    {
                        ListRoles.Add(_context.Roles.Find(RoleId.RoleId).Name);
                         
                    }


                    /*    var claims = new  []
                        {
                            new Claim (JwtRegisteredClaimNames.Sub, model.UserName),
                            new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim (JwtRegisteredClaimNames.UniqueName, model.UserName),
                        //    new Claim ("role", "admin")




                        //  new Claim (JwtRegisteredClaimNames.GivenName, model.UserName),
                        //  new Claim("FactoryName", Convert.ToString(userIdentity.FindFirst($"{ClaimName}").Value))  //допил

                        // Добаим клеймы в токен.


                    };*/

                    #region  Добавление ролей в токен


                    var claims = new List<Claim>();

                    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, model.UserName));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName));


                  

                    foreach (var Role in ListRoles) //
                    {
                         claims.Add(new Claim("role-abc", $"{Role}"));
                    }

                    #endregion






                    var token = new JwtSecurityToken(
                        MVCJwtTokens.Issuer,
                        MVCJwtTokens.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: creds 
                        );

                    var results = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo

                    };
                    return Created("", results);
                }
                else
                {
                    return BadRequest();
                }

            }
            return BadRequest();
        }

    }
}
