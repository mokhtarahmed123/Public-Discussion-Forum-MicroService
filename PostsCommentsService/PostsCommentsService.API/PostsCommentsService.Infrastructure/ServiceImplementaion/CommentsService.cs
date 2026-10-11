using MongoDB.Driver.Linq;
using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Infrastructure.ServiceImplementaion
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository commentsRepository;

        public CommentsService(ICommentsRepository commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken)
        {
            return await commentsRepository.AddAsync(comment, cancellationToken);
        }

        public Task AddImagesAsync(string CommentId, List<string> imageUrls, CancellationToken cancellationToken)
        {
            return commentsRepository.AddImagesAsync(CommentId, imageUrls, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, string PostId, Guid userId, CancellationToken cancellationToken)
        {
            var existing = await commentsRepository.GetByIdAsync(c => c.Id == id && c.PostId == PostId, cancellationToken);
            if (existing is null)
                return false;

            await commentsRepository.DeleteAsync(existing, cancellationToken);
            return true;
        }

        public async Task<Comment?> GetByIdAsync(string id, string PostId, CancellationToken cancellationToken)
        {
            return await commentsRepository.GetByIdAsync(c => c.Id == id && c.PostId == PostId, cancellationToken);
        }

        public async Task<Comment?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await commentsRepository.GetByIdAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Comment>> GetByPostAsync(string postId, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await commentsRepository.GetTableNoTracking(cancellationToken)
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Comment>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await commentsRepository.GetTableNoTracking(cancellationToken)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Comment>> GetImagesByCommentIdAsync(string commentId, CancellationToken cancellationToken)
        {
            return commentsRepository.GetImagesByCommentIdAsync(commentId, cancellationToken);

        }

        public async Task<List<Comment>> GetRepliesAsync(string parentCommentId, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await commentsRepository.GetTableNoTracking(cancellationToken)
                .Where(c => c.ParentCommentId == parentCommentId)
                .OrderBy(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task IncrementRepliesCountAsync(string commentId, int value, CancellationToken cancellationToken)
        {
            var comment = await commentsRepository.GetByIdAsync(c => c.Id == commentId, cancellationToken);
            if (comment is null) return;

            comment.RepliesCount += value;
            await commentsRepository.UpdateAsync(comment, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Comment comment, CancellationToken cancellationToken)
        {
            await commentsRepository.UpdateAsync(comment, cancellationToken);
            return true;
        }
    }
}