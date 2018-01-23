namespace Hotel.Web.Core.Models
{
    // Class created using Edit -> Paste JSON As Classes (Web Essentials extension)

    public class Establishment
    {
        public float Distance { get; set; }
        public int EstablishmentId { get; set; }
        public string EstablishmentType { get; set; }
        public string Location { get; set; }
        public float MinCost { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
        public float UserRating { get; set; }
        public string UserRatingTitle { get; set; }
        public int UserRatingCount { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }

}
