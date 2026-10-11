using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Query.Model;
using PostsCommentsService.Application.Feature.Images.Query.Result;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Images.Query.Handler
{
    public class GetImagesByCommentIdQueryHandler : ResponseHandler, IRequestHandler<GetImagesByCommentIdQuery, Response<List<ImageDto>>>
    {
        private readonly ICommentsService commentsService;

        public GetImagesByCommentIdQueryHandler(ICommentsService commentsService)

        {
            this.commentsService = commentsService;
        }
        public async Task<Response<List<ImageDto>>> Handle(GetImagesByCommentIdQuery request, CancellationToken cancellationToken)
        {
            var images = await commentsService.GetImagesByCommentIdAsync(request.CommentId, cancellationToken);

            if (images is null || !images.Any())
            {
                return NotFound<List<ImageDto>>("No images found for the given comment ID.");
            }

            var imageDtos = images
                .Select(image => new ImageDto(
                    image.ImageUrls?.FirstOrDefault(),
                    image.ImageUrls?.ToArray() ?? Array.Empty<string>(),
                    image.PostId,
                    image.Id))
                .ToList();

            return Success(imageDtos, "Images retrieved successfully.");


        }
    }
}
