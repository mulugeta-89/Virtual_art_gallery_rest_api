using System.Security.Claims;
using Amazon.Runtime.Internal;
using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var arts = await _artService.GetAsync();
            return Ok(arts);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var arts = await _artService.GetPublicAsync();
            return Ok(arts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var art = await _artService.GetAsync(id);
            if (art == null)
            {
                return NotFound("art not found");
            }
            return Ok(art);
        }

        [Authorize(Roles = "ARTIST")]
        [HttpPost]
        public async Task<IActionResult> Post(Art art)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            art.Owner = userId;
            await _artService.CreateAsync(art);
            return CreatedAtAction("Get", art.Id, art);
        }

        [Authorize(Roles = "ARTIST")]
        [HttpPost("upload-image/{artId}")]
        public async Task<IActionResult> UploadImage(string artId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var art = await _artService.GetAsync(artId);

            if (art == null)
                return NotFound("Art not found.");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                art.ImageData = memoryStream.ToArray();
            }

            await _artService.UpdateAsync(artId, art);

            return Ok(new { message = "Image uploaded successfully." });
        }

        [Authorize(Roles = "ARTIST")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Art art)
        {
            var arti = await _artService.GetAsync(id);

            if (arti == null)
            {
                return NotFound("Art not found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (arti.Owner != userId)
            {
                return new ObjectResult("You are not the owner of this art.")
                {
                    StatusCode = 403 // Forbidden
                };
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

        [Authorize(Roles = "ARTIST")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var arti = await _artService.GetAsync(id);

            if (arti == null)
            {
                return NotFound("art not found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (arti.Owner != userId)
            {
                return new ObjectResult("You are not the owner of this art.")
                {
                    StatusCode = 403 // Forbidden
                };
            }
            await _artService.RemoveAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpGet("{id}/Comments")]
        public async Task<IActionResult> GetComments(string id)
        {
            var art = await _artService.GetAsync(id);
            if (art == null)
            {
                return NotFound("art not found");
            }
            return Ok(art.Comments);
        }

        [Authorize]
        [HttpGet("{id}/Comments/{commentId}")]
        public async Task<IActionResult> GetComment(string id, string commentId)
        {
            var art = await _artService.GetAsync(id);
            var artComments = art.Comments;
            if (art == null || artComments == null)
            {
                return NotFound("art not found");
            }
            var comment = artComments.FirstOrDefault(y => y.Id == commentId);
            return Ok(comment);
        }

        [Authorize]
        [HttpPost("{id}/Comments")]
        public async Task<IActionResult> PostComment(string id, Comment comment)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var art = await _artService.GetAsync(id);
            if (art == null)
            {
                return NotFound("art not found");
            }
            var newComment = new Comment
            {
                Content = comment.Content,
                Timestamp = comment.Timestamp,
                UserId = userId
            };

            art.Comments.Add(newComment);
            await _artService.UpdateAsync(id, art);
            return Ok(art.Comments);
        }

        [Authorize]
        [HttpDelete("{id}/Comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(string id, string commentId)
        {
            var art = await _artService.GetAsync(id);
            if (art == null)
            {
                return NotFound("art not found");
            }
            var artComments = art.Comments;
            var comment = artComments.FirstOrDefault(x => x.Id == commentId);
            if (comment == null)
            {
                return NotFound("comment not found");
            }
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (art.Owner != userId)
            {
                return new ObjectResult("You are not the owner of this art.")
                {
                    StatusCode = 403 // Forbiddenn
                };
            }
            if (comment.UserId != userId)
            {
                return new ObjectResult("You are not the ownder of the comment.")
                {
                    StatusCode = 403 // Forbidden
                };
            }
            art.Comments.Remove(comment);
            await _artService.UpdateAsync(id, art);
            return Ok(art.Comments);
        }
    }
}
