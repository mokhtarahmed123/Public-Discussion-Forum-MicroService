using MediatR;
using System.Text.Json.Serialization;
using VotesService.Application.Bases;
using VotesService.Domain.Enum;

namespace VotesService.Application.Feature.Votes.Command.Model
{
    public class VotePostCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public string TargetId { get; set; } = null!;
        public VoteType Type { get; init; }

        //public Guid UserId { get; set; }   // From JWT Token
    }


}
