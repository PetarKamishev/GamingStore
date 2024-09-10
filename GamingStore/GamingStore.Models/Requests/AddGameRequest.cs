using System.Runtime.Serialization;

namespace GamingStore.GamingStore.Models.Requests
{
    public class AddGameRequest
    {
        public string Title { get; init; }

        public DateTime ReleaseDate { get; init; }

        public string GameTags { get; init; }

        public decimal Price { get; init; }
    }
}
