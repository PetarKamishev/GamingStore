namespace GamingStore.GamingStore.Models.Responses
{
    public class AuthenticationResult
    {
        public string token { get; set; }
        public bool isSuccess { get; set; }

        public IEnumerable<string> Error { get; set; }
    }
}
