namespace TrackingAPI.Models
{
    public class OrderDetails
    {
        public int nTrackId { get; set; }
        public string vTrackId { get; set; }
        public string dtDated { get; set; }
        public string vTime { get; set; }
        public int nCustomerUserId { get; set; }
        public int nCityId { get; set; }
        public double nTotalKMs { get; set; }
        public double nTotalRate { get; set; }
        public double nTotalWaitingTimeInMinutes { get; set; }
        public double nTotalWaitingPrice { get; set; }
        public double nDiscountAny { get; set; }
        public double nGrandTotal { get; set; }
        public bool btActive { get; set; }
        public bool btPaid { get; set; }
        public string vRemarks { get; set; }
        public int nDriverUserId { get; set; }
        public bool btFinalClosed { get; set; }
    }
}
