using art_gallery.Interfaces;
using art_gallery.Models;

namespace art_gallery.test.unit
{
    public class ArtServiceFake : IArtsService
    {
        private readonly List<Art> _arts;
        private readonly string userId;
        public ArtServiceFake()
        {
            userId = "b381f98c-2f43-4a9e-9e2e-8523b996e0dd";
            _arts = new List<Art>()
            {
                new Art
                {
                    //Id = new Guid("7aebd5d3-6c6a-4c0c-b1b8-9cdef2a1f345").ToString(),
                    Id = "1",
                    Title = "Mystical Landscape",
                    Private = true,
                    Owner = "c0c5d97c-10f4-4a92-a101-d3b05e1d9e5d",
                    Description = "A breathtaking view of a mystical landscape.",
                    Artist = "Jane Doe",
                    Dimensions = "24x36 inches",
                    //ImageData = Convert.FromBase64String("base64_encoded_image_data"),
                    DateOfWork = DateTime.Parse("2022-01-15T10:30:00Z"),
                    Style = "Abstract",
                    EstimatedValue = 1500.00M,
                    Comments = new List<Comment>
                    {
                            new Comment { Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000001").ToString(), UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000002").ToString(), Content = "This is amazing!", Timestamp = DateTime.UtcNow },
                            new Comment { Id = new Guid("80d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(), UserId = new Guid("90d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(), Content = "I love the colors!", Timestamp = DateTime.UtcNow }
                    }

        },
                new Art
                {
                    //Id = new Guid("f8e902b1-8159-4c89-8f9a-3171b22e7bd2").ToString(),
                    Id = "2",
                    Title = "Sunset Serenity",
                    Private = true,
                    Owner = "e4a81354-13c5-45a8-a173-8d7b70b1cd11",
                    Description = "A serene depiction of a sunset over the ocean.",
                    Artist = "John Smith",
                    Dimensions = "18x24 inches",
                    DateOfWork = DateTime.Parse("2021-09-08T15:45:00Z"),
                    Style = "Realism",
                    EstimatedValue = 1200.00M,
                    Comments = new List<Comment>()
                },
                new Art
                {
                    Id = new Guid("c61db880-2c02-4cd4-9dcb-dc118f78d3c8").ToString(),
                    //Id = "3",
                    Title = "Urban Chaos",
                    Private = false,
                    Owner = "b381f98c-2f43-4a9e-9e2e-8523b996e0dd",
                    Description = "A representation of urban chaos and complexity.",
                    Artist = "Michael Johnson",
                    Dimensions = "30x48 inches",
                    //ImageData = Convert.FromBase64String("base64_encoded_image_data"),
                    DateOfWork = DateTime.Parse("2023-03-20T18:00:00Z"),
                    Style = "Modern Art",
                    EstimatedValue = 1800.00M,
                    Comments = new List<Comment>
                    {
                           new Comment { Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000001").ToString(), UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000002").ToString(), Content = "This is thought-provoking.", Timestamp = DateTime.UtcNow },
                           new Comment { Id = new Guid("80d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(), UserId = new Guid("90d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(), Content = "I see the beauty in chaos!", Timestamp = DateTime.UtcNow }
                    }

        }
    };
        }

        public Task<List<Art>> GetAsync()
        {
            return Task.FromResult(_arts);
        }
        public Task<List<Art>> GetPublicAsync()
        {
            var public_arts = _arts.FindAll(x => x.Private == false);
            return Task.FromResult(public_arts);
        }

        public Task<Art> GetAsync(string id)
        {
            var art = _arts.FirstOrDefault(x => x.Id == id && x.Private == false);
            return Task.FromResult<Art>(art);
        }
        public Task<List<Art>> GetSpecificAsync(string id)
        {
            var arts = _arts.FindAll(x => x.Owner == id);
            return Task.FromResult(arts);
        }
        public Task CreateAsync(Art art)
        {
            art.Id = new Guid().ToString();
            art.Owner = new Guid().ToString();
            _arts.Add(art);
            return Task.FromResult<Art>(art);
        }

        public Task UpdateAsync(string id, Art willReplace)
        {
            var updatedArt = _arts.FirstOrDefault(x => x.Id == id);
            if (updatedArt != null)
            {
                // Preserve the existing Id and Owner
                var originalId = updatedArt.Id;
                var originalOwner = updatedArt.Owner;

                // Copy properties from willReplace to updatedArt
                updatedArt.Id = originalId;
                updatedArt.Owner = originalOwner;
                updatedArt.Title = willReplace.Title;
                updatedArt.Private = willReplace.Private;
                updatedArt.Description = willReplace.Description;
                updatedArt.Artist = willReplace.Artist;
                updatedArt.Dimensions = willReplace.Dimensions;
                updatedArt.DateOfWork = willReplace.DateOfWork;
                updatedArt.Style = willReplace.Style;
                updatedArt.EstimatedValue = willReplace.EstimatedValue;
                updatedArt.Comments = willReplace.Comments;

                return Task.FromResult(updatedArt);
            }

            // Return a completed task with a null result
            return Task.FromResult<Art>(null);
        }

        public Task RemoveAsync(string Id)
        {
            var existing = _arts.First(x => x.Id == Id);
            _arts.Remove(existing);
            return Task.FromResult(existing);
        }


    }
}

