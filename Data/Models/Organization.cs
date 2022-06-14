using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QueueControlServer.Models
{
    [Table("Organizations")]
    public class Organization
    {
        
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

        [JsonIgnore]
        public List<QueueItems> QueueItems { get; set; }
        public Organization()
        {
            QueueItems = new List<QueueItems>();
        }
    }

    [Table("Factories")]
    public class Factory : Organization
    {
        public  ICollection<Terminal> Terminals { get; set; }
    }


    [Table("Carriers")]
    public class Carrier : Organization
    {
        public  ICollection<Driver> Drivers { get; set; }
    }

}
