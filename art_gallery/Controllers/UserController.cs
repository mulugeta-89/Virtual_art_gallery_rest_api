using art_gallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace art_gallery.Controllers
{
    [Authorize]
    [Route("api/{userId}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly ArtsService _artService;
        public readonly SoloExhibitionService _soloExhibitionService;
        public UserController(ArtsService artService, SoloExhibitionService soloExhibitionService)
        {
            _artService = artService;
            _soloExhibitionService = soloExhibitionService;
        }
        [HttpGet("Arts")]
        public async Task<IActionResult> GetArts()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            var arts = await _artService.GetSpecificAsync(userId);
            return Ok(arts);
        }

        [HttpGet("Exhibitions")]
        public async Task<IActionResult> GetExhibitions()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            var exhibitions = await _soloExhibitionService.GetSpecificAsync(userId);
            return Ok(exhibitions);
        }

    }
}
