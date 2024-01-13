using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        public readonly ArtsService _artService;
        public ArtsController(ArtsService artService)
        {
            _artService = artService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var arts = await _artService.GetAsync();
            return Ok(arts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            var art = await _artService.GetAsync(id);
            if (art == null)
            {
                return NotFound("art not found");
            }
            return Ok(art);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Art art)
        {
            await _artService.CreateAsync(art);
            return CreatedAtAction("Get", art.Id, art);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Art art)
        {
            var arti = await _artService.GetAsync(id);

            if (arti == null)
            {
                return NotFound("Art not found");
            }
            arti.Id = art.Id ?? arti.Id;
            arti.Title = art.Title;
            arti.Description = art.Description;
            arti.Artist = art.Artist;
            arti.Dimensions = art.Dimensions;
            arti.DateOfWork = art.DateOfWork;
            arti.EstimatedValue = art.EstimatedValue;
            arti.Style = art.Style;

            await _artService.UpdateAsync(id, arti);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) { 
            var arti = await _artService.GetAsync(id);

            if (arti == null)
            {
                return NotFound("art not found");
            }
            await _artService.RemoveAsync(id);
            return NoContent();
        }

    }
}
