using PostsCommentsService.Data.Interfaces;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.RepositoryInterface
{
    public interface ICommentsRepository : IGenericRepositoryAsync<Comment>
    {
        Task<bool> AddImagesAsync(string CommentId, List<string> imageUrls, CancellationToken cancellationToken);
        Task<List<Comment>> GetImagesByCommentIdAsync(string commentId, CancellationToken cancellationToken);

    }
}
