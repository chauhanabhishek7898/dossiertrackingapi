namespace TrackingAPI.Models
{
    public class VehicleRateMaster
    {
        public int nVRId { get; set; }
        public int nVId { get; set; }
        public int nCityId { get; set; }
        public double nRatePerKM { get; set; }
        public bool btActive { get; set; }
    }
}
