using MediatR;
using VotesService.Application.Bases;
using VotesService.Domain.Enum;

namespace VotesService.Application.Feature.Votes.Command.Model
{
    public record LockVoteCommand : IRequest<Response<bool>>
    {
        public string TargetId { get; set; } = null!;
        public VoteTargetType TargetType { get; set; }
        public bool IsLocked { get; set; } = true;   // true = Close ، false = Open
        public string? PostId { get; set; }

        //public Guid UserId { get; set; }
    }

}
