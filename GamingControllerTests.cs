using AutoMapper;
using FluentAssertions;
using GamingStore.AutoMapper;
using GamingStore.Controllers;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using GamingStore.GamingStore.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GamingStoreTest
{
    public class GamingControllerTests
    {

        private readonly IMapper _mapper;

        public GamingControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            _mapper = mockMapper.CreateMapper();
        }
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

        [Fact]

        public async Task GetAllGames_CheckCount()
        {
            //Setup

            var expectedCount = GamesData.Count;

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetAllGames()).ReturnsAsync(GamesData.ToList());

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.GetAllGames();

            //Assert

            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(expectedCount);
        }

        [Fact]

        public async Task GetGameById_OK()
        {
            //Setup
            var gameId = 1;
            var expectedGame = GamesData.First(x => x.Id == gameId);

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).ReturnsAsync(GamesData.First(x => x.Id == gameId));

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.GetGame(gameId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedGame);
        }

        [Fact]

        public async Task GetGameById_NotFound()
        {
            //Setup
            var gameId = 27;
            var expectedGame = GamesData.FirstOrDefault(x => x.Id == gameId);

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).ReturnsAsync(GamesData.FirstOrDefault(x => x.Id == gameId));

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.GetGame(gameId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedGame);
        }

        [Fact]

        public async Task GetGameById_InvalidId()
        {
            //Setup
            var gameId = -1;
            var expectedGame = GamesData.FirstOrDefault(x => x.Id == gameId);

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).ReturnsAsync(GamesData.FirstOrDefault(x => x.Id == gameId));

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.GetGame(gameId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedGame);
        }

        [Fact]
        public async Task GetGameByTitle_OK()
        {
            //Setup

            var gameTitle = "Battlefront 2";
            var expectedGame = GamesData.First(x => x.Title.Contains(gameTitle));

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameTitle)).ReturnsAsync(GamesData.First(x => x.Title.Contains(gameTitle)));

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.GetGame(gameTitle);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedGame);
        }

        [Fact]
        public async Task RemoveGame_OK()
        {
            //Setup

            var gameId = 1;
            var count = GamesData.Count;

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).Returns(async () => GamesData.First(x => x.Id == gameId));

            mockedGamesRepository.Setup(x => x.RemoveGame(gameId)).Callback(() =>
            {
                GamesData.RemoveAt(gameId);
            });

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = gamesController.RemoveGame(gameId);

            //Assert

            result.Should().NotBeNull();
            GamesData.Count.Should().Be(count - 1);
        }

        [Fact]
        public async Task RemoveGame_InvalidId()
        {
            //Setup

            var gameId = -1;
            var count = GamesData.Count;

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).Returns(async () => GamesData.FirstOrDefault(x => x.Id == gameId));

            mockedGamesRepository.Setup(x => x.RemoveGame(gameId)).Callback(() =>
            {
                GamesData.RemoveAt(gameId);
            });

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = gamesController.RemoveGame(gameId);

            //Assert

            result.Should().NotBeNull();
            GamesData.Count.Should().Be(count);
        }

        [Fact]
        public async Task RemoveGame_NotFound()
        {
            //Setup

            var gameId = 24;
            var count = GamesData.Count;

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.GetGame(gameId)).Returns(async () => GamesData.FirstOrDefault(x => x.Id == gameId));

            mockedGamesRepository.Setup(x => x.RemoveGame(gameId)).Callback(() =>
            {
                GamesData.RemoveAt(gameId);
            });

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = gamesController.RemoveGame(gameId);

            //Assert

            result.Should().NotBeNull();
            var resultObj = await result as NotFoundObjectResult;
            GamesData.Count.Should().Be(count);
            resultObj?.Value.Should().Be("Game not found!");
        }

        [Fact]
        public async Task AddGame_OK()
        {
            //Setup

            var count = GamesData.Count;
            var gameToAdd = new AddGameRequest()
            {
                Title = "New Title",
                Price = 0,
                ReleaseDate = DateTime.Now,
                GameTags = "SinglePlayer"
            };

            var mockedGamesRepository = new Mock<IGamesRepository>();
            var game = _mapper.Map<Games>(gameToAdd);


            object value = mockedGamesRepository.Setup(x => x.AddGame(It.IsAny<Games>())).Callback(() =>
            {
                GamesData.Add(game);
            });
            

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = gamesController.AddGame(gameToAdd);

            //Assert

            result.Should().NotBeNull();
            GamesData.Count().Should().Be(count + 1);         
        }

        [Fact]
        public async Task AddGame_GameAlreadyExists()
        {
            //Setup

            var count = GamesData.Count;
            var gameToAdd = new AddGameRequest()
            {
                Title = "League Of Legends",
                Price = 0,
                ReleaseDate = new DateTime(2009, 10, 27),
                GameTags = "Moba, MultiPlayer"
            };

            var mockedGamesRepository = new Mock<IGamesRepository>();
            var game = _mapper.Map<Games>(gameToAdd);


            mockedGamesRepository.Setup(x => x.AddGame(game)).Callback(() =>
            {
                GamesData.Add(game);
            });


           

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = gamesController.AddGame(gameToAdd);

            //Assert
            var response = await result as BadRequestObjectResult;
            response?.Value.Should().NotBeNull();
            response?.Value.Should().Be("Game already exists!");
            GamesData.Count().Should().Be(count);
        }

        [Fact]
        public async Task SearchByTag_OK()
        {
            //Setup
            string gameTag = "Moba";
            int expected_count = 0;
            for(int i=0; i<GamesData.Count; i++)
            {               
                    if (GamesData[i].GameTags.Contains(gameTag))
                    {
                        expected_count++;
                    }
                    else continue;                
            }

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.SearchByTag(gameTag)).ReturnsAsync(GamesData.Where(x => x.GameTags.Contains(gameTag)).ToList());

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.SearchByTag(gameTag);

            //Assert

            var okObjectResult = result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            var games = new List<Games>((IEnumerable<Games>)okObjectResult.Value);
            games.Count.Should().Be(expected_count);

        }

        [Fact]
        public async Task SearchByTag_NotFound()
        {
            //Setup
            string gameTag = "PvP";            

            var mockedGamesRepository = new Mock<IGamesRepository>();

            mockedGamesRepository.Setup(x => x.SearchByTag(gameTag)).ReturnsAsync(GamesData.Where(x => x.GameTags.Contains(gameTag)).ToList());

            //Inject

            var gamesService = new GamesService(mockedGamesRepository.Object);
            var gamesController = new GamesController(gamesService, _mapper);

            //Act

            var result = await gamesController.SearchByTag(gameTag);

            //Assert

            var notFoundObjectResult = result as NotFoundObjectResult;
            notFoundObjectResult?.Value.Should().NotBeNull();
            notFoundObjectResult?.Value.Should().Be("Game not found!");
        }
    }
}