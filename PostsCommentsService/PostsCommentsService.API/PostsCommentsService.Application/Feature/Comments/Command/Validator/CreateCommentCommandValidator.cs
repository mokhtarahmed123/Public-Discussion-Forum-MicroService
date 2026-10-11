using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        //RuleFor(x => x.PostId)
        //    .NotEmpty().WithMessage("رقم البوست مطلوب.")
        //    .Must(id => ObjectId.TryParse(id, out _)).WithMessage("رقم البوست غير صحيح.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("الكومنت مطلوب.")
            .MaximumLength(2000).WithMessage("الكومنت لازم ميزيدش عن 2000 حرف.");

        RuleFor(x => x.ParentCommentId)
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("رقم الكومنت الأب غير صحيح.")
            .When(x => x.ParentCommentId is not null);

        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}