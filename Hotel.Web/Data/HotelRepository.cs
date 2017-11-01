using Hotel.Web.Models;
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
            var establishments = _searchResults.Establishments;
            var totalResults = _searchResults.Establishments.Count();

            // Filter results
            bool Filter(Establishment e) => 
                (string.IsNullOrWhiteSpace(criteria.Name) || e.Name.ToLower().Contains(criteria.Name.ToLower())) && 
                ((criteria.Stars == null || criteria.Stars.Length == 0) || criteria.Stars.Contains(e.Stars)) && 
                (criteria.MinUserRating == 0 || e.UserRating >= criteria.MinUserRating) && (criteria.MaxUserRating == 0 || e.UserRating <= criteria.MinUserRating) && 
                (criteria.MinCost == 0 || e.MinCost >= criteria.MinCost);

            establishments = establishments.Where(Filter);

            // Sort results
            switch ((SortType)criteria.SortType)
            {
                case SortType.Stars:
                    establishments = establishments.OrderByDescending(e => e.Stars);
                    break;
                case SortType.MinCost:
                    establishments = establishments.OrderBy(e => e.MinCost);
                    break;
                case SortType.UserRating:
                    establishments = establishments.OrderByDescending(e => e.UserRating)
                        .ThenByDescending(e => e.UserRatingCount);
                    break;
                default:
                    establishments = establishments.OrderBy(e => e.Distance);
                    break;
            }

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
