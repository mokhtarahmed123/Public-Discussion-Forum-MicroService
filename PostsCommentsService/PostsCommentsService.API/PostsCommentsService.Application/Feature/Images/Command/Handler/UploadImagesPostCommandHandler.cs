using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Command.Model;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Images.Command.Handler
{
    public class UploadImagesPostCommandHandler
        : ResponseHandler, IRequestHandler<UploadImagesPostCommand, Response<List<string>>>
    {
        private readonly IPostsService postsService;
        private readonly IImageStorageService imageStorageService;

        public UploadImagesPostCommandHandler(IPostsService postsService, IImageStorageService imageStorageService)
        {
            this.postsService = postsService;
            this.imageStorageService = imageStorageService;
        }

        public async Task<Response<List<string>>> Handle(UploadImagesPostCommand request, CancellationToken cancellationToken)
        {

            var post = await postsService.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return NotFound<List<string>>("Post not found.");

            var uploadedUrls = new List<string>();
            try
            {
                foreach (var file in request.Files)
                {
                    await using var stream = file.OpenReadStream();
                    var url = await imageStorageService.UploadAsync(
                        stream, file.FileName, file.ContentType, cancellationToken);

                    uploadedUrls.Add(url);
                }
                await postsService.AddImagesAsync(request.PostId, uploadedUrls, cancellationToken);
            }
            catch
            {

                foreach (var url in uploadedUrls)
                    await imageStorageService.DeleteImageAsync(url, CancellationToken.None);

                throw;
            }

            return Success(uploadedUrls);
        }
    }
}