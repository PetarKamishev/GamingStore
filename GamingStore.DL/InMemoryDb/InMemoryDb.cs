using GamingStore.GamingStore.Models;
using System.Runtime.Serialization;

namespace GamingStore.GamingStore.DL.InMemoryDb
{
    public class InMemoryDb
    {
        public static List<Games> GamesData = new List<Games>()
        {
            new Games()
            {
                Id = 1,
                Title = "BattleFront 2",
                ReleaseDate = new DateTime(2017, 11, 17),
                GameTags= new string[] {"FPS", "Shooter", "SinglePlayer", "Multiplayer", "Action"},
                Price=9.99M
            }


        };
    }
}
