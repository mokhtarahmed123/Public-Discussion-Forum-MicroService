using Microsoft.AspNetCore.Mvc;
using PostsCommentsService.API.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.Feature.Images.Command.Model;
using PostsCommentsService.Application.Feature.Images.Query.Model;
using PostsCommentsService.Application.Feature.Posts.Query.Model;

namespace PostsCommentsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : AppBaseController
    {
        [HttpGet("Start")]
        public IActionResult Ping() => Ok(" Posts API is running.");

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostCommand command, CancellationToken cancellationToken)
        {

            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromBody] Guid userId, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new DeletePostCommand(id, userId), cancellationToken);
            return NewResult(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePostCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetAllPostsQuery(), cancellationToken));
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return NewResult(await Mediator.Send(new GetLatestPostsQuery(page, pageSize), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetPostByIdQuery(id), cancellationToken));
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
        {
            return NewResult(await Mediator.Send(new GetPostsByUserQuery(userId), cancellationToken));
        }
        //[HttpGet("{id}/replies")]
        //public async Task<IActionResult> GetReplies(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        //{
        //    return NewResult(await Mediator.Send(new GetRepliesCommentsQuery(id, "", page, pageSize), cancellationToken));
        //}

        [HttpPost("UploadImages/{postId}")]
        //[Authorize]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(30_000_000)]
        public async Task<IActionResult> UploadImages(
    string postId,
    [FromForm] List<IFormFile> files,
          CancellationToken cancellationToken)
        {
            var command = new UploadImagesPostCommand
            {
                PostId = postId,
                Files = files
            };

            var result = await Mediator.Send(command, cancellationToken);
            return NewResult(result);
        }
        [HttpGet("posts/{postId}/images")]
        public async Task<IActionResult> GetImagesByPostId(string postId, CancellationToken ct)
        {
            var response = await Mediator.Send(new GetImagesByPostIdQuery(postId), ct);
            return StatusCode((int)response.StatusCode, response);
        }

        // GET api/posts/{postId}/images/{id}
        //[HttpGet("posts/{postId}/images/{id}")]
        //public async Task<IActionResult> GetImageById(string postId, string id, CancellationToken ct)
        //{
        //    var response = await Mediator.Send(new GetImageByIdQuery(id, postId), ct);
        //    return StatusCode((int)response.StatusCode, response);
        //}
    }
}