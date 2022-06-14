using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace QueueControlServer.Models
{
    /*  public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
      {
          public AppClaimsPrincipalFactory(
          UserManager<User> userManager,
          RoleManager<Role> roleManager,
          IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
          {
          }

          public async override Task<ClaimsPrincipal> CreateAsync(User user)   // добавление клаймов при создании пользователя
          {
              var principal = await base.CreateAsync(user);

              ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
              new Claim(ClaimTypes.GivenName, user.FactoryName),
              new Claim(ClaimTypes.Surname, user.FactoryName),
          });

              return principal;
          }

      }
      public static class IdentityExtensions
      {
          public static string FirstName(this IIdentity identity)
          {
              var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GivenName);
              // Test for null to avoid issues during local testing
              return (claim != null) ? claim.Value : string.Empty;
          }


          public static string LastName(this IIdentity identity)
          {
              var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname);
              // Test for null to avoid issues during local testing
              return (claim != null) ? claim.Value : string.Empty;
          }
      }  */
}
