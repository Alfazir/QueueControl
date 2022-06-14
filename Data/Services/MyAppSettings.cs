namespace QueueControlServer.Data.Services
{
    public class MyAppSettings
    {
        public const string SectionName = "MySettings";

        public string IntegrationAddress { get; set; }
        public string IntegrationToken { get; set; }
        public string PHPSESSID { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPHPSESSID { get; set; }

    }
}
