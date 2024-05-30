﻿using Microsoft.AspNetCore.Mvc;
using TripApp.Model;
using TripApp.Services;

namespace TripApp.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripController(ITripService tripService) {
            _tripService = tripService;
        }

        [HttpGet("trips")]
        public async Task<IActionResult> GetTrips(CancellationToken cancellationToken, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var fullTripsList = await _tripService.GetAsyncTrips(cancellationToken);
            var pageTripsList = new List<Trip>();

            if (pageSize<=0)
            {
                pageSize = 10;
            }
            if (page <= 0)
            {
                page = 1;
            }

            int pageStartIndex = (page-1) * pageSize;
            int iterator = 0;

            while(pageStartIndex + iterator < fullTripsList.Count() && iterator < pageSize)
            {
                pageTripsList.Add(fullTripsList.ElementAt(pageStartIndex + iterator));
                iterator++;
            }

            int allPages = fullTripsList.Count() / pageSize + 1;

            var tripPage = new TripPage
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = allPages,
                Trips = pageTripsList
            };

            return Ok(tripPage);
        }
    }
}