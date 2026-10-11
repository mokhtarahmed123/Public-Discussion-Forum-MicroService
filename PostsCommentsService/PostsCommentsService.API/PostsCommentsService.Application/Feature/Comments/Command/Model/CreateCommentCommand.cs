using MediatR;
using PostsCommentsService.Application.Bases;
using System.Text.Json.Serialization;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record CreateCommentCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public string PostId { get; set; } = null!;
        public string Content { get; init; } = null!;
        public string? ParentCommentId { get; init; }

    }
}
