namespace TrackingAPI.Models
{
    public class OrderRouteDetails
    {
        public int nTrackId { get; set; }
        public int nRouteId { get; set; }
        public string vSource { get; set; }
        public string vSourceLat { get; set; }
        public string vSourceLong { get; set; }
        public string vSourceAddress { get; set; }
        public string vDestination { get; set; }
        public string vDestinationLat { get; set; }
        public string vDestinationLong { get; set; }
        public string vDestinationAddress { get; set; }
        public int nKMs { get; set; }
        public int nRate { get; set; }
        public int nWaitingTimeInMinutes { get; set; }
        public int nWaitingPrice { get; set; }
        public int nTotalRate { get; set; }
        public string vPickUpPointReachedTime { get; set; }
        public string vPickUpPointStartTime { get; set; }
        public string vDestinationReachedEndTime { get; set; }
        public bool btClosed { get; set; }
    }
}
