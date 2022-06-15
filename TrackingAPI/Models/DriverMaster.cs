namespace TrackingAPI.Models
{
    public class DriverMaster
    {
        public int nDriverId { get; set; }
        public int nUserId { get; set; }
        public string vDriverId { get; set; }
        public int nVId { get; set; }
        public string vGender { get; set; }
        public string dtDOB { get; set; }
        public int nCityId { get; set; }
        public string vPresentAddress { get; set; }
        public string vPermanentAddress { get; set; }
        public string vAlternateNo { get; set; }
        public string vLicenseNo { get; set; }
        public string vLicenseNoFilePath { get; set; }
        public string vAadhaarNo { get; set; }
        public string vAadhaarNoFilePath { get; set; }
        public string vPANNo { get; set; }
        public string vPANNoFilePath { get; set; }
        public string vVehicleRegistrationNo { get; set; }
        public string vVehicleRegistrationNoFilePath { get; set; }
        public string vVehicleInsuranceFilePath { get; set; }
        public string vPhotoFilePath { get; set; }
        public string vAnyOtherRemarks { get; set; }
        public string vFullName { get; set; }
        public string vMobileNo { get; set; }
        public string vPassword { get; set; }
        public string vEmailId { get; set; }
        public string btPromotion { get; set; }
        public bool btOnDuty { get; set; }
        public string vDiriverCurrentLat { get; set; }
        public string vDiriverCurrentLong { get; set; }

    }
}
