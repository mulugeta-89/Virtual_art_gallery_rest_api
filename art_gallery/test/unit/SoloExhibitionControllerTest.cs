using art_gallery.Controllers;
using art_gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace art_gallery.test.unit
{
    public class SoloExhibitionControllerTest
    {
        private readonly SoloExhibitionController _soloExhibitionController;
        SoloExhibitionServiceFake _soloExhibitionServce = new SoloExhibitionServiceFake();
        public SoloExhibitionControllerTest() {
            _soloExhibitionController = new SoloExhibitionController(_soloExhibitionServce);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnOkResult()
        {
            //Act
            var okResult = await _soloExhibitionController.Get();

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);

        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllExhibitions()
        {
            //Act
            var okResult = await _soloExhibitionController.Get() as OkObjectResult;

            //Assert
            var exhibitions = Assert.IsType<List<SoloExhibition>>(okResult.Value);
            Assert.Equal(3, exhibitions.Count);
        }

        [Fact]
        public async Task GetByid_InvalidExhibitionIdProvided_ReturnsNotFound()
        {
            //var exhibition_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var exhibition_id = Guid.NewGuid().ToString();
            //Act
            var notFoundResult = await _soloExhibitionController.GetById(exhibition_id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task GetByid_validExhibitionIdProvided_ReturnsOk()
        {
            var exhibition_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            //Act
            var okResult = await _soloExhibitionController.GetById(exhibition_id) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task GetByid_validExhibitionIdProvided_ReturnsRightExhibition()
        {
            var exhibition_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            //Act
            var okResult = await _soloExhibitionController.GetById(exhibition_id) as OkObjectResult;

            //Assert
            var exhibition = Assert.IsType<SoloExhibition>(okResult.Value);
            Assert.Equal(exhibition_id, exhibition.Id);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnCreatedAtAction()
        {
            var tobecreated = new SoloExhibition
            {
                Id = "a87b1a6d-4ef0-4c2b-8e35-2fd356c27a1a",
                Title = "Modern Abstracts: Exploring Shapes and Colors",
                Description = "Experience the avant-garde world of modern abstracts, pushing the boundaries of shapes and colors.",
                StartDate = DateTime.Parse("2024-03-01T12:00:00"),
                EndDate = DateTime.Parse("2024-03-10T18:00:00"),
                Curator = "f86c25cf-9e71-48e2-9a3c-4f5e1582b37d",
                ArtworkIds = new List<string>
            {
                "45d8123a-91a9-4e8e-8a1c-6e3a7a9f4c5f",
                "9e5f6a37-23c1-4e5f-a4c1-0f0d6542d7f1",
                "7b3c82f1-d309-434b-862e-8d43e3e98710"
            },
                Comments = new List<Comment>
            {
                new Comment
                {
                    Id = "3b9c7d32-77d5-4dcd-946b-7a2a1edc89f0",
                    UserId = "user123",
                    Content = "Absolutely stunning collection!",
                    Timestamp = DateTime.Parse("2024-03-05T15:30:00")
                },
                new Comment
                {
                    Id = "f7f9d2a2-0a2b-4f97-a789-d1f256e32d9e",
                    UserId = "artFan456",
                    Content = "The use of colors is mesmerizing.",
                    Timestamp = DateTime.Parse("2024-03-06T10:45:00")
                }
            }
            };
            //Act
            var createdResult = await _soloExhibitionController.Create(tobecreated);

            //Assert
            Assert.IsType<CreatedAtActionResult>(createdResult);
        }

        [Fact]
        public async Task Create_WhenCalled_CreatesAnObject()
        {
            var tobecreated = new SoloExhibition
            {
                Id = "a87b1a6d-4ef0-4c2b-8e35-2fd356c27a1a",
                Title = "Modern Abstracts: Exploring Shapes and Colors",
                Description = "Experience the avant-garde world of modern abstracts, pushing the boundaries of shapes and colors.",
                StartDate = DateTime.Parse("2024-03-01T12:00:00"),
                EndDate = DateTime.Parse("2024-03-10T18:00:00"),
                Curator = "f86c25cf-9e71-48e2-9a3c-4f5e1582b37d",
                ArtworkIds = new List<string>
            {
                "45d8123a-91a9-4e8e-8a1c-6e3a7a9f4c5f",
                "9e5f6a37-23c1-4e5f-a4c1-0f0d6542d7f1",
                "7b3c82f1-d309-434b-862e-8d43e3e98710"
            },
                Comments = new List<Comment>
            {
                new Comment
                {
                    Id = "3b9c7d32-77d5-4dcd-946b-7a2a1edc89f0",
                    UserId = "user123",
                    Content = "Absolutely stunning collection!",
                    Timestamp = DateTime.Parse("2024-03-05T15:30:00")
                },
                new Comment
                {
                    Id = "f7f9d2a2-0a2b-4f97-a789-d1f256e32d9e",
                    UserId = "artFan456",
                    Content = "The use of colors is mesmerizing.",
                    Timestamp = DateTime.Parse("2024-03-06T10:45:00")
                }
            }
            };
            //Act
            var createdResult = await _soloExhibitionController.Create(tobecreated) as CreatedAtActionResult;
            var created_exhibition = createdResult.Value as SoloExhibition;

            //Assert
            Assert.IsType<SoloExhibition>(created_exhibition);
            Assert.Equal(tobecreated.Title, created_exhibition.Title);
        }
    }
}
