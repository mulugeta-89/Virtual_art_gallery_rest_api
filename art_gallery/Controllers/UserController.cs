using art_gallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace art_gallery.Controllers
{
    [Authorize]
    [Route("/{userId}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly ArtsService _artService;
        public UserController(ArtsService artService)
        {
            _artService = artService;
        }
        [HttpGet("arts")]
        public async Task<IActionResult> Get()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            var arts = await _artService.GetSpecificAsync(userId);
            return Ok(arts);
        }

    }
}
