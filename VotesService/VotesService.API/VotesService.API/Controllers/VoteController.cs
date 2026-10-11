using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VotesService.API.Bases;
using VotesService.Application.Bases;
using VotesService.Application.Feature.Votes.Command.Model;
using VotesService.Application.Feature.Votes.Query.Model;
using VotesService.Application.Feature.Votes.Query.Results;
using VotesService.Domain.Enum;

namespace VotesService.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class VoteController : AppBaseController
    {

        [Authorize]
        [HttpPost("posts/{postId}/vote")]
        [SwaggerOperation(
            Summary = "Votes on a post",
            Description = "Adds a new vote, changes an existing vote, or removes it (toggle) when the same vote type is sent again.")]
        [SwaggerResponse(200, "Vote added, changed, or removed successfully", type: typeof(Response<string>))]
        [SwaggerResponse(400, "Invalid vote data or voting is locked", type: typeof(Response<string>))]
        [SwaggerResponse(404, "Post not found", type: typeof(Response<string>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> VotePost([FromRoute] string postId, [FromBody] VotePostCommand command, CancellationToken cancellationToken)
        {
            command.TargetId = postId;
            return NewResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet("posts/{postId}/vote")]
        [SwaggerOperation(
            Summary = "Gets the vote summary of a post",
            Description = "Returns the number of up votes, down votes, and the final score.")]
        [SwaggerResponse(200, "Vote summary retrieved successfully", type: typeof(Response<GetVoteSummaryResult>))]
        [SwaggerResponse(400, "Invalid post id", type: typeof(Response<GetVoteSummaryResult>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> GetPostSummary(string postId, CancellationToken cancellationToken)
            => NewResult(await Mediator.Send(new GetVoteSummaryQuery(VoteTargetType.Post, postId), cancellationToken));


        [Authorize]
        [HttpGet("posts/{postId}/vote/mine")]
        [SwaggerOperation(
            Summary = "Gets a user's vote on a post",
            Description = "Returns the vote type of the given user. The type is null if the user has not voted.")]
        [SwaggerResponse(200, "User vote retrieved successfully", type: typeof(Response<GetUserVoteResult>))]
        [SwaggerResponse(400, "Invalid post id or user", type: typeof(Response<GetUserVoteResult>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> GetMyPostVote(string postId, [FromQuery] Guid userId, CancellationToken cancellationToken)
            => NewResult(await Mediator.Send(new GetUserVoteQuery(VoteTargetType.Post, postId, userId), cancellationToken));

        [Authorize]
        [HttpPatch("posts/{postId}/vote/lock")]
        [SwaggerOperation(
            Summary = "Locks or unlocks voting on a post",
            Description = "Only the post owner can lock or unlock. Set isLocked to true to lock, or false to unlock.")]
        [SwaggerResponse(200, "Lock state updated successfully", type: typeof(Response<bool>))]
        [SwaggerResponse(400, "Invalid post id", type: typeof(Response<bool>))]
        [SwaggerResponse(403, "Only the owner can lock or unlock voting", type: typeof(Response<bool>))]
        [SwaggerResponse(404, "Post not found", type: typeof(Response<bool>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> LockPost(string postId, [FromBody] LockVoteCommand command, CancellationToken cancellationToken)
        {
            command.TargetType = VoteTargetType.Post;
            command.TargetId = postId;
            command.PostId = postId;
            return NewResult(await Mediator.Send(command, cancellationToken));
        }

        // ===================== Comments =====================
        [Authorize]

        [HttpPost("posts/{postId}/comments/{commentId}/vote")]
        [SwaggerOperation(
            Summary = "Votes on a comment",
            Description = "Adds a new vote, changes an existing vote, or removes it (toggle) when the same vote type is sent again.")]
        [SwaggerResponse(200, "Vote added, changed, or removed successfully", type: typeof(Response<string>))]
        [SwaggerResponse(400, "Invalid vote data or voting is locked", type: typeof(Response<string>))]
        [SwaggerResponse(404, "Comment not found", type: typeof(Response<string>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> VoteComment(string postId, string commentId, [FromBody] VoteCommentCommand command, CancellationToken cancellationToken)
        {
            command.PostId = postId;
            command.TargetId = commentId;
            return NewResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet("posts/{postId}/comments/{commentId}/vote")]
        [SwaggerOperation(
            Summary = "Gets the vote summary of a comment",
            Description = "Returns the number of up votes, down votes, and the final score.")]
        [SwaggerResponse(200, "Vote summary retrieved successfully", type: typeof(Response<GetVoteSummaryResult>))]
        [SwaggerResponse(400, "Invalid comment id", type: typeof(Response<GetVoteSummaryResult>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> GetCommentSummary(string postId, string commentId, CancellationToken cancellationToken)
            => NewResult(await Mediator.Send(new GetVoteSummaryQuery(VoteTargetType.Comment, commentId), cancellationToken));

        [Authorize]
        [HttpGet("posts/{postId}/comments/{commentId}/vote/mine")]
        [SwaggerOperation(
            Summary = "Gets a user's vote on a comment",
            Description = "Returns the vote type of the given user. The type is null if the user has not voted.")]
        [SwaggerResponse(200, "User vote retrieved successfully", type: typeof(Response<GetUserVoteResult>))]
        [SwaggerResponse(400, "Invalid comment id or user", type: typeof(Response<GetUserVoteResult>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> GetMyCommentVote(string postId, string commentId, [FromQuery] Guid userId, CancellationToken cancellationToken)
            => NewResult(await Mediator.Send(new GetUserVoteQuery(VoteTargetType.Comment, commentId, userId), cancellationToken));

        [Authorize]
        [HttpPatch("posts/{postId}/comments/{commentId}/vote/lock")]
        [SwaggerOperation(
            Summary = "Locks or unlocks voting on a comment",
            Description = "Only the comment owner can lock or unlock. Set isLocked to true to lock, or false to unlock.")]
        [SwaggerResponse(200, "Lock state updated successfully", type: typeof(Response<bool>))]
        [SwaggerResponse(400, "Invalid post or comment id", type: typeof(Response<bool>))]
        [SwaggerResponse(403, "Only the owner can lock or unlock voting", type: typeof(Response<bool>))]
        [SwaggerResponse(404, "Comment not found", type: typeof(Response<bool>))]
        [SwaggerResponse(500, "An unexpected error occurred")]
        public async Task<IActionResult> LockComment(string postId, string commentId, [FromBody] LockVoteCommand command, CancellationToken cancellationToken)
        {
            command.TargetType = VoteTargetType.Comment;
            command.TargetId = commentId;
            command.PostId = postId;
            return NewResult(await Mediator.Send(command, cancellationToken));
        }


        [HttpGet("Votes/TopTenPosts")]
        public async Task<IActionResult> GetTopTenPost()
        {
            var query = await Mediator.Send(new GetTopTenPostAsync());
            return NewResult(query);

        }
        [HttpGet("Votes/TopTenComments")]
        public async Task<IActionResult> GetTopTenComments()
        {
            var query = await Mediator.Send(new GetTopTenCommentAsync());
            return NewResult(query);

        }
    }
}