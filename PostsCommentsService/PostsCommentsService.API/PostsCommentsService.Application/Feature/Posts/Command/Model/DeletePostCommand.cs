using MediatR;
using PostsCommentsService.Application.Bases;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record DeletePostCommand(string Id, Guid UserId) : IRequest<Response<string>>;

}
