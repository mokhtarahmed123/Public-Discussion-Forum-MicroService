namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public class CreateCommentRequest
    {
        public string Content { get; init; } = null!;
        public string? ParentCommentId { get; init; }
    }
}
