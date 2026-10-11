using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PostsCommentsService.Domain.Entities
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCommentId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }

        public int LikesCount { get; set; }
        public int RepliesCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        [BsonElement("ImageUrls")]
        public List<string> ImageUrls { get; set; } = new();



    }
}
