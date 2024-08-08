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
                GameTags= new string[] {"FPS", "Shooter", "SinglePlayer", "MultiPlayer", "Action"},
                Price=9.99M
            },

            new Games()
            {
                Id=2,
                Title = "League of Legends",
                ReleaseDate = new DateTime(2009, 10, 27),
                GameTags= new string[] {"Moba", "MultiPlayer"},
                Price=0M
            },

            new Games()
            {
                Id=3,
                Title="Hogwarts Legacy",
                ReleaseDate = new DateTime(2023, 02, 10),
                GameTags= new string[] {"Puzzle", "SinglePlayer", "Action", "RPG", "Adventure"},
                Price=59.99M
            }
        };
    }
}
