using PostsCommentsService.Data.Interfaces;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.RepositoryInterface
{
    public interface IPostsRepository : IGenericRepositoryAsync<Post>
    {
        Task<bool> AddImagesAsync(string postId, List<string> imageUrls, CancellationToken cancellationToken);
        Task<List<Post>> GetImagesByPostIdAsync(string postId, CancellationToken cancellationToken);
        Task<Post> GetImageByIdAsync(string postId, string imageId, CancellationToken cancellationToken);
    }
}