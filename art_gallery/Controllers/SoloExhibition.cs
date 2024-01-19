using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace art_gallery.Controllers
{
    [Authorize]
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
                return NotFound("Exhibition Not Found");
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
                return NotFound("Specified Exhibition not found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (soloExhibition.Curator != userId)
            {
                return new ObjectResult("You are not the curator this Exhibition.")
                {
                    StatusCode = 403 // Forbidden
                };
            }
            existingExhibition.Id = soloExhibition.Id ?? existingExhibition.Id;
            existingExhibition.StartDate = soloExhibition.StartDate;
            existingExhibition.EndDate = soloExhibition.EndDate;
            existingExhibition.Description = soloExhibition.Description;
            existingExhibition.Title = soloExhibition.Title;
            existingExhibition.ArtworkIds = soloExhibition.ArtworkIds;

            await _soloExhibitionService.UpdateAsync(id, existingExhibition);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var soloExhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (soloExhibition == null)
            {
                return NotFound("Exhibition Not Found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (soloExhibition.Curator != userId)
            {
                return new ObjectResult("You are not the curator of this Exhibition.")
                {
                    StatusCode = 403 // Forbidden
                };
            }

            await _soloExhibitionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
