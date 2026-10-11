using MongoDB.Driver;
using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Domain.Entities;
using PostsCommentsService.Infrastructure.DataBaseConfiguration;
using PostsCommentsService.Infrastructure.InfrastructureBases;

namespace PostsCommentsService.Infrastructure.RepositoryImplementaion
{
    public class PostsRepository : GenericRepositoryAsync<Post>, IPostsRepository
    {
        public PostsRepository(ForumService forumService) : base(forumService)
        {
        }

        public async Task<bool> AddImagesAsync(string postId, List<string> imageUrls, CancellationToken cancellationToken)
        {
            var update = Builders<Post>.Update
                    .PushEach(p => p.ImageUrls, imageUrls)
          .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(
                p => p.Id == postId && !p.IsDeleted,
                update,
                cancellationToken: cancellationToken);

            return result.MatchedCount > 0;
        }

        public async Task<Post> GetImageByIdAsync(string postId, string imageId, CancellationToken cancellationToken)
        {
            var post = await _collection.Find(p => p.Id == postId && !p.IsDeleted)
                .Project(p => new Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Content = p.Content,
                    ImageUrls = p.ImageUrls.Where(url => url == imageId).ToList(),
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    IsDeleted = p.IsDeleted
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (post == null || post.ImageUrls.Count == 0)
            {
                throw new KeyNotFoundException($"Image with ID '{imageId}' not found in post '{postId}'.");
            }
            return post;


        }

        public Task<List<Post>> GetImagesByPostIdAsync(string postId, CancellationToken cancellationToken)
        {
            return _collection.Find(p => p.Id == postId && !p.IsDeleted)
                .Project(p => new Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Content = p.Content,
                    ImageUrls = p.ImageUrls,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    IsDeleted = p.IsDeleted
                })
                .ToListAsync(cancellationToken);

        }
    }
}
