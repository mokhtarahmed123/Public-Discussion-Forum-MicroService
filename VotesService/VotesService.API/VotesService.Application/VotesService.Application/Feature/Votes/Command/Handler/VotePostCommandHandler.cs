using Forum.Contracts;
using MassTransit;
using MediatR;
using VotesService.Application.Bases;
using VotesService.Application.ExternalApiService.PostServiceInterface;
using VotesService.Application.Feature.Votes.Command.Model;
using VotesService.Application.ServiceInterface;
using VotesService.Domain.Enum;
using VotesService.Domain.Helper;
namespace VotesService.Application.Feature.Votes.Command.Handler
{
    public class VotePostCommandHandler : ResponseHandler, IRequestHandler<VotePostCommand, Bases.Response<string>>
    {
        private readonly IVotesService votesService;
        private readonly IPostClient postClient;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ICurrentUserService currentUserService;

        public VotePostCommandHandler(IVotesService votesService, IPostClient postClient, IPublishEndpoint publishEndpoint, ICurrentUserService currentUserService)
        {
            this.votesService = votesService;
            this.postClient = postClient;
            this.publishEndpoint = publishEndpoint;
            this.currentUserService = currentUserService;
        }

        public async Task<Bases.Response<string>> Handle(VotePostCommand request, CancellationToken ct)
        {
            var userId = currentUserService.UserId;

            var postId = request.TargetId;

            var existing = await votesService.GetUserVoteAsync(
                Guid.Parse(userId), VoteTargetType.Post, request.TargetId, ct);

            if (existing is not null && existing.Locked)
                return BadRequest<string>("تصويتك مقفول ومينفعش يتغير.");

            if (existing is null &&
                await votesService.IsLockedAsync(VoteTargetType.Post, request.TargetId, ct))
                return BadRequest<string>("التصويت مقفول على البوست ده.");

            var newValue = ToValue(request.Type);


            if (existing is null)
            {
                var post = await postClient.GetPostByIdAsync(request.TargetId, ct);
                if (post is null || post.IsDeleted)
                    return NotFound<string>("البوست مش موجود.");

                var vote = new Domain.Entities.Votes
                {
                    TargetId = request.TargetId,
                    PostId = request.TargetId,
                    TargetType = VoteTargetType.Post,
                    UserId = Guid.Parse(userId),
                    Type = request.Type
                };


                var created = await votesService.AddAsync(vote, ct);
                await Publish(postId, Guid.Parse(userId), newValue, ct);
                return Success(created.Id);
            }


            if (existing.Type == request.Type)
            {
                await votesService.DeleteAsync(existing, ct);
                await Publish(postId, Guid.Parse(userId), -newValue, ct);
                return Deleted<string>();
            }

            existing.Type = request.Type;
            existing.UpdatedAt = DateTime.UtcNow;
            await votesService.UpdateAsync(existing, ct);
            await Publish(postId, Guid.Parse(userId), newValue * 2, ct);

            return Success(existing.Id);
        }

        private static int ToValue(VoteType type) => type == VoteType.Up ? 1 : -1;

        private Task Publish(string postId, Guid userId, int delta, CancellationToken ct) =>
            publishEndpoint.Publish(new VotePostMessage
            {
                PostId = postId,
                UserId = (userId),
                Delta = delta
            }, ct);
    }
}
