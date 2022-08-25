namespace TrackingAPI.Models
{
    public class CityMaster
    {
        public int nCityId { get; set; }
        public string vCityName { get; set; }
        public int nStateId { get; set; }
        public bool btActive { get; set; }
        public bool btMainInterRelatedCity { get; set; }
        public int? nInterRelatedCityId { get; set; }
    }
}
