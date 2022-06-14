using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace QueueControlServer.Models
{
    public class QueueItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QueueItemId { get; set; }            

        public QueueItems()
        {
            CreatedAt= DateTime.Now;
        }
        public DateTime CreatedAt { get; private set; }
        public int OderLine { get; set; }              

        [Required(ErrorMessage = "Не указан гос. номер")]
        [RegularExpression(@"[АВЕКМНОРСТУХавекмнорстух]\d{3}(?<!000)[АВЕКМНОРСТУХавекмнорстух]{2}\d{2,3}", ErrorMessage = "Некорректный гос. номер")]
        public string CarNumber { get; set; }          // гос номер ТС

        [StringLength(13, MinimumLength = 13, ErrorMessage = "Некорректный штрихкод !")]
        [Remote(action: "CheckBarecode", controller: "QueueItemsController"
         , ErrorMessage ="Вы уже зарегистрированны в очереди!!! Отмените предыдущую регистрация прежде чем повторить попытку!") ]
        public string Barecode { get; set; }           // Штрихкод карты, если карта не использовалась, будет нулевым

        [Required(ErrorMessage = "Не указаны Ф.И.О")]
        public string DriverName { get; set; }         // ФИО Водителя

        public string BrandName { get; set; }              // марка цемента (значение запрашиваеться из стороннего сервиса или вводиться пользователем )
        public Brand Brand { get; set; }                 // марка цемента (значение из параметров целевого завода) HACK возможно стоит убрать, т.к. есть в Queue

        public string PackageName { get; set; }            // Вид упаковки  (значение запрашиваеться из стороннего сервиса или вводиться пользователем ) 
        public Package Pacakege { get; set; }             // вид упаковки из целевого завода  HACK возможно стоит убрать, т.к. есть в Queue  Потом обновить методы!


        public string QueueName { get; set; }   // имя очереди
        public Guid QueueId { get; set; } = Guid.Empty;     // ид очереди
        //[JsonIgnore]
        public Queue Queue { get; set; }      // ссылочный тип на очередь, которой пренадлежит элемент.


        public string FactoryName { get; set; }
        public Guid FactoryId { get; set; }             // ID завода
        [JsonIgnore]
        public Factory Factory { get; set; }            // Завод,  значение берёться из UserClaim FactoryName


        public string CarrierName { get; set; }
        public Guid CarrierId { get; set; }             // ID перевозчика
        [JsonIgnore]
        public Carrier Carriers { get; set; }          //TODO поправить при следующей миграции       Организация перевозчик

    }
}
