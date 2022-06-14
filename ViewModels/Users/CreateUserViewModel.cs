using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using QueueControlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.ViewModels.Users
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Year { get; set; }
        public string FactoryName { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
   
        public SelectList Organizations { get; set; }

    }
    public class CreateDriverViewModel : CreateUserViewModel
    {
        public Carrier Carrier { get; set; }
    }
    public class CreateTerminalViewModel:CreateUserViewModel
    {
        public Factory Factory { get; set; }
       // public List <string> Queues { get; set; }
        public List <CheckBoxItem> Queues { get; set; }
    }

    public class CreateSupervisorViewModel : CreateUserViewModel
    {
        public Factory Factory { get; set; }
        public Carrier Carrier { get; set; }
    }

    public class CreateLocalAdminViewModel : CreateUserViewModel
    {
        public Factory Factory { get; set; }
        public Carrier Carrier { get; set; }
    }



}
