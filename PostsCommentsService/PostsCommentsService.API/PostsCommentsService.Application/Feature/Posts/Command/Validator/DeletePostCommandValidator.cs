using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("رقم البوست مطلوب.")
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("رقم البوست غير صحيح.");

        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}