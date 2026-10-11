using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Command.Model;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Images.Command.Handler
{
    public class UploadImagesCommentCommandHandler : ResponseHandler,
        IRequestHandler<UploadImagesCommentCommand, Response<List<string>>>
    {
        private readonly ICommentsService CommentsService;
        private readonly IImageStorageService imageStorageService;

        public UploadImagesCommentCommandHandler(ICommentsService CommentsService, IImageStorageService imageStorageService)
        {
            this.CommentsService = CommentsService;
            this.imageStorageService = imageStorageService;
        }


        public async Task<Response<List<string>>> Handle(UploadImagesCommentCommand request, CancellationToken cancellationToken)
        {

            var post = await CommentsService.GetByIdAsync(request.CommentId, cancellationToken);
            if (post is null)
                return NotFound<List<string>>("Comment not found.");

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
                await CommentsService.AddImagesAsync(request.CommentId, uploadedUrls, cancellationToken);
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
