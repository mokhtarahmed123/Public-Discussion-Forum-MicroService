using Forum.Contracts;
using MassTransit;
using MediatR;
using VotesService.Application.Bases;
using VotesService.Application.ExternalApiService.CommentServiceInterface;
using VotesService.Application.Feature.Votes.Command.Model;
using VotesService.Application.ServiceInterface;
using VotesService.Domain.Enum;
using VotesService.Domain.Helper;

namespace VotesService.Application.Feature.Votes.Command.Handler
{
    public class VoteCommentCommandHandler : ResponseHandler, IRequestHandler<VoteCommentCommand, Bases.Response<string>>
    {
        private readonly IVotesService votesService;
        private readonly ICommentClient commentClient;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ICurrentUserService currentUserService;

        public VoteCommentCommandHandler(
            IVotesService votesService,
            ICommentClient commentClient,
            IPublishEndpoint publishEndpoint, ICurrentUserService currentUserService)
        {
            this.votesService = votesService;
            this.commentClient = commentClient;
            this.publishEndpoint = publishEndpoint;
            this.currentUserService = currentUserService;
        }

        public async Task<Bases.Response<string>> Handle(VoteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var existing = await votesService.GetUserVoteAsync(
              Guid.Parse(userId), VoteTargetType.Comment, request.TargetId, cancellationToken);

            if (existing is not null && existing.Locked)
                return BadRequest<string>("تصويتك مقفول ومينفعش يتغير.");

            if (existing is null &&
                await votesService.IsLockedAsync(VoteTargetType.Comment, request.TargetId, cancellationToken))
                return BadRequest<string>("التصويت مقفول على الكومنت ده.");

            var newValue = ToValue(request.Type);

            if (existing is null)
            {
                var comment = await commentClient.GetCommentByIdAsync(request.PostId, request.TargetId, cancellationToken);
                if (comment is null || comment.IsDeleted)
                    return NotFound<string>("الكومنت مش موجود.");

                var vote = new Domain.Entities.Votes
                {
                    TargetId = request.TargetId,
                    TargetType = VoteTargetType.Comment,
                    UserId = Guid.Parse(userId),
                    PostId = request.PostId,
                    Type = request.Type
                };

                var created = await votesService.AddAsync(vote, cancellationToken);
                await Publish(request.TargetId, request.PostId, Guid.Parse(userId), newValue, cancellationToken);
                return Success(created.Id);
            }

            if (existing.Type == request.Type)
            {
                await votesService.DeleteAsync(existing, cancellationToken);
                await Publish(request.TargetId, request.PostId, Guid.Parse(userId), -newValue, cancellationToken);
                return Deleted<string>();
            }

            existing.Type = request.Type;
            existing.UpdatedAt = DateTime.UtcNow;
            await votesService.UpdateAsync(existing, cancellationToken);
            await Publish(request.TargetId, request.PostId, Guid.Parse(userId), newValue * 2, cancellationToken);

            return Success(existing.Id);
        }

        private static int ToValue(VoteType type) => type == VoteType.Up ? 1 : -1;

        private Task Publish(string commentId, string PostId, Guid userId, int delta, CancellationToken ct) =>
            publishEndpoint.Publish(new VoteCommentMessage
            {
                CommentId = commentId,
                UserId = userId,
                Delta = delta,
                PostId = PostId,
            }, ct);
    }
}