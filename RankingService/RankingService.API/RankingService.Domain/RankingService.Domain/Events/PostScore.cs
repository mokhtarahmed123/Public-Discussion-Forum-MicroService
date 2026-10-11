using MongoDB.Bson.Serialization.Attributes;

namespace RankingService.Domain.Events
{
    public class PostScore
    {
        [BsonId]

        public string PostId { get; set; } = default!;
        public int Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
