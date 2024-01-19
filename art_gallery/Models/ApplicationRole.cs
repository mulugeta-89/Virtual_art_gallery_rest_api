using System.Runtime.Serialization;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace art_gallery.Models
{
    [CollectionName("roles")]
    public class ApplicationRole : MongoIdentityRole<Guid> { }
}
