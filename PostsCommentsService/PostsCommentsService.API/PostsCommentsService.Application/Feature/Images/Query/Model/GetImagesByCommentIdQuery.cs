using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Images.Query.Result;

namespace PostsCommentsService.Application.Feature.Images.Query.Model
{
    public record GetImagesByCommentIdQuery(string CommentId) : IRequest<Response<List<ImageDto>>>;

}
