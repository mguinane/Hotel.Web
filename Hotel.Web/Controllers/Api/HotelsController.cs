using AutoMapper;
using Hotel.Web.Core.Models;
using Hotel.Web.Core.Repositories;
using Hotel.Web.Filters;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hotel.Web.Controllers.Api
{
    [Route("api/hotels")]
    [ValidateModel]
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly ILogger<HotelsController> _logger;

        private readonly int _pageSize = int.Parse(Startup.Configuration["pageSettings:pageSize"]);

        public HotelsController(IHotelRepository repository, ILogger<HotelsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var availability = _repository.GetHotels(new SearchCriteria(), _pageSize);

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
        public IActionResult Results([FromBody]SearchCriteriaViewModel criteria)
        {
            try
            {
                var availability = _repository.GetHotels(Mapper.Map<SearchCriteria>(criteria), _pageSize);

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
