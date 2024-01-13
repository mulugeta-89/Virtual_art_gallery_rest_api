using art_gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        public static List<Exhibition> exhibitions = new List<Exhibition>()
    {
        new Exhibition()
        {
            Id = 1,
            Title = "Modern Art Showcase",
            Description = "A collection of contemporary artworks",
            StartDate = new DateTime(2022, 1, 15),
            EndDate = new DateTime(2022, 2, 15),
            Artworks = new List<Art>
            {
                new Art()
                {
                    Id = 1,
                    Title = "Abstract Harmony",
                    Description = "Vibrant abstract painting",
                    Artist = "Jane Doe",
                    Date_Of_Work = new DateTime(2021, 12, 1),
                    Style = "Abstract",
                    Dimensions = new double[] { 36.0, 48.0 },
                    EstimatedValue = 5000.00m
                },
                new Art()
                {
                    Id = 2,
                    Title = "Sculpted Elegance",
                    Description = "Graceful sculpture in marble",
                    Artist = "John Smith",
                    Date_Of_Work = new DateTime(2021, 11, 15),
                    Style = "Sculpture",
                    Dimensions = new double[] { 24.0, 12.0, 8.0 },
                    EstimatedValue = 8000.00m
                }
            }
        },
        new Exhibition ()
        {
            Id = 2,
            Title = "Impressionist Masterpieces",
            Description = "A showcase of iconic impressionist paintings",
            StartDate = new DateTime(2022, 3, 1),
            EndDate = new DateTime(2022, 4, 1),
            Artworks = new List<Art>
            {
                new Art()
                {
                    Id = 3,
                    Title = "Water Lilies",
                    Description = "Claude Monet's serene water lilies",
                    Artist = "Claude Monet",
                    Date_Of_Work = new DateTime(1916, 1, 1),
                    Style = "Impressionism",
                    Dimensions = new double[] { 40.0, 30.0 },
                    EstimatedValue = 12000000.00m
                },
                new Art()
                {
                    Id = 4,
                    Title = "Starry Night Over the Rhône",
                    Description = "Vincent van Gogh's night sky masterpiece",
                    Artist = "Vincent van Gogh",
                    Date_Of_Work = new DateTime(1888, 9, 16),
                    Style = "Post-Impressionism",
                    Dimensions = new double[] { 28.0, 36.0 },
                    EstimatedValue = 15000000.00m
                }
            }
        },
    };
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(exhibitions);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var exhibition = exhibitions.FirstOrDefault(x => x.Id == id);
            if (exhibition == null)
            {
                return NotFound("Exhibition not found");
            }
            return Ok(exhibition);
        }


        [HttpPost]
        public IActionResult Post(Exhibition exhibition)
        {
            exhibitions.Add(exhibition);
            return CreatedAtAction("Get", exhibition.Id, exhibition);
        }

        [HttpPut("exhibitions/{id}")]
        public IActionResult Put(int id, Exhibition updatedExhibition)
        {
            var existingExhibition = exhibitions.FirstOrDefault(x => x.Id == id);
            if (existingExhibition == null)
            {
                return NotFound("Exhibition not found");
            }
            existingExhibition.Title = updatedExhibition.Title;
            existingExhibition.StartDate = updatedExhibition.StartDate;
            existingExhibition.EndDate = updatedExhibition.EndDate;
            existingExhibition.Description = updatedExhibition.Description;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exhibition = exhibitions.FirstOrDefault(x => x.Id == id);

            if(exhibition == null)
            {
                return NotFound("Exhibition not found");
            }

            exhibitions.Remove(exhibition);
            return NoContent();

        }

    }
}
