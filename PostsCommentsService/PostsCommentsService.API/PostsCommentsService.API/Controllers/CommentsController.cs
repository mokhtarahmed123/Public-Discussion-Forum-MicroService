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


        [HttpPatch("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> Update(string postId, string id, [FromBody] UpdateCommentCommand command, CancellationToken cancellationToken)
        {
            command.PostId = postId;
            command.Id = id;

            return NewResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("posts/{postId}/comments")]
        public async Task<IActionResult> Create(string postId, [FromBody] CreateCommentCommand command, CancellationToken cancellationToken)
        {
            command.PostId = postId;

            return NewResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpDelete("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> Delete(string postId, string id, Guid UserId, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new DeleteCommentCommand(id, UserId, postId), cancellationToken));
        }

        [HttpGet("posts/{postId}/comments/{id}")]
        public async Task<IActionResult> GetById(string postId, string id, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetCommentByIdQuery(postId, id), cancellationToken));
        }
        [HttpGet("post/{postId}/comments")]
        public async Task<IActionResult> GetByPost(string postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return NewResult(await Mediator.Send(new GetCommentsByPostQuery(postId, page, pageSize), cancellationToken));
        }
        [HttpGet("users/{userId:guid}/comments")]
        public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetCommentsByUserQuery(userId), cancellationToken));
        }

        [HttpGet("posts/{postId}/comments/{id}/replies")]
        public async Task<IActionResult> GetReplies(string postId, string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return NewResult(await Mediator.Send(new GetRepliesCommentsQuery(postId, id, page, pageSize), cancellationToken));
        }


        [HttpPost("Comments/UploadImages/{commentId}")]
        //[Authorize]
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
