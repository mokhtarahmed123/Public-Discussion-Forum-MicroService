using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

namespace PostsCommentsService.Application.Feature.Posts.Command.Validator
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("رقم البوست مطلوب.")
                .Must(id => ObjectId.TryParse(id, out _)).WithMessage("رقم البوست غير صحيح.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("العنوان مطلوب.")
                .MaximumLength(200);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("المحتوى مطلوب.")
                .MaximumLength(10000);

            RuleFor(x => x.TypeOfPosts).IsInEnum();
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
