namespace WpfIoTSimulatorApp.Models
{
    // Json용 클래스. 다른 곳 안씀.
    public class PrcMsg
    {
        public string ClientId { get; set; }

        public string PlantCode { get; set; }

        public string FacilityId { get; set; }

        public string Timestamp { get; set; }

        public string Flag { get; set; }
    }
}