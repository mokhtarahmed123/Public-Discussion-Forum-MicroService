using MediatR;
using Microsoft.AspNetCore.Http;
using PostsCommentsService.Application.Bases;

namespace PostsCommentsService.Application.Feature.Images.Command.Model
{
    public record UploadImagesCommentCommand : IRequest<Response<List<string>>>
    {
        public string CommentId { get; init; } = default!;
        public List<IFormFile> Files { get; init; } = new();

    }
}
