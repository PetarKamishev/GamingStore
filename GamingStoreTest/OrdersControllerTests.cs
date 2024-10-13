using AutoMapper;
using FluentAssertions;
using GamingStore.AutoMapper;
using GamingStore.Controllers;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using GamingStore.GamingStore.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStoreTest
{
    public class OrdersControllerTests
    {
        private readonly IMapper _mapper;

        public OrdersControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            _mapper = mockMapper.CreateMapper();
        }

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
                ClientName = "Client Third",
                OrderDate = new DateTime(2023, 12, 01)
            }
        };

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

        public async Task GetAllOrders_CheckCount()
        {
            //Setup
            var expectedCount = OrdersData.Count;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.GetAllOrders()).ReturnsAsync(OrdersData.ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.GetAllOrders();

            //Assert

            result.Should().NotBeEmpty();
            result.Count.Should().Be(expectedCount);
        }

        [Fact]

        public async Task GetOrdersByGameId_OK()
        {
            //Setup

            var gameId = 1;
            var expectedOrders = OrdersData.FindAll(x => x.GameId == gameId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.GetOrdersByGameId(gameId)).Returns(async () => OrdersData.Where(x => x.GameId == gameId).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = ordersController.GetOrdersByGameId(gameId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task GetOrdersByGameId_NotFound()
        {
            //Setup

            var gameId = 3;
            var expectedOrders = OrdersData.FindAll(x => x.GameId == gameId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.GetOrdersByGameId(gameId)).Returns(async () => OrdersData.Where(x => x.GameId == gameId).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.GetOrdersByGameId(gameId);

            //Assert

            result.Should().BeNullOrEmpty();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task GetOrdersByGameId_InvalidId()
        {
            //Setup

            var gameId = -1;
            var expectedOrders = OrdersData.FindAll(x => x.GameId == gameId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.GetOrdersByGameId(gameId)).Returns(async () => OrdersData.Where(x => x.GameId == gameId).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.GetOrdersByGameId(gameId);

            //Assert

            result.Should().BeNullOrEmpty();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task GetOrdersByOrderId_OK()
        {
            //Setup 
            var orderId = 1;
            var expectedOrder = OrdersData.FirstOrDefault(x => x.OrderId == orderId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByOrderId(orderId)).ReturnsAsync(OrdersData.FirstOrDefault(x => x.OrderId == orderId));

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.GetOrdersByOrderId(orderId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedOrder);
        }

        [Fact]

        public async Task GetOrdersByOrderId_NotFound()
        {
            //Setup 
            var orderId = 4;
            var expectedOrder = OrdersData.FirstOrDefault(x => x.OrderId == orderId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByOrderId(orderId)).ReturnsAsync(OrdersData.FirstOrDefault(x => x.OrderId == orderId));

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.GetOrdersByOrderId(orderId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedOrder);
        }

        [Fact]

        public async Task GetOrdersByOrderId_InvalidId()
        {
            //Setup 
            var orderId = -1;
            var expectedOrder = OrdersData.FirstOrDefault(x => x.OrderId == orderId);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByOrderId(orderId)).ReturnsAsync(OrdersData.FirstOrDefault(x => x.OrderId == orderId));

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.GetOrdersByOrderId(orderId);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedOrder);
        }

        [Fact]

        public async Task GetOrdersByClientName_OK()
        {
            //Setup 

            var clientName = "Client First";
            var expectedOrders = OrdersData.FindAll(x => x.ClientName.Contains(clientName));

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByClientName(clientName)).ReturnsAsync(OrdersData.Where(x => x.ClientName.Contains(clientName)).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.GetOrdersByClientName(clientName);

            //Assert

            result.Should().NotBeNull();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task GetOrdersByClientName_NotFound()
        {
            //Setup

            var clientName = "Client Fourth";
            var expectedOrders = OrdersData.FindAll(x => x.ClientName.Contains(clientName));

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByClientName(clientName)).ReturnsAsync(OrdersData.Where(x => x.ClientName.Contains(clientName)).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.GetOrdersByClientName(clientName);

            //Assert

            result.Should().BeNullOrEmpty();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task GetSpecificGameOrders_OK()
        {
            //Setup

            var gameTitle = "Battlefront 2";
            var game = GamesData.FirstOrDefault(x => x.Title.Contains(gameTitle));
            var expectedOrders = OrdersData.FindAll(x => x.GameId == game.Id);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.GetSpecificGameOrders(gameTitle)).Returns(async () => OrdersData.Where(x => x.GameId == game.Id).ToList());

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.GetSpecificGameOrders(gameTitle);

            //Assert

            result.Should().NotBeNullOrEmpty();
            result.Equals(expectedOrders);
        }

        [Fact]

        public async Task AddOrder_OK()
        {
            //Setup

            var request = new AddOrderRequest
            {
                GameId = 1,
                ClientEmail="newclientemail@gmail.com",
                ClientName="New Client",
                OrderDate= DateTime.Now
            };

            var ordersCount = OrdersData.Count;
            var order = _mapper.Map<Orders>(request);

            var mockedOrdersRepository = new Mock<IOrdersRepository>();

            mockedOrdersRepository.Setup(x => x.AddOrder(It.IsAny<Orders>())).Callback(() =>
            {
                OrdersData.Add(order);
            });

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.AddOrder(request);

            //Assert

            result.Should().NotBeNull();
            OrdersData.Count.Should().Be(ordersCount+1);
        }

        [Fact]

        public async Task RemoveOrder_OK()
        {
            //Setup

            var orderId = 1;
            var orderCount = OrdersData.Count;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x=>x.GetOrdersByOrderId(orderId))
                .Returns(async ()=> OrdersData.FirstOrDefault(x=>x.OrderId == orderId));

            mockedOrdersRepository.Setup(x => x.RemoveOrder(orderId)).Callback(() =>
            {
                OrdersData.RemoveAt(orderId);
            });

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.RemoveOrder(orderId);

            //Assert

            result.Should().NotBeNull();
            OrdersData.Count.Should().Be(orderCount - 1);
        }

        [Fact]

        public async Task RemoveOrder_OrderNotFound()
        {
            //Setup

            var orderId = 0;
            var ordersCount = OrdersData.Count;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByOrderId(orderId)).ReturnsAsync(OrdersData.FirstOrDefault(x => x.OrderId == orderId));
            mockedOrdersRepository.Setup(x => x.RemoveOrder(orderId)).Callback(() =>
            {
                OrdersData.RemoveAt(orderId);
            });

            //Inject

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act

            var result = await ordersController.RemoveOrder(orderId);

            //Assert

            result.Should().NotBeNull();
            OrdersData.Count.Should().Be(ordersCount);           
        }

        [Fact]

        public async Task RemoveOrder_InvalidOrderId()
        {
            //Setup

            var orderId = -1;
            var ordersCount = OrdersData.Count;

            var mockedOrdersRepository = new Mock<IOrdersRepository>();
            mockedOrdersRepository.Setup(x => x.GetOrdersByOrderId(orderId)).ReturnsAsync(OrdersData.FirstOrDefault(x => x.OrderId == orderId));
            mockedOrdersRepository.Setup(x => x.RemoveOrder(orderId)).Callback(() =>
            {
                OrdersData.RemoveAt(orderId);
            });

            //Inject 

            var ordersService = new OrdersService(mockedOrdersRepository.Object);
            var ordersController = new OrderController(ordersService, _mapper);

            //Act 

            var result = await ordersController.RemoveOrder(orderId);

            //Asssert

            result.Should().NotBeNull();
            OrdersData.Count.Should().Be(ordersCount);
        }
    }
}
