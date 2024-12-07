using AutoMapper;
using GamingStore.GamingStore.BL.BackgroundJobs;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.BL.Services;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.Models.Models;
using GamingStore.GamingStore.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrderController(IOrdersService ordersService, IMapper mapper)
        {
            _ordersService = ordersService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAllOrders")]

        public async Task<List<Orders>> GetAllOrders()
        {
            return await _ordersService.GetAllOrders();
        }

        [HttpGet("GetOrdersByGameId")]

        public async Task<List<Orders>> GetOrdersByGameId(int gameId)
        {
            if (gameId >= 0)
            {
                return await _ordersService.GetOrdersByGameId(gameId);
            }
            else return new List<Orders>();
        }

        [HttpGet("GetOrdersByOrderId")]

        public async Task<IActionResult> GetOrdersByOrderId(int orderId)
        {

            var result = await _ordersService.GetOrdersByGameId(orderId);
            if (result == null) return NotFound(orderId);
            return Ok(result);
        }

        [HttpGet("GetOrdersByClientName")]

        public async Task<List<Orders>> GetOrdersByClientName(string clientName)
        {
            if (!string.IsNullOrEmpty(clientName))
            {
                return await _ordersService.GetOrdersByClientName(clientName);
            }
            else return new List<Orders>();
        }

        [HttpGet("GetSpecificGameOrders")]

        public async Task<List<Orders>> GetSpecificGameOrders(string gameTitle)
        {
            if (!string.IsNullOrEmpty(gameTitle))
            {
                return await _ordersService.GetSpecificGameOrders(gameTitle);
            }
            else return new List<Orders>();
        }

        [HttpGet("GetOrdersCount")]

        public async Task<int> GetOrdersCount()
        {
            return await _ordersService.GetOrdersCount();
        }

        [HttpPost("AddOrder")]

        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (request == null) return BadRequest();
            else
            {
                var orderToAdd = _mapper.Map<Orders>(request);
                await _ordersService.AddOrder(orderToAdd);
                var produceOrderService = new ProduceOrderService(new KafkaProducer());
                await produceOrderService.ProduceOrder(orderToAdd);
                return Ok(orderToAdd);
            }
        }


        [HttpDelete("RemoveOrder")]

        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid Id!");
            var order = await _ordersService.GetOrdersByOrderId(orderId);
            if (order == null) return BadRequest("Order not found!");
            else
            {
                await _ordersService.RemoveOrder(orderId);
                return Ok(order);
            }
        }
    }
}
