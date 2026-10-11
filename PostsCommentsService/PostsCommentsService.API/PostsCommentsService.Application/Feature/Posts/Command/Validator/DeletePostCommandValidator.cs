using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Comments.Command.Model;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Post ID is required.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("The post ID is invalid.");
    }
}