using FluentAssertions;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Repositories;
using GamingStore.GamingStore.Models.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStoreTest
{
    public class GetGameTitlesByClientNameServiceTest
    {
        public static List<Games> GamesData = new List<Games>()
        {
            new Games
            {
                Id = 1,
                Title = "Battlefront 2",
                Price = 59.99M,
                ReleaseDate = new DateTime(2017,02,17),
                GameTags="Shooter, MultiPlayer, FPS, SinglePlayer"
            },

            new Games
            {
                Id=2,
                Title = "League Of Legends",
                Price = 0,
                ReleaseDate = new DateTime(2009,10,27),
                GameTags="Moba, MultiPlayer"
            },

            new Games
            {
                Id=10,
                Title= "Overwatch 2",
                Price= 0,
                ReleaseDate = new DateTime(2022,10,04),
                GameTags="FPS, MultiPlayer, Moba, Shooter"
            }
        };

        public static List<Orders> OrdersData = new List<Orders>()
        {
            new Orders
            {
                OrderId = 1,
                GameId = 1,
                ClientEmail = "client1@gmail.com",
                ClientName = "Client First",
                OrderDate = new DateTime(2020,10,21)
            },

            new Orders
            {
                OrderId=2,
                GameId =1,
                ClientEmail = "client2@gmail.com",
                ClientName = "Client Second",
                OrderDate = new DateTime(2020, 08, 12)
            },

            new Orders
            {
                OrderId=3,
                GameId =10,
                ClientEmail = "client3@gmail.com",
                ClientName = "Client First",
                OrderDate = new DateTime(2023, 12, 01)
            }
        };

        [Fact]

        public async Task GetGameTitleByClientName_CheckCount()
        {
            //Setup

            var expectedCount = 2;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            var clientName = "Client First";

            var titles = new List<string>();
            foreach(var game in GamesData)
            {
                if(OrdersData.Where(x=>x.ClientName.Equals(clientName) && x.GameId == game.Id).Any())
                {
                titles.Add(game.Title);
                }
            }
          

            mockedOrdersRepository.Setup(x => x.GetGameTitlesByClientName(clientName)).ReturnsAsync(titles); ;

            //Inject

            var getGameTitleByClientNameService = new GetGameTitlesByClientNameService(mockedOrdersRepository.Object);

            //Act

            var result = getGameTitleByClientNameService.GetGameTitlesByClientName("Client First");

            //Assert

            result.Result.Count.Should().Be(expectedCount);

        }

        [Fact]

        public async Task GetGameTitleByClientName_NotFound()
        {
            //Setup

            var expectedCount = 0;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            var clientName = "Fourth Client";

            var titles = new List<string>();
            foreach (var game in GamesData)
            {
                if (OrdersData.Where(x => x.ClientName.Equals(clientName) && x.GameId == game.Id).Any())
                {
                    titles.Add(game.Title);
                }
            }


            mockedOrdersRepository.Setup(x => x.GetGameTitlesByClientName(clientName)).ReturnsAsync(titles); ;

            //Inject

            var getGameTitleByClientNameService = new GetGameTitlesByClientNameService(mockedOrdersRepository.Object);

            //Act

            var result = getGameTitleByClientNameService.GetGameTitlesByClientName("Fourth Client");

            //Assert

            result.Result.Count.Should().Be(expectedCount);

        }

    }
}
