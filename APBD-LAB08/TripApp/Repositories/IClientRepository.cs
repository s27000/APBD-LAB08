﻿namespace TripApp.Repositories
{
    public interface IClientRepository
    {
        Task DeleteClient(int idClient, CancellationToken cancellationToken);
    }
}