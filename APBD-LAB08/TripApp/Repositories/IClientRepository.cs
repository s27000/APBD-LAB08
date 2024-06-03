using TripApp.Model;

namespace TripApp.Repositories
{
    public interface IClientRepository
    {
        Task DeleteClient(int idClient, CancellationToken cancellationToken);
        Task AssignClientToTrip(int idTrip, RequestClientAssignment requestClientAssignment, CancellationToken cancellationToken);
        bool HasTripsAssigned(int idClient);
        bool DoesClientExist(RequestClientAssignment requestClientAssignment);
        bool IsClientAlreadyAssigned(int idTrip, RequestClientAssignment requestClientAssignment);

        bool IsTripUpToDate(int idTrip);
    }
}
