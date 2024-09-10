namespace GamingStore.GamingStore.Models.Requests
{
    public class AddOrderRequest
    {      

        public int GameId { get; set; }

        public string ClientName { get; set; } 

        public string ClientEmail { get; set; } 

        public DateTime OrderDate { get; set; }
    }
}
