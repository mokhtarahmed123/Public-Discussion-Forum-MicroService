using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment content is required.")
            .MaximumLength(2000).WithMessage("Comment content must not exceed 2000 characters.");

        RuleFor(x => x.ParentCommentId)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("The parent comment ID is invalid.")
            .When(x => x.ParentCommentId is not null);

    }
}