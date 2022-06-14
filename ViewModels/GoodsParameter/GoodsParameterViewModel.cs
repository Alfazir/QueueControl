using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QueueControlServer.Models;

namespace QueueControlServer.ViewModels.GoodsParameter
{
    public class GoodsParameterViewModel
    {
       




     
        public List<Brand> Brands { get; set; }
        public List<Package> Packages { get; set; }
  //      public SelectList Organizations { get; set; }
        public string OrganizationsName { get; set; }
    //    public SelectList Factories { get; internal set; }
     //   public SelectList Carriers { get; internal set; }
        public string Name { get; set; }
       
    }
}

