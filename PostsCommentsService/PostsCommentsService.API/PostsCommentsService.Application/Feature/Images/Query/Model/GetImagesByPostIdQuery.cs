using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Query.Result;

namespace PostsCommentsService.Application.Feature.Images.Query.Model
{
    public record GetImagesByPostIdQuery(string PostId) : IRequest<Response<List<ImageDto>>>;
}


