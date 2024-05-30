using System.Net;

namespace TripApp.Model
{
    public class Trip
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<Country> Countries { get; set; }
        public List<Client> Clients { get; set; }

        public string ToString()
        {
            return "TripModel||Name:" + Name + "|Description:" + "|DateFrom:" + DateFrom + "|DateTo:" + DateTo + "|MaxPeople:" + MaxPeople;
        }
    }
}
