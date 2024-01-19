using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace art_gallery.Models
{
    public class Art
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; }
        public bool Private { get; set; } = true;
        public string? Owner { get; set; }
        public string Description { get; set; }
        public string Artist { get; set; }
        public string Dimensions { get; set; }
        public byte[]? ImageData { get; set; } = new byte[0];

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DateOfWork { get; set; }

        public string Style { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal EstimatedValue { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
