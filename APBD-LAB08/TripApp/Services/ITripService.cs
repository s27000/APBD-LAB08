using TripApp.Model;

namespace TripApp.Services
{
    public interface ITripService
    {
        Task<List<Trip>> GetAsyncTrips(CancellationToken cancellation);
    }
}
