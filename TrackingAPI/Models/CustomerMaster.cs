﻿namespace TrackingAPI.Models
{
    public class CustomerMaster
    {
        public int nCId { get; set; }
        public string vCId { get; set; }
        public int nUserId { get; set; }
        public string vGender { get; set; }
        public string dtDOB { get; set; }
        public string vAadhaarNo { get; set; }
        public string vAadhaarNoFilePath { get; set; }
        public string vFullName { get; set; }
        public string vMobileNo { get; set; }
        public string vPassword { get; set; }
        public string vEmailId { get; set; }
        public string btPromotion { get; set; }
        public int nCityId { get; set; }
        public string vAddress { get; set; }
        public string vLat { get; set; }
        public string vLong { get; set; }
        public string vFlatNoPlotNoLaneBuilding { get; set; }
    }
}
