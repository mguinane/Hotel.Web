using FluentAssertions;
using Hotel.Web.Core.Models;
using Xunit;

namespace Hotel.Web.Tests.Core.Models
{
    public class SearchCriteriaTests
    {
        [Fact]
        public void Constructor_CreateNewSearchCriteria_ReturnSearchCriteria()
        {
            // Arrange
            // Act
            var result = new SearchCriteria();

            // Assert
            var searchCriteria = result.Should().BeOfType<SearchCriteria>().Subject;
        }

        [Fact]
        public void Constructor_CreateNewSearchCriteria_ReturnSearchCriteriaWithDefaultPageIndexValue()
        {
            // Arrange
            var defaultPageIndex = 1;

            // Act
            var result = new SearchCriteria();

            // Assert
            result.PageIndex.Should().Be(defaultPageIndex);
        }

        [Fact]
        public void Constructor_CreateNewSearchCriteria_ReturnSearchCriteriaWithDefaultSortTypeValue()
        {
            // Arrange
            var defaultSortType = SortType.Distance;

            // Act
            var result = new SearchCriteria();

            // Assert
            result.SortType.Should().Be(defaultSortType);
        }
    }
}
