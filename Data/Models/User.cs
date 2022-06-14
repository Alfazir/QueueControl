using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QueueControlServer.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string FactoryName { get; set; }
        public string UserType { get; set; }         
       
        // имя завода


        // public virtual ICollection<IdentityUserClaim <string>> FactoryName { get; set; }

        /*public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {

            var authenticationType = "FactoryName";
            var userIdentity = new ClaimsIdentity(await manager.GetClaimsAsync(this), authenticationType);
            await manager.AddClaimAsync(this, new Claim("FactoryName", model.FactoryName));
            return userIdentity;
        }
        */
    }

    [Table("Drivers")]
    public class Driver : User
    {
        public string DriverName { get; set; }
        public Carrier Carrier { get; set; }
    }

    [Table("Terminals")]
    public class Terminal : User
    {
        public string TerminalName { get; set; }
        public Factory Factory { get; set; }
    }

    [Table("Supervisors")]
    public class Supervisor : User
    {
        public string SupervisorName { get; set; }
        public Organization Organization { get; set; }
    }

  [Table("LocalAdmin")]
    public class LocalAdmin : User
    {
        public string LocalAdminName { get; set; }
        public Organization Organization { get; set; }
    }

}
