using Microsoft.AspNetCore.Server.IIS.Core;
using System.Data.SqlClient;
using System.Net.Sockets;
using TripApp.Model;

namespace TripApp.Repositories
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task DeleteClient(int idClient, CancellationToken cancellationToken)
        {
            if (HasTripsAssigned(idClient))
            {
                throw new Exception("Client has trips assigned");
            }
            await using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "delete from client where idCLient = @IdClient";
            com.Parameters.AddWithValue("@IdClient", idClient);

            await com.ExecuteScalarAsync(cancellationToken);
        }
        public bool HasTripsAssigned(int idClient)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "select count(idtrip) from client\n" +
                "inner join client_trip ct on client.idClient = ct.idClient\n" +
                "where client.idclient = @idClient";
            com.Parameters.AddWithValue("@idClient", idClient);

            int result = (int)com.ExecuteScalar();

            if(result == 0)
            {
                return false;
            }
            return true;
        }
    }
}
