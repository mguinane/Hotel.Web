using FluentAssertions;
using Hotel.Web.Core.Models;
using Xunit;

namespace Hotel.Web.Tests.Core.Models
{
    public class SearchCriteriaTests
    {
        [Fact]
        public void Constructor_CreateNewSearchCriteria_ReturnSearchCriteriaWithDefaultPropertyValues()
        {
            // Arrange
            // Act
            var result = new SearchCriteria();

            // Assert
            var searchCriteria = result.Should().BeOfType<SearchCriteria>().Subject;
            searchCriteria.PageIndex.Should().Be(1);
            searchCriteria.SortType.Should().Be(SortType.Distance);
        }
    }
}
