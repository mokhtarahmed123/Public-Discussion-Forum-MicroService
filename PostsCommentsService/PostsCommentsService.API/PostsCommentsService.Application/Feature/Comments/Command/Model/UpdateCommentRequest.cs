namespace PostsCommentsService.Application.Feature.Comments.Command.Model
{
    public record UpdateCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}
