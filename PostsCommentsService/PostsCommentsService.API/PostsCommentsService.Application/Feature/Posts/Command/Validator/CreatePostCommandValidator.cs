using FluentValidation;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

namespace PostsCommentsService.Application.Feature.Comments.Command.Validator
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Post title is required.")
                .MaximumLength(200).WithMessage("Post title must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required.")
                .MaximumLength(10000).WithMessage("Post content must not exceed 10000 characters.");

            RuleFor(x => x.TypeOfPosts)
                .IsInEnum().WithMessage("The post type is invalid.");
        }
    }
}
