using MessagePack;

namespace GamingStore.GamingStore.Models.Models
{
    [MessagePackObject]
    public class Orders
    {
        [Key(0)]
        public int OrderId { get; set; }

        [Key(1)]
        public int GameId { get; set; }

        [Key(2)]
        public string ClientName { get; set; } = string.Empty;

        [Key(3)]
        public string ClientEmail { get; set; } = string.Empty;

        [Key(4)]
        public DateTime OrderDate { get; set; }

    }
}
