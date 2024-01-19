using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace art_gallery.Models
{
    // Represents a comment made by a user for a piece of art.
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Content { get; set; }
        public DateTime? Timestamp { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
    }
}
