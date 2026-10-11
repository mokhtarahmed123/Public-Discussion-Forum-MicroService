using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Domain.Enums;

namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record CreatePostCommand : IRequest<Response<string>>
    {
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;
        public TypeOfPosts TypeOfPosts { get; init; } = TypeOfPosts.None;
        public bool IsLocked { get; init; }

        //[JsonIgnore]
        public Guid UserId { get; set; }


    }
}
