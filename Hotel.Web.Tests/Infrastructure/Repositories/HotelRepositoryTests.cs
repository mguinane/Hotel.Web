using FluentAssertions;
using Hotel.Web.Core.Models;
using Hotel.Web.Infrastructure.Repositories;
using Xunit;

namespace Hotel.Web.Tests.Infrastucture.Repositories
{
    public class HotelRepositoryTests
    {
        private HotelRepository _repository;
        private SearchCriteria _searchCriteria;

        private const int DefaultPageSize = 5;

        public HotelRepositoryTests()
        {
            _repository = new HotelRepository();
            _searchCriteria = new SearchCriteria();
        }

        [Fact]
        public void GetHotels_DefaultSearchCriteria_ReturnAvailabilitySearch()
        {
            // Arrange
            // Act
            var result = _repository.GetHotels(_searchCriteria, DefaultPageSize);

            // Assert
            var availability = result.Should().BeOfType<AvailabilitySearch>().Subject;
        }

        [Fact]
        public void GetHotels_DefaultSearchCriteria_ReturnAvailabilitySearchWithEstablishments()
        {
            // Arrange
            // Act
            var result = _repository.GetHotels(_searchCriteria, DefaultPageSize);

            // Assert
            result.Establishments.Should().NotBeEmpty();
        }

        [Fact]
        public void GetHotels_DefaultSearchCriteria_ReturnAvailabilitySearchWithEstablishmentsSortedByDistance()
        {
            // Arrange
            // Act
            var result = _repository.GetHotels(_searchCriteria, DefaultPageSize);

            // Assert
            result.Establishments.Should().BeInAscendingOrder(x => x.Distance);
        }

        [Fact]
        public void GetHotels_InvalidSearchCriteria_ReturnAvailabilitySearchWithNoEstablishments()
        {
            // Arrange
            var searchCriteria = new SearchCriteria() { MinUserRating = 99 };

            // Act
            var result = _repository.GetHotels(searchCriteria, 5);

            // Assert
            result.Establishments.Should().HaveCount(0);
        }
    }
}
