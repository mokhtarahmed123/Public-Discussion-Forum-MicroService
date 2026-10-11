using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsCommentsService.API.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Model;
using PostsCommentsService.Application.Feature.Images.Command.Model;
using PostsCommentsService.Application.Feature.Images.Query.Model;

namespace PostsCommentsService.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentsController : AppBaseController
    {

        [Authorize]
        [HttpPatch("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> Update([FromRoute] string postId, [FromRoute] string id, [FromBody] UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCommentCommand
            {
                PostId = postId,
                Id = id,
                Content = request.Content,
            };

            return NewResult(await Mediator.Send(command, cancellationToken));
        }
        [Authorize]
        [HttpPost("posts/{postId}/comments")]
        public async Task<IActionResult> Create([FromRoute] string postId, [FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
        {

            var command = new CreateCommentCommand
            {
                Content = request.Content,
                PostId = postId,
                ParentCommentId = request.ParentCommentId,

            };

            return NewResult(await Mediator.Send(command, cancellationToken));
        }
        [Authorize]

        [HttpDelete("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string postId, [FromRoute] string id, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new DeleteCommentCommand(id, postId), cancellationToken));
        }

        [HttpGet("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string postId, [FromRoute] string id, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetCommentByIdQuery(postId, id), cancellationToken));
        }
        [HttpGet("post/{postId}/comments")]
        public async Task<IActionResult> GetByPost([FromRoute] string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return NewResult(await Mediator.Send(new GetCommentsByPostQuery(postId, page, pageSize), cancellationToken));
        }
        [HttpGet("users/{userId:guid}/comments")]
        public async Task<IActionResult> GetByUser([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetCommentsByUserQuery(userId), cancellationToken));
        }

        [HttpGet("posts/{postId}/comments/{id}/replies")]
        public async Task<IActionResult> GetReplies([FromRoute] string postId, [FromRoute] string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return NewResult(await Mediator.Send(new GetRepliesCommentsQuery(postId, id, page, pageSize), cancellationToken));
        }


        [HttpPost("Comments/UploadImages/{commentId}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(30_000_000)]
        public async Task<IActionResult> UploadImages(
            string commentId,
          [FromForm] List<IFormFile> files,
          CancellationToken cancellationToken)
        {
            var command = new UploadImagesCommentCommand
            {
                CommentId = commentId,
                Files = files
            };

            var result = await Mediator.Send(command, cancellationToken);
            return NewResult(result);
        }
        [HttpGet("comments/{commentId}/images")]
        public async Task<IActionResult> GetImagesByCommentId(string commentId, CancellationToken ct)
        {
            var response = await Mediator.Send(new GetImagesByCommentIdQuery(commentId), ct);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
