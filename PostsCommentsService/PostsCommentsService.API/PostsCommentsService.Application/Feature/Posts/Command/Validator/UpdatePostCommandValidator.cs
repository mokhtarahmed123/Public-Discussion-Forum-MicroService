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
                .NotEmpty().WithMessage("Post ID is required.")
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("The post ID is invalid.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Post title is required.")
                .MaximumLength(200)
                .WithMessage("Post title must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required.")
                .MaximumLength(10000)
                .WithMessage("Post content must not exceed 10000 characters.");

            RuleFor(x => x.TypeOfPosts)
                .IsInEnum()
                .WithMessage("The post type is invalid.");
        }
    }
}
