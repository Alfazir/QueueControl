using QueueControlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.ViewModels.Queue
{
    public class CreateQueueViewModel
    {
        public string QueueName { get; set; }

        public Guid OrganizationId { get; set; }
        public Factory Factory { get; set; }
        

        public int GoodsParameterId { get; set; }  // Hack айдишник брэнда переписать на более понятные ссылки
        public int PackageParameterId { get; set; }  // айдишник упаковки (вынес отдельно, фактически сюда сохраняеться GoodsParameterId упаковки из базы )
        public Brand Brand { get; set; }
        public Package Package { get; set; }
    }
}
