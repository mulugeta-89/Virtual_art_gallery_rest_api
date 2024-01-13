using art_gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        public static List<Art> artPieces = new List<Art>()
         {
            new Art()
            {
            Id = 1,
            Title = "Starry Night",
            Description = "A famous night sky painting by Vincent van Gogh",
            Artist = "Vincent van Gogh",
            Date_Of_Work = new DateTime(1889, 6, 18),
            Style = "Post-Impressionism",
            Dimensions = new double[] { 73.7, 92.1 }, // Height x Width (for simplicity, assuming 2D)
            EstimatedValue = 10000000.00m
            },
            new Art()
            {
                Id = 2,
                Title = "The Persistence of Memory",
                Description = "A surrealist painting by Salvador Dali featuring melting clocks",
                Artist = "Salvador Dali",
                Date_Of_Work = new DateTime(1931, 1, 1),
                Style = "Surrealism",
                Dimensions = new double[] { 9.5, 13.0 }, // Height x Width (for simplicity, assuming 2D)
                EstimatedValue = 8000000.00m
            },
            new Art()
            {
                Id = 3,
                Title = "David",
                Description = "A renowned marble statue by Michelangelo",
                Artist = "Michelangelo",
                Date_Of_Work = new DateTime(1504, 1, 1),
                Style = "Renaissance",
                Dimensions = new double[] { 170.0, 54.0, 52.0 }, // Height x Width x Depth
                EstimatedValue = 150000000.00m
            },
            new Art()
            {
                Id = 4,
                Title = "Guernica",
                Description = "A powerful anti-war mural by Pablo Picasso",
                Artist = "Pablo Picasso",
                Date_Of_Work = new DateTime(1937, 1, 1),
                Style = "Cubism",
                Dimensions = new double[] { 349.0, 776.0 }, // Height x Width (for simplicity, assuming 2D)
                EstimatedValue = 200000000.00m
            }
          };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(artPieces);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var art = artPieces.FirstOrDefault(x => x.Id == id);
            if (art == null)
            {
                return BadRequest("Art Not found");
            }
            return Ok(art);
        }

        [HttpPost]
        public IActionResult Post(Art art)
        {
            artPieces.Add(art);
            return CreatedAtAction("Get", art.Id, art);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Art art)
        {
            var arti = artPieces.FirstOrDefault(x => x.Id == id);

            if (arti == null)
            {
                return NotFound("Art not found");
            }
            arti.Id = art.Id;
            arti.Title = art.Title;
            arti.Description = art.Description;
            arti.Artist = art.Artist;
            arti.Dimensions = art.Dimensions;
            arti.Date_Of_Work = art.Date_Of_Work;
            arti.EstimatedValue = art.EstimatedValue;
            arti.Style = art.Style;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) { 
            var arti = artPieces.FirstOrDefault(x => x.Id == id);

            if (arti == null)
            {
                return NotFound("art not found");
            }
            artPieces.Remove(arti);
            return NoContent();
        }

    }
}
