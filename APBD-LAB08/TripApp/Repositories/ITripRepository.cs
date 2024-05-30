using TripApp.Model;

namespace TripApp.Repositories
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetAsyncTrips(CancellationToken cancellationToken);
        List<Country> GetCountryList(int idTrip);
        List<Client> GetClientList(int idTrip);
    }
}
