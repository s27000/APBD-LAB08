using Microsoft.AspNetCore.Server.IIS.Core;
using System.Data.SqlClient;
using System.Net.Sockets;
using TripApp.Model;

namespace TripApp.Repositories
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task AssignClientToTrip(int idTrip, RequestClientAssignment requestClientAssignment, CancellationToken cancellationToken)
        {
            if (DoesClientExist(requestClientAssignment))
            {
                throw new Exception("Client with such PESEL already exists");
            }

            if(IsClientAlreadyAssigned(idTrip, requestClientAssignment))
            {
                throw new Exception("Client with such PESEL is already assigned to this trip");
            }

            if (IsTripUpToDate(idTrip))
            {
                throw new Exception("Trip does not exist or is already in the past");
            }

            await using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)\n" +
                "VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);" +
                "SELECT SCOPE_IDENTITY()";
            com.Parameters.AddWithValue("@FirstName", requestClientAssignment.FirstName);
            com.Parameters.AddWithValue("@LastName", requestClientAssignment.LastName);
            com.Parameters.AddWithValue("@Email", requestClientAssignment.Email);
            com.Parameters.AddWithValue("@Telephone", requestClientAssignment.Telephone);
            com.Parameters.AddWithValue("@Pesel", requestClientAssignment.Pesel);

            int newCLientId = (int)(decimal)await com.ExecuteScalarAsync(cancellationToken);

            await using SqlCommand com2 = UnitOfWork().CreateCommand();
            com2.CommandText = "insert into Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)\n" +
                "values (@IdClient, @IdTrip, @RegisteredAt, @PaymentDate)";
            com2.Parameters.AddWithValue("@IdClient", newCLientId);
            com2.Parameters.AddWithValue("@IdTrip", idTrip);
            com2.Parameters.AddWithValue("@RegisteredAt", DateTime.Now);
            com2.Parameters.AddWithValue("@PaymentDate", requestClientAssignment.PaymentDate);

            await com2.ExecuteNonQueryAsync(cancellationToken);
        }

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

        public bool DoesClientExist(RequestClientAssignment requestClientAssignment)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "select count(pesel) from client\n" +
                    "where pesel = @Pesel";
            com.Parameters.AddWithValue("@Pesel", requestClientAssignment.Pesel);

            int result = (int)com.ExecuteScalar();

            if (result == 0)
            {
                return false;
            }
            return true;
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

        public bool IsClientAlreadyAssigned(int idTrip, RequestClientAssignment requestClientAssignment)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "select count(pesel) from client\n"+
                "inner join client_trip ct on client.idClient = ct.idClient\n" +
                "where pesel = @Pesel and idTrip = @IdTrip";
            com.Parameters.AddWithValue("@Pesel", requestClientAssignment.Pesel);
            com.Parameters.AddWithValue("@IdTrip", idTrip);

            int result = (int)com.ExecuteScalar();

            if (result == 0)
            {
                return false;
            }
            return true;
        }

        public bool IsTripUpToDate(int idTrip)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "select count(idTrip) from trip\n"+
                "where idTrip = @idTrip and DateTo < getdate()";
            com.Parameters.AddWithValue("@IdTrip", idTrip);

            int result = (int)com.ExecuteScalar();

            if (result == 0)
            {
                return false;
            }
            return true;
        }
    }
}
