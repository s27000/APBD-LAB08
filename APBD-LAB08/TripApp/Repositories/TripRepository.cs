using System.Data.SqlClient;
using TripApp.Model;

namespace TripApp.Repositories
{
    public class TripRepository : Repository, ITripRepository
    {
        public TripRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Client> GetClientList(int idTrip)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "SELECT client.FirstName, client.LastName FROM client\n" +
                "INNER JOIN Client_Trip CT on Client.IdClient = CT.IdClient\n" +
                "Where IdTrip = @IdTrip";
            com.Parameters.AddWithValue("@IdTrip", idTrip);

            using var dr = com.ExecuteReader();

            var clients = new List<Client>();
            
            while(dr.Read())
            {
                var newClient = new Client
                {
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString()
                };

                clients.Add(newClient);
            }
            return clients;
        }

        public List<Country> GetCountryList(int idTrip)
        {
            using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "SELECT country.Name FROM country\n" +
                "INNER JOIN country_trip CT on country.IdCountry = CT.IdCountry\n" +
                "WHERE IdTrip = @IdTrip";
            com.Parameters.AddWithValue("@IdTrip", idTrip);

            using var dr = com.ExecuteReader();

            var countries = new List<Country>();

            while (dr.Read())
            {
                var newCountry = new Country
                {
                    Name = dr["Name"].ToString()
                };

                countries.Add(newCountry);
            }
            return countries;
        }

        public async Task<List<Trip>> GetAsyncTrips(CancellationToken cancellationToken)
        {
            await using SqlCommand com = UnitOfWork().CreateCommand();
            com.CommandText = "SELECT * FROM Trip\n" +
                "Order By DateFrom desc";

            await using var dr = await com.ExecuteReaderAsync(cancellationToken);

            var tripsMap = new Dictionary<int, Trip>();

            while(await dr.ReadAsync(cancellationToken))
            {
                var newTrip = new Trip
                {
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    DateFrom = (DateTime)dr["DateFrom"],
                    DateTo = (DateTime)dr["DateTo"],
                    MaxPeople = (int)dr["MaxPeople"],
                };

                tripsMap.Add((int)dr["IdTrip"],newTrip);
            }
            dr.Close();

            foreach(var idTrip in tripsMap.Keys)
            {
                tripsMap[idTrip].Clients = GetClientList(idTrip);
                tripsMap[idTrip].Countries = GetCountryList(idTrip);
            }

            return tripsMap.Values.ToList();
        }
    }
}
