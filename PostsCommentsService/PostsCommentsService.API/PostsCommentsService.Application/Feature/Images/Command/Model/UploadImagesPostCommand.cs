using MediatR;
using Microsoft.AspNetCore.Http;
using PostsCommentsService.Application.Bases;

namespace PostsCommentsService.Application.Feature.Images.Command.Model
{
    public record UploadImagesPostCommand : IRequest<Response<List<string>>>
    {
        public string PostId { get; set; } = default!;
        public List<IFormFile> Files { get; set; } = new();
    }
}
