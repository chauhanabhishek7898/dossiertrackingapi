namespace TrackingAPI.Models
{
    public class ServiceSubTypeMaster
    {
        public int nSSTId { get; set; }
        public string vServiceSubType { get; set; }
        public int nSTId { get; set; }
        public int nVId { get; set; }
        public int nCityId { get; set; }
        public int nFromKM { get; set; }
        public int nToKM { get; set; }
        public double nRate { get; set; }
        public bool btActive { get; set; }
    }
}
