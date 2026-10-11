using MediatR;
using PostsCommentsService.Application.Bases;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record CreateCommentCommand : IRequest<Response<string>>
    {

        public string PostId { get; set; } = null!;
        public string Content { get; init; } = null!;
        public string? ParentCommentId { get; init; }
        public Guid UserId { get; set; }
    }
}
