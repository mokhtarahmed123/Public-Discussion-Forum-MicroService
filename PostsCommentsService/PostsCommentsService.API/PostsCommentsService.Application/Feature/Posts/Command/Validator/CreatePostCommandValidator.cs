using FluentValidation;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

namespace PostsCommentsService.Application.Feature.Comments.Command.Validator
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
<<<<<<< HEAD
                .NotEmpty().WithMessage("Post title is required.")
                .MaximumLength(200).WithMessage("Post title must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required.")
                .MaximumLength(10000).WithMessage("Post content must not exceed 10000 characters.");

            RuleFor(x => x.TypeOfPosts)
                .IsInEnum().WithMessage("The post type is invalid.");
=======
                .NotEmpty().WithMessage("العنوان مطلوب.")
                .MaximumLength(200).WithMessage("العنوان لازم ميزيدش عن 200 حرف.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("المحتوى مطلوب.")
                .MaximumLength(10000).WithMessage("المحتوى لازم ميزيدش عن 10000 حرف.");

            RuleFor(x => x.TypeOfPosts)
                .IsInEnum().WithMessage("نوع البوست غير صحيح.");

            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty).WithMessage("المستخدم غير معروف.");
>>>>>>> 6ce00c0b2538e159445141615b97fd4ad1221beb
        }
    }
}
