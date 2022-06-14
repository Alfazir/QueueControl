using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueControlServer.Models
{
    [Table("Queues")]
    public class Queue
    {

        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QueueId { get; set; }

        public string QueueName { get; set; } = string.Empty;  // имя будем состовлять из параметров очереди 

   
        public Brand Brand { get; set; }
        public Package Package { get; set; }  
        public Factory Factory { get; set; } 
    }
}
