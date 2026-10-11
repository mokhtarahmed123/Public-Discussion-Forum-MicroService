using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Infrastructure.ServiceImplementaion
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<Post> AddAsync(Post post, CancellationToken cancellationToken)
        {
            return await postsRepository.AddAsync(post, cancellationToken);
        }

        public async Task AddImagesAsync(string postId, List<string> imageUrls, CancellationToken cancellationToken)
        {
            await postsRepository.AddImagesAsync(postId, imageUrls, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, Guid userId, CancellationToken cancellationToken)
        {
            var existing = await postsRepository.GetByIdAsync(p => p.Id == id, cancellationToken);
            if (existing is null)
                return false;

            await postsRepository.DeleteAsync(existing, cancellationToken);
            return true;
        }

        public async Task<List<Post>> GetAll(CancellationToken cancellationToken)
        {
            return await postsRepository.GetAll(cancellationToken);

        }

        public async Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await postsRepository.GetByIdAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Post>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await postsRepository.GetTableNoTracking(cancellationToken)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public Task<Post> GetImageByIdAsync(string postId, string imageId, CancellationToken cancellationToken)
        {
            return postsRepository.GetImageByIdAsync(postId, imageId, cancellationToken);

        }

        public Task<List<Post>> GetImagesByPostIdAsync(string postId, CancellationToken cancellationToken)
        {
            return postsRepository.GetImagesByPostIdAsync(postId, cancellationToken);

        }

        public async Task<List<Post>> GetLatestAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await postsRepository.GetTableNoTracking(cancellationToken)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task IncrementCommentsCountAsync(string postId, int value, CancellationToken cancellationToken)
        {
            var post = await postsRepository.GetByIdAsync(p => p.Id == postId, cancellationToken);
            if (post is null) return;

            post.CommentsCount += value;
            await postsRepository.UpdateAsync(post, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Post post, CancellationToken cancellationToken)
        {
            await postsRepository.UpdateAsync(post, cancellationToken);
            return true;
        }
    }
}