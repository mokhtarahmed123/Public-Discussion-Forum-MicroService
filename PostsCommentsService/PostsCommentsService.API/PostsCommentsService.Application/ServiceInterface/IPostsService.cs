using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.ServiceInterface
{
    public interface IPostsService
    {
        Task<Post> AddAsync(Post post, CancellationToken cancellationToken);
        Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<Post>> GetLatestAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<List<Post>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Post>> GetAll(CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Post post, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, Guid userId, CancellationToken cancellationToken);
        Task IncrementCommentsCountAsync(string postId, int value, CancellationToken cancellationToken);
        Task AddImagesAsync(string postId, List<string> imageUrls, CancellationToken cancellationToken);
        Task<Post> GetImageByIdAsync(string postId, string imageId, CancellationToken cancellationToken);
        Task<List<Post>> GetImagesByPostIdAsync(string postId, CancellationToken cancellationToken);


    }
}
