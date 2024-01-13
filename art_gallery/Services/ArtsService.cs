using art_gallery.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace art_gallery.Services
{
    public class ArtsService
    {
        private readonly IMongoCollection<Art> _artsCollection;

        public ArtsService(
            IOptions<ArtGalleryDatabaseSettings> ArtGalleryDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ArtGalleryDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ArtGalleryDatabaseSettings.Value.DatabaseName);

            _artsCollection = mongoDatabase.GetCollection<Art>(
                ArtGalleryDatabaseSettings.Value.ArtsCollectionName);
        }

        public async Task<List<Art>> GetAsync() =>
            await _artsCollection.Find(_ => true).ToListAsync();

        public async Task<Art?> GetAsync(string id) =>
            await _artsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Art newArt) =>
            await _artsCollection.InsertOneAsync(newArt);

        public async Task UpdateAsync(string id, Art updatedArt) =>
            await _artsCollection.ReplaceOneAsync(x => x.Id == id, updatedArt);

        public async Task RemoveAsync(string id) =>
            await _artsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
