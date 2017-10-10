using Hotel.Web.Models;

namespace Hotel.Web.Data
{
    public interface IHotelRepository
    {
        AvailabilitySearch GetHotels(SearchResultsCriteria criteria, int pageSize);
    }
}
