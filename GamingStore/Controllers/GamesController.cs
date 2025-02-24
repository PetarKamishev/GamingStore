﻿using AutoMapper;
using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models.Models;
using GamingStore.GamingStore.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;
        public GamesController(IGamesService gamesService, IMapper mapper)
        {
            _gamesService = gamesService;
            _mapper = mapper;

        }

        [HttpGet("GetAllGames")]
        public async Task<List<Games>> GetAllGames()
        {
            var result = await _gamesService.GetAllGames();
            return result;
        }

        [HttpGet("GetGame")]
        public async Task<IActionResult> GetGame(int id)
        {
            if (id <= 0) return BadRequest();
            else
            {
                var result = await _gamesService.GetGame(id);
                return result != null ? Ok(result) : NotFound();
            }
        }

        [HttpGet("GetGameByTitle")]
        public async Task<IActionResult> GetGame(string title)
        {
            if (title == null) return BadRequest();
            else
            {
                var result = await _gamesService.GetGame(title);
                return result != null ? Ok(result) : NotFound();
            }
        }

        [HttpPost("AddGame")]
        public async Task<IActionResult> AddGame([FromBody] AddGameRequest addGameRequest)
        {
            if (addGameRequest == null) return BadRequest(addGameRequest);

            var game = await _gamesService.GetGame(addGameRequest.Title);
            if (game != null) return BadRequest("Game already exists!");
            else
            {
                var gameToAdd = _mapper.Map<Games>(addGameRequest);
                await _gamesService.AddGame(gameToAdd);
                return Ok();
            }
        }

        [HttpPatch("AddGameTag")]

        public async Task<Games> AddGameTag([FromBody] AddGameTagRequest addGameTagRequest)
        {
            await _gamesService.AddGameTag(addGameTagRequest.Title, addGameTagRequest.GameTag);
            return await _gamesService.GetGame(addGameTagRequest.Title);         
        }

        [HttpDelete("RemoveGameTag")]

        public async Task<Games> RemoveGameTag([FromBody] RemoveGameTagRequest removeGameTagRequest)
        {
            await _gamesService.RemoveGameTag(removeGameTagRequest.Title, removeGameTagRequest.GameTag);
            return await _gamesService.GetGame(removeGameTagRequest.Title);           
        }

        [HttpDelete("RemoveGame")]

        public async Task<IActionResult> RemoveGame(int id)
        {
            if (id <= 0) return BadRequest("Invalid Id!");
            var game = await _gamesService.GetGame(id);
            if (game == null) return BadRequest("Game not found!");
            else
            {
                await _gamesService.RemoveGame(id);
                return Ok();
            }
        }

        [HttpGet("SearchByTag")]

        public async Task<IActionResult> SearchByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return BadRequest();
            else
            {
                var result = await _gamesService.SearchByTag(tag);
                if (result == null || result.Count == 0) return NotFound("Game not found!");
                else return Ok(result);
            }
        }

    }
}
