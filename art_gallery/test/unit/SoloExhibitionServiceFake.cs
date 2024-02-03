using art_gallery.Interfaces;
using art_gallery.Models;

namespace art_gallery.test.unit
{
    public class SoloExhibitionServiceFake : ISoloExhibitionService
    {
        private readonly List<SoloExhibition> _exhibitions;
        public SoloExhibitionServiceFake()
        {
            _exhibitions = new List<SoloExhibition>()
            {
                new SoloExhibition
                {
                    Id = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8",
                    Title = "Impressionist Masterpieces: A Journey Through Light and Color",
                    Description = "Explore the enchanting world of Impressionist art in this mesmerizing exhibition.",
                    StartDate = DateTime.Now.AddDays(7),
                    EndDate = DateTime.Now.AddDays(14),
                    Curator = "3c9512c3-6fbd-4485-b58b-09b75dcb8be2",
                    ArtworkIds = new List<string> { "60d5b4a6-6d4d-89d2-ef8c-d6d100000001", "60d5b4a6-6d4d-89d2-ef8c-d6d100000002" },
                    Comments = new List<Comment>
                        {
                            new Comment { Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000001").ToString(), UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000002").ToString(), Content = "This is thought-provoking.", Timestamp = DateTime.UtcNow },
                            new Comment { Id = new Guid("80d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(), UserId = new Guid("90d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(), Content = "I see the beauty in chaos!", Timestamp = DateTime.UtcNow }
                        }
                },
                new SoloExhibition
                {
                    Id = "1c5e5a22-6b3f-4f3e-b9b7-5a5ae309ad4a",
                    Title = "Modern Abstract Expressions: A Fusion of Colors and Emotions",
                    Description = "Dive into the world of modern abstract art, where colors and emotions collide.",
                    StartDate = DateTime.Now.AddDays(14),
                    EndDate = DateTime.Now.AddDays(21),
                    Curator = "1c5e5a22-6b3f-4f3e-b9b7-5a5ae309ad4a",
                    ArtworkIds = new List<string> { "8d8be88a-9fc1-4b7c-bf5e-512f62d98be1", "e6b6c61c-c22d-461c-9ba1-1d84f19a2582" },
                    Comments = new List<Comment>
                        {
                            new Comment { Id = new Guid("60d5b4a6-6d4d-89d2-ef8c-d6d100000001").ToString(), UserId = new Guid("70d5b4a6-6d4d-89d2-ef8c-d6d100000002").ToString(), Content = "This is amazing!", Timestamp = DateTime.UtcNow },
                            new Comment { Id = new Guid("80d5b4a6-6d4d-89d2-ef8c-d6d100000003").ToString(), UserId = new Guid("90d5b4a6-6d4d-89d2-ef8c-d6d100000004").ToString(), Content = "I love the colors!", Timestamp = DateTime.UtcNow }
                        }
                },
                new SoloExhibition
                {
                    Id = "3c9512c3-6fbd-4485-b58b-09b75dcb8be2",
                    Title = "Classic Portraits: Timeless Elegance and Character",
                    Description = "Embark on a journey through time with classic portraits showcasing timeless elegance and character.",
                    StartDate = DateTime.Now.AddDays(21),
                    EndDate = DateTime.Now.AddDays(28),
                    Curator = "c61db880-2c02-4cd4-9dcb-dc118f78d3c8",
                    ArtworkIds = new List<string> { "23bc7de7-7d8d-4c82-9ab3-12797a15c03e", "1f4e699b-7f35-4c82-99d5-98653b92c281" },
                    Comments = new List<Comment>()
                    
                }
            };
        }
        public Task<List<SoloExhibition>> GetAllAsync()
        {
            return Task.FromResult(_exhibitions);
        }

        public Task<SoloExhibition> GetByIdAsync(string id)
        {
            var exhibition = _exhibitions.FirstOrDefault(x => x.Id == id);
            return Task.FromResult<SoloExhibition>(exhibition);
        }
        public Task<List<SoloExhibition>> GetSpecificAsync(string ownerId)
        {
            var exhibitions = _exhibitions.FindAll(x => x.Curator == ownerId);
            return Task.FromResult(exhibitions);
        }
        public Task CreateAsync(SoloExhibition soloExhibition)
        {
            soloExhibition.Id = Guid.NewGuid().ToString();
            soloExhibition.Curator = Guid.NewGuid().ToString();

            _exhibitions.Add(soloExhibition);
            return Task.FromResult(soloExhibition);
        }
        public Task UpdateAsync(string id, SoloExhibition willReplace)
        {
            var updatedExhibition = _exhibitions.FirstOrDefault(x => x.Id == id);
            if (updatedExhibition != null)
            {
                // Preserve the existing Id and Owner
                var originalId = updatedExhibition.Id;
                var originalOwner = updatedExhibition.Curator;

                // Copy properties from willReplace to updatedExhibition
                updatedExhibition.Id = originalId;
                updatedExhibition.Curator = originalOwner;
                updatedExhibition.Title = willReplace.Title;
                updatedExhibition.Description = willReplace.Description;
                updatedExhibition.StartDate = willReplace.StartDate;
                updatedExhibition.EndDate = willReplace.EndDate;
                updatedExhibition.ArtworkIds = willReplace.ArtworkIds;
                updatedExhibition.Comments = willReplace.Comments;

                return Task.FromResult(updatedExhibition);
            }

            // Return a completed task with a null result
            return Task.FromResult<SoloExhibition>(null);
        }
        public Task DeleteAsync(string Id)
        {
            var existing = _exhibitions.First(x => x.Id == Id);
            _exhibitions.Remove(existing);
            return Task.FromResult(existing);
        }
    }
}
