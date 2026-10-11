using PostsCommentsService.Application.RepositoryInterface;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Infrastructure.ServiceImplementaion
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IPostsService postsService;
        private readonly IPostsRepository postsRepository;

        public ImageStorageService(IPostsService postsService, IPostsRepository postsRepository)
        {
            this.postsService = postsService;
            this.postsRepository = postsRepository;
        }

        public async Task<bool> DeleteImageAsync(string imageUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return (false);
            }

            var path = Path.Combine("wwwroot", imageUrl.TrimStart('/'));
            if (File.Exists(path))
            {
                File.Delete(path);
                return (true);
            }
            return (false);

        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct)
        {
            var extension = Path.GetExtension(fileName);
            var newName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine("wwwroot", "uploads", newName);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using var fileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fileStream, ct);
            return $"/uploads/{newName}";

        }


    }
}
