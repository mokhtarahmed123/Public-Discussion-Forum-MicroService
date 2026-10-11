using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Query.Model;
using PostsCommentsService.Application.Feature.Images.Query.Result;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Images.Query.Handler
{
    public class GetImagesByPostIdQueryHandler : ResponseHandler, IRequestHandler<GetImagesByPostIdQuery, Response<List<ImageDto>>>
    {
        private readonly IPostsService postsService;

        public GetImagesByPostIdQueryHandler(IPostsService postsService)
        {
            this.postsService = postsService;
        }
        public async Task<Response<List<ImageDto>>> Handle(GetImagesByPostIdQuery request, CancellationToken cancellationToken)
        {
            var images = await postsService.GetImagesByPostIdAsync(request.PostId, cancellationToken);

            if (images is null || !images.Any())
            {
                return NotFound<List<ImageDto>>("No images found for the specified post ID.");
            }

            var imageDtos = images
                .Select(image => new ImageDto(
                    image.ImageUrls?.FirstOrDefault(),
                    image.ImageUrls?.ToArray() ?? Array.Empty<string>(),
                    request.PostId,
                    null))
                .ToList();

            return Success(imageDtos, "Images retrieved successfully.");
        }
    }
}
