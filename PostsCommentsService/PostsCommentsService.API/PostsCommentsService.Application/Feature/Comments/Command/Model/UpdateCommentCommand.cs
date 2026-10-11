using MediatR;
using PostsCommentsService.Application.Bases;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record UpdateCommentCommand : IRequest<Response<string>>
    {

        public string Id { get; set; } = null!;

        public string PostId { get; set; } = null!;
        public string Content { get; init; } = null!;


        public Guid UserId { get; set; }
    }
}
