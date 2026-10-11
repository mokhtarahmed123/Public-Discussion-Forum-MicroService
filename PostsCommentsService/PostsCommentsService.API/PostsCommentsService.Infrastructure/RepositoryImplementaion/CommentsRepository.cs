using MongoDB.Driver;
using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Domain.Entities;
using PostsCommentsService.Infrastructure.DataBaseConfiguration;
using PostsCommentsService.Infrastructure.InfrastructureBases;

namespace PostsCommentsService.Infrastructure.RepositoryImplementaion
{
    public class CommentsRepository : GenericRepositoryAsync<Comment>, ICommentsRepository
    {
        public CommentsRepository(ForumService forumService) : base(forumService)
        {
        }

        public async Task<bool> AddImagesAsync(string CommentId, List<string> imageUrls, CancellationToken cancellationToken)
        {
            var update = Builders<Comment>.Update
            .PushEach(p => p.ImageUrls, imageUrls)
                 .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(
                p => p.Id == CommentId && !p.IsDeleted,
                update,
                cancellationToken: cancellationToken);

            return result.MatchedCount > 0;
        }

        public Task<List<Comment>> GetImagesByCommentIdAsync(string commentId, CancellationToken cancellationToken)
        {
            return _collection.Find(p => p.Id == commentId && !p.IsDeleted)
                .Project(p => new Comment
                {
                    Id = p.Id,
                    PostId = p.PostId,
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
