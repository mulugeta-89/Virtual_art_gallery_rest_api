using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace art_gallery.Models
{
    public class SoloExhibition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime StartDate { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime EndDate { get; set; }

        public string? Curator { get; set; }

        [BsonElement("artworkIds")]
        public List<string> ArtworkIds { get; set; } = new List<string>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
