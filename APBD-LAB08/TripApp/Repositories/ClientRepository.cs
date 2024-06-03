namespace TripApp.Repositories
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task DeleteClient(int idClient, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
