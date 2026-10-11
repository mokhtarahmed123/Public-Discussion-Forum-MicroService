namespace PostsCommentsService.Application.ServiceInterface
{
    public interface IImageStorageService
    {

        Task<bool> DeleteImageAsync(string imageUrl, CancellationToken cancellationToken);
        Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

    }
}
