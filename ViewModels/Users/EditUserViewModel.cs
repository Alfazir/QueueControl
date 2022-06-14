using Microsoft.AspNetCore.Identity;
using QueueControlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.ViewModels.Users
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public string FactoryName { get; set; }
        
    }

    public class EditTerminalViewModel:EditUserViewModel
    {
        public Factory Factory { get; set; }
        public List<CheckBoxItem> Queues { get; set; }
    }

}
