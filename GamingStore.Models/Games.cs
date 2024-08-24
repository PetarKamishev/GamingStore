using System.Runtime.Serialization;

namespace GamingStore.GamingStore.Models
{
    public class Games
    {
        public required string Title { get; set; }

        public string Desctiption { get; set; } = string.Empty;

        public int Id { get; set; }

        public DateTime ReleaseDate { get; set; }

        public  string GameTags { get; set; }     

        public decimal Price { get; set; }
    }
}
