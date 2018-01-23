using Hotel.Web.Core.Models;

namespace Hotel.Web.Core.Repositories
{
    public interface IHotelRepository
    {
        AvailabilitySearch GetHotels(SearchCriteria criteria, int pageSize);
    }
}
