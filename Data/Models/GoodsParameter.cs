using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QueueControlServer.Models
{
    

    [Table("GoodsParameter")]
    public class GoodsParameter       
    {
        public int GoodsParameterId { get; set; }

    }

    

    [Table("Brands")]
    public class Brand: GoodsParameter
    {
        public string BrandName { get; set; }
    }
    [Table("Packages")]
    public class Package: GoodsParameter
    {
        public string PackageName { get; set; }
    }
}
