using FluentValidation;
using MongoDB.Bson;
using VotesService.Application.Feature.Votes.Command.Model;

namespace VotesService.Application.Feature.Votes.Command.Validation
{
    public class VoteCommentCommandValidation : AbstractValidator<VoteCommentCommand>
    {
        public VoteCommentCommandValidation()
        {
            RuleFor(x => x.TargetId)
                .NotEmpty().WithMessage("رقم العنصر مطلوب.")
                .Must(id => ObjectId.TryParse(id, out _)).WithMessage("رقم العنصر غير صحيح.");

            RuleFor(x => x.Type).IsInEnum();

        }
    }
}
