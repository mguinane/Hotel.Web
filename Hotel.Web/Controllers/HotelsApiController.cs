using AutoMapper;
using Hotel.Web.Data;
using Hotel.Web.Models;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Hotel.Web.Filters;

namespace Hotel.Web.Controllers
{
    [Route("api/hotels")]
    [ValidateModel]
    public class HotelsApiController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly ILogger<HotelsApiController> _logger;

        private readonly int _pageSize = int.Parse(Startup.Configuration["pageSettings:pageSize"]);

        public HotelsApiController(IHotelRepository repository, ILogger<HotelsApiController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var availability = _repository.GetHotels(new SearchResultsCriteria() { PageIndex = 1, SortType = (int)SortType.Distance }, _pageSize);

                var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Hotel search results: {ex}");
            }

            return BadRequest("Failed to get Hotel search results");
        }

        [HttpPost]
        public IActionResult Results([FromBody]SearchResultsCriteria criteria)
        {
            try
            {
                var availability = _repository.GetHotels(criteria, _pageSize);

                var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve Hotel search results: {ex}");
            }

            return BadRequest("Failed to retrieve Hotel search results");
        }
    }
}
