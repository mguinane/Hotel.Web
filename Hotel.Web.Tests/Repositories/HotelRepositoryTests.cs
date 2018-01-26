using FluentAssertions;
using Hotel.Web.Core.Models;
using Hotel.Web.Infrastructure.Repositories;
using Xunit;

namespace Hotel.Web.Tests.Repositories
{
    public class HotelRepositoryTests
    {
        private HotelRepository _repository;

        public HotelRepositoryTests()
        {
            _repository = new HotelRepository();
        }

        [Fact]
        public void GetHotels_InvalidSearchCriteria_ReturnAvailabilitySearchWithNoEstablishments()
        {
            // Arrange
            var searchCriteria = new SearchCriteria() { MinUserRating = 99 };

            // Act
            var result = _repository.GetHotels(searchCriteria, 5);

            // Assert
            var availability = result.Should().BeOfType<AvailabilitySearch>().Subject;
            availability.Establishments.Should().HaveCount(0);
        }

        [Fact]
        public void GetHotels_DefaultSearchCriteria_ReturnAvailabilitySearchWithEstablishments()
        {
            // Arrange
            var searchCriteria = new SearchCriteria();

            // Act
            var result = _repository.GetHotels(searchCriteria, 5);

            // Assert
            var availability = result.Should().BeOfType<AvailabilitySearch>().Subject;
            availability.Establishments.Should().NotBeEmpty();
            availability.Establishments.Should().BeInAscendingOrder(x => x.Distance);
            availability.PageIndex.Should().Be(1);
        }
    }
}
