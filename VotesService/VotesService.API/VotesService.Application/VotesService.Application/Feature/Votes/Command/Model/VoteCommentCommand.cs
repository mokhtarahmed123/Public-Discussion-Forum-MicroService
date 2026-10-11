using MediatR;
using System.Text.Json.Serialization;
using VotesService.Application.Bases;
using VotesService.Domain.Enum;

namespace VotesService.Application.Feature.Votes.Command.Model
{
    public class VoteCommentCommand
    : IRequest<Response<string>>
    {
        [JsonIgnore]
        public string TargetId { get; set; } = null!;
        [JsonIgnore]
        public string PostId { get; set; } = null!;
        public VoteType Type { get; init; }
        //[JsonIgnore]
        //public Guid UserId { get; set; }   // From JWT Token


    }
}

