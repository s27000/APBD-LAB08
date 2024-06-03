using TripApp.Model;
using TripApp.Repositories;

namespace TripApp.Services
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ITripRepository _tripRepository;
        private readonly IClientRepository _clientRepository;

        public TripService(IUnitOfWork unitOfWork, ITripRepository tripRepository, IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _clientRepository = clientRepository;
        }

        public async Task DeleteClient(int idClient, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.InitializeAsync(cancellationToken);

                await _clientRepository.DeleteClient(idClient, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
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
