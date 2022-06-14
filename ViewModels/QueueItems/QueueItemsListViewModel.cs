using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QueueControlServer.Models;

namespace QueueControlServer.ViewModels.QueueItems
{
    public class QueueItemsListViewModel
    {
        public IEnumerable<QueueControlServer.Models.QueueItems> QueueItems { get; set; }
        public SelectList Organizations { get; set; }
        public string OrganizationsName { get; set; }
        public SelectList Factories { get; internal set; }
        public SelectList Carriers { get; internal set; }
        public string Name { get; set; }
        public SelectList QueueNames { get; set; }
    }
}

