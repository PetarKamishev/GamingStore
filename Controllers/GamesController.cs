using GamingStore.GamingStore.BL.Interfaces;
using GamingStore.GamingStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace GamingStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
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
            if (id == 0) return BadRequest();
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
        public async Task<IActionResult> AddGame([FromBody] Games game)
        {
            if (game == null) return BadRequest();
            else
            {
                await _gamesService.AddGame(game);
                return Ok();
            }
        }

        [HttpDelete("RemoveGame")]

        public async Task<IActionResult> RemoveGame(int id)
        {
            if (id.Equals(0)) return BadRequest();
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
                if (result == null || result.Count == 0) return NotFound();
                else return Ok(result);
            }
        }

    }
}
