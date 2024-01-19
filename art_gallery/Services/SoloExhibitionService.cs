using art_gallery.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace art_gallery.Services
{
    public class SoloExhibitionService
    {
        private readonly IMongoCollection<SoloExhibition> _exhibitionsCollection;

        public SoloExhibitionService(IOptions<ArtGalleryDatabaseSettings> ArtGalleryDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ArtGalleryDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ArtGalleryDatabaseSettings.Value.DatabaseName);

            _exhibitionsCollection = mongoDatabase.GetCollection<SoloExhibition>(
                ArtGalleryDatabaseSettings.Value.ExhibitionsCollectionName);
        }

        public async Task<List<SoloExhibition>> GetAllAsync()
        {
            return await _exhibitionsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<SoloExhibition> GetByIdAsync(string id)
        {
            return await _exhibitionsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SoloExhibition>> GetSpecificAsync(string ownerId) =>
            await _exhibitionsCollection.Find(x => x.Curator == ownerId).ToListAsync();

        public async Task CreateAsync(SoloExhibition soloExhibition)
        {
            await _exhibitionsCollection.InsertOneAsync(soloExhibition);
        }

        public async Task UpdateAsync(string id, SoloExhibition soloExhibition)
        {
            await _exhibitionsCollection.ReplaceOneAsync(s => s.Id == id, soloExhibition);
        }

        public async Task DeleteAsync(string id)
        {
            await _exhibitionsCollection.DeleteOneAsync(s => s.Id == id);
        }
    }
}
