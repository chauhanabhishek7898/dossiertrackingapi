namespace TrackingAPI.Models
{
    public class CorporateMaster
    {
        public int nEId { get; set; }
        public string vEId { get; set; }
        public string vEType { get; set; }
        public string vEstablishmentName { get; set; }
        public string vContactPersonOwner { get; set; }
        public string vCPDesignation { get; set; }
        public string vCPMobileNo { get; set; }
        public string vCPEmailId { get; set; }
        public string vAddress { get; set; }
        public int nCityId { get; set; }
        public string vPinCode { get; set; }
        public string vContactNo { get; set; }
        public string vWhatsUpNo { get; set; }
        public string vEmailId { get; set; }
        public string vWebsiteLink { get; set; }
        public string vTaxDetails { get; set; }
        public string vAuthorizedSignatory { get; set; }
        public string vAuthorizedSignatoryFilePath { get; set; }
        public string vLogoFilePath { get; set; }
        public bool btActive { get; set; }
        public string vPassword { get; set; }
    }
}
