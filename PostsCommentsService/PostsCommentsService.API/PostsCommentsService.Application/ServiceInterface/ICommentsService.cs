using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.ServiceInterface
{
    public interface ICommentsService
    {
        Task<Comment> AddAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> GetByIdAsync(string id, string PostId, CancellationToken cancellationToken);
        Task<Comment?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<Comment>> GetByPostAsync(string postId, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<Comment>> GetRepliesAsync(string parentCommentId, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<Comment>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Comment comment, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, string postId, Guid userId, CancellationToken cancellationToken);
        Task IncrementRepliesCountAsync(string commentId, int value, CancellationToken cancellationToken);
        Task AddImagesAsync(string CommentId, List<string> imageUrls, CancellationToken cancellationToken);
        Task<List<Comment>> GetImagesByCommentIdAsync(string commentId, CancellationToken cancellationToken);

    }
}
