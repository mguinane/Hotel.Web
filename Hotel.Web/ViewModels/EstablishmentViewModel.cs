using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.ViewModels
{
    public class EstablishmentViewModel
    {
        [DisplayFormat(DataFormatString = "{0:0.#}")]
        public float Distance { get; set; }
        public string Location { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
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
