namespace PostsCommentsService.Application.Feature.Comments.Command.Validator
{
    using FluentValidation;
    using PostsCommentsService.Application.Feature.Comments.Command.Model;

    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Comment content is required.")
                .MaximumLength(2000).WithMessage("Comment content must not exceed 2000 characters.");



        }
    }
}
