using Microsoft.AspNetCore.Mvc;
using TripApp.Model;
using TripApp.Repositories;
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

        [HttpDelete("clients/{idClient:int}")]
        public async Task<IActionResult> DeleteClient(int idClient, CancellationToken cancellationToken)
        {
            try
            {
                await _tripService.DeleteClient(idClient, cancellationToken);
                return Ok("Client has been deleted");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("trips/{idTrip:int}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, RequestClientAssignment requestClientAssignment, CancellationToken cancellationToken)
        {
            try
            {
                await _tripService.AssignClientToTrip(idTrip, requestClientAssignment, cancellationToken);
                return Ok("Client has been assigned to Trip");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
