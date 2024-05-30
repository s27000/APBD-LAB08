namespace TripApp.Model
{
    public class TripPage
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get;set; }
        public List<Trip> Trips { get; set; }
    }
}
