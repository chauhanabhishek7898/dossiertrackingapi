namespace TrackingAPI.Models
{
    public class UserMaster
    {
        public int nUserId { get; set; }
        public string vFullName { get; set; }
        public string vUserName { get; set; }
        public string vPassword { get; set; }
        public string vMobileNo { get; set; }
        public string vEmailId { get; set; }
        public int nRoleId { get; set; }
        public bool btActive { get; set; }
        public bool btPromotion { get; set; }
        public string vMobileNoOrEmailId { get; set; }
        public int nLoggedInUserId { get; set; }

    }
}
