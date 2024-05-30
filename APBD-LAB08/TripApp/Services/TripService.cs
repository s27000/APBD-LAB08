using TripApp.Model;
using TripApp.Repositories;

namespace TripApp.Services
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ITripRepository _tripRepository;

        public TripService(IUnitOfWork unitOfWork, ITripRepository tripRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
        }
        public async Task<List<Trip>> GetAsyncTrips(CancellationToken cancellationToken)
        {
            await _unitOfWork.InitializeAsync(cancellationToken);

            var trips = await _tripRepository.GetAsyncTrips(cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return trips;
        }
    }
}
