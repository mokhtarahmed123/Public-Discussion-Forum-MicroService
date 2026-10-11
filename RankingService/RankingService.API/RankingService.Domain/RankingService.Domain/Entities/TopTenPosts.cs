using MongoDB.Bson.Serialization.Attributes;

namespace RankingService.Domain.Entities
{
    public class TopTenPosts
    {
        [BsonId]
        public string Id { get; init; } = default!;
        public string Title { get; init; } = default!;
        public string Content { get; init; } = default!;
        public int Score { get; set; }

        public int LikesCount { get; init; }
        public int CommentsCount { get; init; }
        public bool IsLocked { get; init; }
        public bool IsPinned { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }




    }
}
