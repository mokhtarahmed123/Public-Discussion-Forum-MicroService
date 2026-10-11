using MediatR;
using PostsCommentsService.Application.Bases;
using System.Text.Json.Serialization;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record UpdateCommentCommand : IRequest<Response<string>>
    {

        public string Id { get; set; } = null!;
        [JsonIgnore]
        public string PostId { get; set; } = null!;
        public string Content { get; init; } = null!;

    }
}
