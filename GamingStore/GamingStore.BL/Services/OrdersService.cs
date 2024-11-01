﻿using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.DL.Dataflow;
using GamingStore.GamingStore.DL.Interfaces;
using GamingStore.GamingStore.DL.Kafka;
using GamingStore.GamingStore.Models.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;

namespace GamingStore.GamingStore.BL.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;  
       

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        public async Task AddOrder(Orders orders)
        {
            await _ordersRepository.AddOrder(orders);           
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            var result = await _ordersRepository.GetAllOrders();
            return result;
        }

        public async Task<List<Orders>> GetOrdersByClientName(string clientName)
        {
            var result = await _ordersRepository.GetOrdersByClientName(clientName);
            return result;
        }

        public async Task<List<Orders>> GetOrdersByGameId(int id)
        {
            var result = await _ordersRepository.GetOrdersByGameId(id);
            return result;
        }

        public async Task<Orders> GetOrdersByOrderId(int id)
        {
            var result = await _ordersRepository.GetOrdersByOrderId(id);
            return result;
        }

        public async Task<List<Orders>> GetSpecificGameOrders(string gameTitle)
        {
            var result = await _ordersRepository.GetSpecificGameOrders(gameTitle);
            return result;
        }

        public async Task RemoveOrder(int id)
        {
            await _ordersRepository.RemoveOrder(id);
        }

        public async Task DataflowExecute()
        {
            var dataflow = new DataflowConsumeAndProduce();
            await dataflow.DataflowConsume();         
        }
    }
}
