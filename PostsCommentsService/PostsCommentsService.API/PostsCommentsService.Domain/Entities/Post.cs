using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PostsCommentsService.Domain.Enums;

namespace PostsCommentsService.Domain.Entities
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]

        public TypeOfPosts TypeOfPosts { get; set; } = TypeOfPosts.None;

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }

        [BsonElement("ImageUrls")]
        public List<string> ImageUrls { get; set; } = new();

    }
}
