using art_gallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace art_gallery.Services
{
    public class IdentityService
    {

        public IdentityService(
            IOptions<ArtGalleryDatabaseSettings> ArtGalleryDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ArtGalleryDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ArtGalleryDatabaseSettings.Value.DatabaseName);
        }
    }
}

