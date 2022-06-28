namespace TrackingAPI.Models
{
    public class TrackingDetails
    {
        public int nTrackId { get; set; }
        public string vTrackId { get; set; }
        public string dtDated { get; set; }
        public string vTime { get; set; }
        public int nCustomerUserId { get; set; }
        public int nCityId { get; set; }
        public string vFromLocation { get; set; }
        public string vFromLocationLat { get; set; }
        public string vFromLocationLong { get; set; }
        public string vFromFlatNoPlotNoLaneBuilding { get; set; }
        public string vToLocation { get; set; }
        public string vToLocationLat { get; set; }
        public string vToLocationLong { get; set; }
        public string vToFlatNoPlotNoLaneBuilding { get; set; }
        public bool btActive { get; set; }
        public bool btPaid { get; set; }
        public double nKMs { get; set; }
        public double nRate { get; set; }
        public int nWaitingTimeInMinutes { get; set; }
        public double nWaitingPrice { get; set; }
        public double nTotalRate { get; set; }
        public string vRemarks { get; set; }
        public int nDriverUserId { get; set; }
        public string vDiriverCurrentLat { get; set; }
        public string vDiriverCurrentLong { get; set; }
        public bool btClosed { get; set; }
        public string dtCreatedDate { get; set; }
        public string dtModifiedDate { get; set; }
        public int nCreatedByUserId { get; set; }
        public int nModifiedByUserId { get; set; }
        public int nLoggedInUserId { get; set; }
    }
}
