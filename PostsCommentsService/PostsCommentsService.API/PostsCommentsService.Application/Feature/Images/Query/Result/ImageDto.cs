namespace PostsCommentsService.Application.Feature.Images.Query.Result
{
    public record ImageDto(string Id, string[] Url, string PostId, string? CommentId);
}
