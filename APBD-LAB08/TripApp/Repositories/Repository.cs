namespace TripApp.Repositories
{
    public class Repository
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork()
        {
            return _unitOfWork;
        }
    }
}
