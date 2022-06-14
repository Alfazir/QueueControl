using QueueControlServer.Models;

namespace QueueControlServer.ViewModels.QueueItems
{
    public class CreateWithoutBarecodeViewModel
    {
        public int QueueItemId { get; set; }               // ид элемента
        public int OderLine { get; set; }              // место в очереди (поля имеют нулевые значение в базе, но заполняються в мщмент вызова метода GetQueueItems в зависимости от входных данных метода )
        public string CarNumber { get; set; }          // гос номер ТС
        public string Barecode { get; set; }           // Штрихкод карты, если карта не использовалась, будет нулевым
        public string DriverName { get; set; }         // ФИО Водителя


        public string GoodsParameterId { get; set; }
        public string BrandName { get; set; }              // марка цемента (значение запрашиваеться из стороннего сервиса или вводиться пользователем )
        public Brand Brand { get; set; }                 // марка цемента (значение из параметров целевого завода)


        public string PackageName { get; set; }            // Вид упаковки  (значение запрашиваеться из стороннего сервиса или вводиться пользователем ) 
        public Package Pacakege { get; set; }             // вид упаковки из целевого завода


        public string QueueName { get; set; }   // имя очереди

       
    }
}
