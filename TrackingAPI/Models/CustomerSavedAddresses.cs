namespace TrackingAPI.Models
{
    public class CustomerSavedAddresses
    {
        public int nOtherAdId { get; set; }
        public int nUserId { get; set; }
        public int nCityId { get; set; }
        public string vAddress { get; set; }
        public string vLat { get; set; }
        public string vLong { get; set; }
        public bool btActive { get; set; }

    }
}
