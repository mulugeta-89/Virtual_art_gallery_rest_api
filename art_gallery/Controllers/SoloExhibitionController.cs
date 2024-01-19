using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Authorization;
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
                return NotFound("Exhibition Not Found");
            }
            return Ok(soloExhibition);
        }

        [Authorize(Roles = "ARTIST")]
        [HttpPost]
        public async Task<ActionResult> Create(SoloExhibition soloExhibition)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            soloExhibition.Curator = userId;
            await _soloExhibitionService.CreateAsync(soloExhibition);

            return CreatedAtAction("Get", soloExhibition.Id, soloExhibition);
        }

        [Authorize(Roles = "ARTIST")]
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

        [Authorize(Roles = "ARTIST")]
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

        [Authorize]
        [HttpGet("{id}/Comments")]
        public async Task<IActionResult> GetComments(string id)
        {
            var exhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (exhibition == null)
            {
                return NotFound("exhibition not found");
            }
            return Ok(exhibition.Comments);
        }

        [Authorize]
        [HttpGet("{id}/Comments/{commentId}")]
        public async Task<IActionResult> GetComment(string id, string commentId)
        {
            var exhibition = await _soloExhibitionService.GetByIdAsync(id);
            var exhibitionComments = exhibition.Comments;
            if (exhibition == null || exhibitionComments == null)
            {
                return NotFound("exhibition not found");
            }
            var comment = exhibitionComments.FirstOrDefault(y => y.Id == commentId);
            return Ok(comment);
        }

        [Authorize]
        [HttpPost("{id}/Comments")]
        public async Task<IActionResult> PostComment(string id, Comment comment)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var exhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (exhibition == null)
            {
                return NotFound("exhibition not found");
            }
            var newComment = new Comment
            {
                Content = comment.Content,
                Timestamp = comment.Timestamp,
                UserId = userId
            };

            exhibition.Comments.Add(newComment);
            await _soloExhibitionService.UpdateAsync(id, exhibition);
            return Ok(exhibition.Comments);
        }

        [Authorize]
        [HttpDelete("{id}/Comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(string id, string commentId)
        {
            var exhibition = await _soloExhibitionService.GetByIdAsync(id);
            if (exhibition == null)
            {
                return NotFound("exhibition not found");
            }
            var exhibitionComments = exhibition.Comments;
            var comment = exhibitionComments.FirstOrDefault(x => x.Id == commentId);
            if (comment == null)
            {
                return NotFound("comment not found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (comment.UserId != userId)
            {
                return new ObjectResult("You are not the owner of the comment.")
                {
                    StatusCode = 403 // Forbidden
                };
            }
            exhibition.Comments.Remove(comment);
            await _soloExhibitionService.UpdateAsync(id, exhibition);
            return Ok(exhibition.Comments);
        }
    }
}
