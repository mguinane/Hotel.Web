using AutoMapper;
using Hotel.Web.Data;
using Hotel.Web.Models;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hotel.Web.Controllers
{
    [Route("api/hotels")]
    public class HotelsApiController : Controller
    {
        private IHotelRepository _repository;
        private ILogger<HotelsApiController> _logger;

        private int _pageSize = Int32.Parse(Startup.Configuration["pageSettings:pageSize"]);

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
                _logger.LogError($"Failed to get Hotel search results: {ex.ToString()}");
            }

            return BadRequest("Failed to get Hotel search results");
        }

        [HttpPost]
        public IActionResult Results([FromBody]SearchResultsCriteria criteria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var availability = _repository.GetHotels(criteria, _pageSize);

                    var results = Mapper.Map<AvailabilitySearchViewModel>(availability);

                    return Ok(results);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve Hotel search results: {ex.ToString()}");
            }

            return BadRequest("Failed to retrieve Hotel search results");
        }
    }
}
