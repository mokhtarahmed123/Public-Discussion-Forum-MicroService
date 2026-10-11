using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Query.Results;

namespace PostsCommentsService.Application.Feature.Comments.Query.Model
{
    public record GetCommentByIdQuery(string PostId, string CommentId) : IRequest<Response<GetCommentByIdResult>>
 ;
}
