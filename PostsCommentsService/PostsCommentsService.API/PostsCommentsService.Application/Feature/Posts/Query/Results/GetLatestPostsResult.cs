using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PostsCommentsService.Domain.Enums;

namespace PostsCommentsService.Application.Feature.Posts.Query.Results
{
    public class GetLatestPostsResult
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]

        public TypeOfPosts TypeOfPosts { get; set; }


        public Guid UserId { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }

    }
}
