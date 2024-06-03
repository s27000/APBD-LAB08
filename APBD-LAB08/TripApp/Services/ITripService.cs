using TripApp.Model;

namespace TripApp.Services
{
    public interface ITripService
    {
        Task<List<Trip>> GetAsyncTrips(CancellationToken cancellationToken);
        Task DeleteClient(int idClient, CancellationToken cancellationToken);
        Task AssignClientToTrip(int idTrip, RequestClientAssignment requestClientAssignment, CancellationToken cancellationToken);
    }
}
