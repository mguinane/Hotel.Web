using AutoMapper;
using FluentAssertions;
using Hotel.Web.Controllers;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.Infrastructure.Services.Logging;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hotel.Web.Tests.Controllers
{
    public class HotelsControllerTests
    {
        private HotelsController _hotelsController;
        private Mock<IHotelRepository> _mockHotelRepository;
        private Mock<ILoggerAdapter<HotelsController>> _mockLogger;
        private Mock<IOptions<PageSettings>> _mockOptions;
        private PageSettings _pageSettings;

        public HotelsControllerTests()
        {
            _mockHotelRepository = new Mock<IHotelRepository>();
            _mockLogger = new Mock<ILoggerAdapter<HotelsController>>();
            _mockOptions = new Mock<IOptions<PageSettings>>();
            _pageSettings = new PageSettings() { PageSize = 5 };
            _mockOptions.Setup(p => p.Value).Returns(_pageSettings);
            _hotelsController = new HotelsController(_mockHotelRepository.Object, _mockLogger.Object, _mockOptions.Object);

            Mapper.Initialize(mapConfig =>
            {
                mapConfig.CreateMap<SearchCriteriaViewModel, SearchCriteria>();
                mapConfig.CreateMap<Establishment, EstablishmentViewModel>();
                mapConfig.CreateMap<AvailabilitySearch, AvailabilitySearchViewModel>();
            });
        }

        [Fact]
        public void Index_AnySearchCriteria_ReturnViewResult()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());

            // Act
            var result = _hotelsController.Index();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        }

        [Fact]
        public void Index_AnySearchCriteriaAndEmptyRepository_ReturnAvailabilitySearchViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());

            // Act
            var result = _hotelsController.Index() as ViewResult;

            // Assert
            result.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Index_AnySearchCriteriaAndNonEmptyRepository_ReturnAvailabilitySearchViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetFakeAvailabilitySearch());

            // Act
            var result = _hotelsController.Index() as ViewResult;

            // Assert
            result.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Results_AnySearchCriteria_ReturnPartialViewResult()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel());

            // Assert
            var viewResult = result.Should().BeOfType<PartialViewResult>().Subject;
        }

        [Fact]
        public void Results_AnySearchCriteriaAndEmptyRepository_ReturnAvailabilitySearchViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => new AvailabilitySearch());

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel()) as PartialViewResult;

            // Assert
            result.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Results_AnySearchCriteriaAndNonEmptyRepository_ReturnAvailabilitySearchViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetFakeAvailabilitySearch());

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel()) as PartialViewResult;

            // Assert
            result.Model.Should().BeOfType<AvailabilitySearchViewModel>();
        }

        [Fact]
        public void Index_RepositoryThrowsException_ReturnRedirectToErrorActionResult()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Index();

            // Assert
            var redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        }

        [Fact]
        public void Index_RepositoryThrowsException_ReturnActionNameAsError()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Index() as RedirectToActionResult;

            // Assert
            result.ActionName.Should().Be("Error");
        }

        [Fact]
        public void Results_RepositoryThrowsException_ReturnPartialViewResult()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
        }

        [Fact]
        public void Results_RepositoryThrowsException_ReturnNullViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel()) as PartialViewResult;

            // Assert
            result.Model.Should().BeNull();
        }

        [Fact]
        public void Results_ModelStateInvalid_ReturnPartialViewResult()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetFakeAvailabilitySearch());
            _hotelsController.ModelState.AddModelError("Name", "name property is too long");

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel());

            // Assert
            var partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
        }

        [Fact]
        public void Results_ModelStateInvalid_ReturnNullViewModel()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Returns(() => GetFakeAvailabilitySearch());
            _hotelsController.ModelState.AddModelError("Name", "name property is too long");

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel()) as PartialViewResult;

            // Assert
            result.Model.Should().BeNull();
        }

        [Fact]
        public void Index_ExceptionThrown_ErrorLogged()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Index();

            // Assert
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()));
        }

        [Fact]
        public void Results_ExceptionThrown_ErrorLogged()
        {
            // Arrange
            _mockHotelRepository.Setup(r => r.GetHotels(It.IsAny<SearchCriteria>(), It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _hotelsController.Results(new SearchCriteriaViewModel());

            // Assert
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()));
        }

        private AvailabilitySearch GetFakeAvailabilitySearch()
        {
            var establishments = new List<Establishment>()
            {
                new Establishment() { Name = "Hotel 1", Stars = 1, Location = "Town 1" },
                new Establishment() { Name = "Hotel 2", Stars = 2, Location = "Town 2" },
                new Establishment() { Name = "Hotel 3", Stars = 3, Location = "Town 3" },
                new Establishment() { Name = "Hotel 4", Stars = 4, Location = "Town 4" },
                new Establishment() { Name = "Hotel 5", Stars = 5, Location = "Town 5" },
            };

            return new AvailabilitySearch("1", establishments, 1, 1);
        }
    }
}
