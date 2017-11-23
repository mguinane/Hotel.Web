using Hotel.Web.Models;
using Hotel.Web.Models.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hotel.Web.Data
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AvailabilitySearch _searchResults;

        public HotelRepository()
        {
            _searchResults = JsonConvert.DeserializeObject<AvailabilitySearch>(File.ReadAllText(@"Data\hotels.json"));
        }

        public AvailabilitySearch GetHotels(SearchResultsCriteria criteria, int pageSize)
        {
            var totalResults = _searchResults.Establishments.Count();

            // Filter & Sort results
            var establishments = _searchResults.Establishments.Filter(criteria).Sort((SortType)criteria.SortType);

            // Page results
            var establishmentList = establishments as IList<Establishment> ?? establishments.ToList();

            var availability = new AvailabilitySearch()
            {
                AvailabilitySearchId = _searchResults.AvailabilitySearchId,
                Establishments = establishmentList.Skip((criteria.PageIndex - 1) * pageSize).Take(pageSize),
                PageIndex = criteria.PageIndex,
                PageCount = (int)Math.Ceiling(establishmentList.Count() / (double)pageSize),
                TotalResults = totalResults
            };

            return availability;
        }
    }   
}
