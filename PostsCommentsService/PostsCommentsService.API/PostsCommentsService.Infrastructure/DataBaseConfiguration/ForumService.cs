using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PostsCommentsService.API.Bases;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Infrastructure.DataBaseConfiguration
{
    public class ForumService
    {
        private readonly IMongoDatabase _database;
        private readonly PostsAndCommentsDatabaseSettings _settings;

        public ForumService(IOptions<PostsAndCommentsDatabaseSettings> options)
        {
            _settings = options.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var name = typeof(T).Name switch
            {
                nameof(Post) => _settings.PostsCollectionName,
                nameof(Comment) => _settings.CommentsCollectionName,
                _ => throw new InvalidOperationException($"No collection configured for {typeof(T).Name}")
            };

            return _database.GetCollection<T>(name);
        }
    }
}
