using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotDealServer.Models
{
    public record HotDealProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        [BsonElement("Name")]
        public required string ProductName { get; set; }
        public required string Href { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public CommunityType CommunityType { get; set; }
    }
}
