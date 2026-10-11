using FluentValidation;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

namespace PostsCommentsService.Application.Feature.Comments.Command.Validator
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("العنوان مطلوب.")
                .MaximumLength(200).WithMessage("العنوان لازم ميزيدش عن 200 حرف.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("المحتوى مطلوب.")
                .MaximumLength(10000).WithMessage("المحتوى لازم ميزيدش عن 10000 حرف.");

            RuleFor(x => x.TypeOfPosts)
                .IsInEnum().WithMessage("نوع البوست غير صحيح.");

            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty).WithMessage("المستخدم غير معروف.");
        }
    }
}
