namespace TrackingAPI.Models
{
    public class OrderDetails
    {
        public int nLoggedInUserId { get; set; }
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
        public string vOrderId { get; set; }
        public bool btActive { get; set; }
        public bool btPaid { get; set; }
        public string vRemarks { get; set; }
        public int nDriverUserId { get; set; }
        public bool btFinalClosed { get; set; }
        public string vCity { get; set; }
        public double nR1KMs { get; set; }
        public double? nR2KMs { get; set; }
        public double? nR3KMs { get; set; }
        public string vCustomerName1 { get; set; }
        public string vCustomerMobileNo1 { get; set; }
        public string vCustomerName2 { get; set; }
        public string vCustomerMobileNo2 { get; set; }
        public string vCustomerName3 { get; set; }
        public string vCustomerMobileNo3 { get; set; }
        public string vSource { get; set; }
        public string vSourceLat { get; set; }
        public string vSourceLong { get; set; }
        public string vSourceAddress { get; set; }
        public string vD1 { get; set; }
        public string vD1Lat { get; set; }
        public string vD1Long { get; set; }
        public string vD1Address { get; set; }
        public string vD2 { get; set; }
        public string vD2Lat { get; set; }
        public string vD2Long { get; set; }
        public string vD2Address { get; set; }
        public string vD3 { get; set; }
        public string vD3Lat { get; set; }
        public string vD3Long { get; set; }
        public string vD3Address { get; set; }
        public string vItemType { get; set; }
        public bool btCancel { get; set; }
        public string vCancellationReason { get; set; }
    }
}