using art_gallery.Controllers;
using art_gallery.Models;
using art_gallery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace art_gallery.test.unit
{
    public class ArtControllerTest
    {
        private readonly ArtsController _artController;
        private readonly ArtsService _artService;
        ArtServiceFake myFakeService = new ArtServiceFake();
        public ArtControllerTest()
        {
            _artController = new ArtsController(myFakeService);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _artController.GetAll();
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            //Act
            var okResult = await _artController.GetAll() as OkObjectResult;

            //Asert
            var items = Assert.IsType<List<Art>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            //Act
            var okResult = await _artController.Get();

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsPublicArts()
        {
            //Act
            var okResult = await _artController.Get() as OkObjectResult;

            //Assert
            var items = Assert.IsType<List<Art>>(okResult.Value);
            Assert.Equal(1, items.Count);
        }

        [Fact]
        public async Task GetSpecific_UnknownIdProvided_ReturnsNotFound()
        {
            //Act
            var notFoundResult = await _artController.Get(Guid.NewGuid().ToString());

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult as NotFoundObjectResult);

        }

        [Fact]
        public async Task GetSpecific_knownIdProvided_ReturnsOkResult()
        {
            //Act
            var okResult = await _artController.Get("c61db880-2c02-4cd4-9dcb-dc118f78d3c8");

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetSpecific_knownIdProvided_ReturnsRightItem()
        {
            var testId = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            //Act
            var okResult = await _artController.Get(testId) as OkObjectResult;

            //Assert
            var item = Assert.IsType<Art>(okResult.Value);
            Assert.Equal(testId, (item as Art).Id);
        }

        [Fact]
        public async Task PostArt_ValidObjectProvided_ReturnsCreatedResponse()
        {
            var passedObject = new Art
            {
                Id = new Guid("b3929f7c-6e96-4f42-a059-92a6b54d3f6e").ToString(),
                Title = "Abstract Harmony",
                Private = true,
                Owner = Guid.NewGuid().ToString(),
                Description = "An abstract composition that explores the harmony of colors and shapes.",
                Artist = "Sophie Anderson",
                Dimensions = "24x36 inches",
                DateOfWork = DateTime.Parse("2023-02-10T12:15:00Z"),
                Style = "Abstract",
                EstimatedValue = 1500.00M,
                Comments = new List<Comment>()
            };
            //Act
            var createdResponse = await _artController.Post(passedObject);

            //Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public async Task PostArt_ValidObjectProvided_ReturnsCreatedItem()
        {
            var passedObject = new Art
            {
                Id = new Guid("b3929f7c-6e96-4f42-a059-92a6b54d3f6e").ToString(),
                Title = "Abstract Harmony",
                Private = true,
                Owner = Guid.NewGuid().ToString(),
                Description = "An abstract composition that explores the harmony of colors and shapes.",
                Artist = "Sophie Anderson",
                Dimensions = "24x36 inches",
                DateOfWork = DateTime.Parse("2023-02-10T12:15:00Z"),
                Style = "Abstract",
                EstimatedValue = 1500.00M,
                Comments = new List<Comment>()
            };

            //Act
            var createdResponse = await _artController.Post(passedObject) as CreatedAtActionResult;
            var item = createdResponse.Value as Art;

            //Assert
            Assert.IsType<Art>(item);
            Assert.Equal(item.Title, passedObject.Title);

        }

        [Fact]
        public async Task UpdateArt_InvalidIdProvided_ReturnsNotFound()
        {
            var id = Guid.NewGuid().ToString();

            var Replacer = new Art
            {
                Id = new Guid("3e91e5e4-2eab-4c2b-9f0a-7d16e36d4c63").ToString(),
                Title = "Enchanted Forest",
                Private = false,
                Owner = Guid.NewGuid().ToString(),
                Description = "A magical depiction of an enchanted forest with vibrant colors and mystical creatures.",
                Artist = "Elena Rodriguez",
                Dimensions = "30x48 inches",
                DateOfWork = DateTime.Parse("2022-11-25T16:30:00Z"),
                Style = "Fantasy",
                EstimatedValue = 1800.00M,
                Comments = new List<Comment>()

            };

            //Act
            var notFoundResult = await _artController.Put(id, Replacer);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task UpdateArt_validIdProvidedButNotTheOwner_ReturnsForbidden()
        {
            var id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";

            var replacer = new Art
            {
                Id = new Guid("3e91e5e4-2eab-4c2b-9f0a-7d16e36d4c63").ToString(),
                Title = "Enchanted Forest",
                Private = false,
                Owner = Guid.NewGuid().ToString(),
                Description = "A magical depiction of an enchanted forest with vibrant colors and mystical creatures.",
                Artist = "Elena Rodriguez",
                Dimensions = "30x48 inches",
                DateOfWork = DateTime.Parse("2022-11-25T16:30:00Z"),
                Style = "Fantasy",
                EstimatedValue = 1800.00M,
                Comments = new List<Comment>()
            };

            //Act
            var foundedArt = await _artController.Put(id, replacer) as ObjectResult;

            //Assert
            Assert.NotNull(foundedArt);
            Assert.Equal(403, foundedArt.StatusCode);
        }
        [Fact]
        public async Task UpdateArt_validIdProvidedAndValidOwneer_ReturnsNoContent()
        {
            var id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var userId = "b381f98c-2f43-4a9e-9e2e-8523b996e0dd";
            var replacer = new Art
            {
                Title = "Enchanted Forest",
                Private = false,
                Description = "A magical depiction of an enchanted forest with vibrant colors and mystical creatures.",
                Artist = "Elena Rodriguez",
                Dimensions = "30x48 inches",
                DateOfWork = DateTime.Parse("2022-11-25T16:30:00Z"),
                Style = "Fantasy",
                EstimatedValue = 1800.00M,
                Comments = new List<Comment>()
            };
            //Act
            var okResult = await _artController.Get(id) as OkObjectResult;
            var item = Assert.IsType<Art>(okResult.Value);
            var owner = (item as Art).Owner;
            // Act
            var foundedArt = await _artController.Put(id, replacer) as NoContentResult;

            // Assert
            if (foundedArt != null && userId == owner)
            {
                Assert.Equal(204, foundedArt.StatusCode);
                Assert.Equal((item as Art).Title, replacer.Title);
            }
            
        }

        [Fact]
        public async Task DeleteArt_InvalidIdProvided_ReturnsNotFound()
        {
            var id = Guid.NewGuid().ToString();

            //Act
            var notFoundResult = await _artController.Delete(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteArt_validIdProvidedButOwnerIsNot_ReturnsForbidden()
        {
            var id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var userId = Guid.NewGuid().ToString();

            //var okResult = await _artController.Get(id) as OkObjectResult;
            //var item = Assert.IsType<Art>(okResult.Value);
            //var owner = (item as Art).Owner;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                    }, "mock"));

            _artController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            //Act
            var deletedArt = await _artController.Delete(id) as ObjectResult;

            
             Assert.Equal(403, deletedArt.StatusCode);

        }

        [Fact]
        public async Task DeleteArt_ValidIdProvidedAndValidOwner_ReturnsNoContent()
        {
            var id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var userId = "b381f98c-2f43-4a9e-9e2e-8523b996e0dd";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
            }, "mock"));

            _artController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var deletedArt = await _artController.Delete(id) as NoContentResult;

            // Assert
            Assert.NotNull(deletedArt);
            Assert.Equal(204, deletedArt.StatusCode);

            var arts = await _artController.Get(id);
            Assert.IsType<NotFoundObjectResult>(arts);

        }


        [Fact]
        public async Task GetComments_ValidArtIdProvided_ReturnsOkResult()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            //Act
            var okResult = await _artController.GetComments(art_id);

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetComments_InValidArtIdProvided_ReturnsNotFound()
        {
            var art_id = Guid.NewGuid().ToString();

            //Act
            var notFoundResult = await _artController.GetComments(art_id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task GetComments_ValidIdProvided_ReturnsTheRightComments()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";

            //Act
            var okResult = await _artController.GetComments(art_id) as OkObjectResult;

            //Assert
            var comments = Assert.IsType<List<Comment>>(okResult.Value);
            Assert.Equal(2, (comments as List<Comment>).Count);
        }

        [Fact]
        public async Task GetComment_InvalidArtIdProvided_ReturnsNotFound()
        {
            var art_id = Guid.NewGuid().ToString();
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";

            //Act
            var notFoundResult = await _artController.GetComment(art_id, comment_id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task GetComment_ValidArtIdProvided_ReturnsOkResult()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";

            //Act
            var okResult = await _artController.GetComment(art_id, comment_id);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task GetComment_ValidArtIdProvided_ReturnsTheRightComment()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";

            //Act
            var okResult = await _artController.GetComment(art_id, comment_id) as OkObjectResult;

            //Assert
            var founded_comment = Assert.IsType<Comment>(okResult.Value as Comment);
            Assert.Equal(comment_id, founded_comment.Id);
        }

        [Fact]
        public async Task PostComment_InValidArtIdProvided_ReturnsNotFound()
        {
            var art_id = Guid.NewGuid().ToString();
            var another_comment = new Comment {
                Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(),
                UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(),
                Content = "I appreciate the insight.",
                Timestamp = DateTime.UtcNow
            };
            //Act
            var notFoundResult = await _artController.PostComment(art_id, another_comment);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task PostComment_ValidArtIdProvided_ReturnsOkResult()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var another_comment = new Comment
            {
                Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(),
                UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(),
                Content = "I appreciate the insight.",
                Timestamp = DateTime.UtcNow
            };
            //Act
            var okResult = await _artController.PostComment(art_id, another_comment);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public async Task PostComment_ValidArtIdProvided_AddCommentToComments()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var another_comment = new Comment
            {
                Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(),
                UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(),
                Content = "I appreciate the insight.",
                Timestamp = DateTime.UtcNow
            };
            //Act
            var okResult = await _artController.PostComment(art_id, another_comment) as OkObjectResult;

            //Assert
            var comments = Assert.IsType<List<Comment>>(okResult.Value);
            Assert.Equal(3, comments.Count);
            
        }

        [Fact]
        public async Task DeleteComment_InvalidArtIdProvided_ReturnsNotFound()
        {
            var art_id = Guid.NewGuid().ToString();
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";

            //Act
            var notFoundResult = await _artController.DeleteComment(art_id, comment_id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteComment_ValidArtIdButInvalidCommentId_ReturnsNotFound()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var comment_id = Guid.NewGuid().ToString();

            //Act
            var notFoundResult = await _artController.DeleteComment(art_id, comment_id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteComment_ValidArtIdAndValidCommentIdButNotTheOwner_ReturnsForbidden()
        {
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
                // Add other claims as needed
            }, "mock"));

            // Set the user for the controller context
            _artController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            //Act
            var forbiddenResult = await _artController.DeleteComment(art_id, comment_id) as ObjectResult;

            //Assert

            Assert.Equal(403, forbiddenResult.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_ValidArtIdAndValidCommentIdAndValidOwner_ReturnsOk()
        {
            // Arrange
            var art_id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8";
            var comment_id = "60d5b4a6-6d4d-89d2-ef8c-d6d100000001";
            var userId = "70d5b4a6-6d4d-89d2-ef8c-d6d100000002"; // Use the correct user ID

            // Simulate an authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
                // Add other claims as needed
            }, "mock"));

            // Set the user for the controller context
            _artController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var okResult = await _artController.DeleteComment(art_id, comment_id) as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Comment deleted successfully", okResult?.Value);
        }




    }
}
