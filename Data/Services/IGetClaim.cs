using QueueControlServer.Models;
using QueueControlServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;



namespace QueueControlServer.Services
{
    public class UserClaimService : IGetUserClaim
    {

        private readonly ApplicationDBContext _context;


        private readonly IHubContext<SignalServer> _signalHub;
        public UserClaimService(ApplicationDBContext context )//, IHubContext<SignalServer> signalHub)
        {
            _context = context;
          //  _signalHub = signalHub;
        }
     
        public string GetUserClaim(string ClaimName, ClaimsIdentity userIdentity )
        {
            string ClaimVal;
            try
            {
                if (userIdentity.FindFirst($"{ClaimName}") == null)
                {

                    ClaimVal = null;
                    var ClaimType = userIdentity.NameClaimType;
                    var rolesName = (userIdentity.Claims.Where(c => c.Type == ClaimType).ToList()).First();
                    var User = _context.Users.Where(p => p.UserName == Convert.ToString(rolesName.Value)).FirstOrDefault();

                    if (_context.UserClaims
                            .Where(p => p.UserId == User.Id & p.ClaimType == ClaimName)
                            .FirstOrDefault() == null)
                    {


                    }
                    else
                    {
                        ClaimVal = _context.UserClaims
                            .Where(p => p.UserId == User.Id & p.ClaimType == ClaimName)
                            .FirstOrDefault()
                            .ClaimValue.ToString();

                    }



                }
                else
                {
                    ClaimVal = Convert.ToString(userIdentity.FindFirst($"{ClaimName}").Value);
                }
                return ClaimVal;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public string GetUserRole (ClaimsIdentity userIdentity)
        {
            
            if (userIdentity.Claims.Count()==0)
            {
                return "unauthorized";
            }

            var ClaimType = userIdentity.NameClaimType;
            var rolesName = (userIdentity.Claims.Where(c => c.Type == ClaimType).ToList()).First();
            var User = _context.Users.Where(p => p.UserName == Convert.ToString(rolesName.Value)).FirstOrDefault();
            var RoleId = _context.UserRoles.Where(p => p.UserId == User.Id).FirstOrDefault();
            var Role = _context.Roles.Find(RoleId.RoleId).Name;   // HACK вызывает исключениее при отсутствии роли. Обработать"!

            //var rolesName = (userIdentity.Claims.Where(c => c.Type == userIdentity.RoleClaimType).ToList()).First();
            return (Convert.ToString(Role));

        }
    }
}

