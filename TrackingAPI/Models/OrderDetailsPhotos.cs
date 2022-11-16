namespace TrackingAPI.Models
{
    public class OrderDetailsPhotos
    {
        public int nImageId { get; set; }
        public int nTrackId { get; set; }
        public string vPhotoFilePath { get; set; }
        public string vRemarks { get; set; }
        public bool btIsTrackImage { get; set; }
    }
}
