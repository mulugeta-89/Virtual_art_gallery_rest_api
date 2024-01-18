using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace art_gallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoloExhibitionController : ControllerBase
    {
        private readonly SoloExhibitionService _soloExhibitionService;

        public SoloExhibitionController(SoloExhibitionService soloExhibitionService)
        {
            _soloExhibitionService = soloExhibitionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<SoloExhibition>>> Get()
        {
            var soloExhibitions = await _soloExhibitionService.GetAllAsync();
            return Ok(soloExhibitions);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SoloExhibition>> GetById(string id)
        {
            var soloExhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (soloExhibition == null)
            {
                return NotFound();
            }
            return Ok(soloExhibition);
        }


        [HttpPost]
        public async Task<ActionResult> Create(SoloExhibition soloExhibition)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            soloExhibition.Curator = userId;
            await _soloExhibitionService.CreateAsync(soloExhibition);

            return CreatedAtAction("Get", soloExhibition.Id, soloExhibition);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, SoloExhibition soloExhibition)
        {
            var existingExhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (existingExhibition == null)
            {
                return NotFound();
            }

            await _soloExhibitionService.UpdateAsync(id, soloExhibition);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var soloExhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (soloExhibition == null)
            {
                return NotFound();
            }

            await _soloExhibitionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
