namespace PostsCommentsService.Application.Feature.Comments.Command.Validator
{
    using FluentValidation;
    using PostsCommentsService.Application.Feature.Comments.Command.Model;

    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("الكومنت مطلوب.")
                .MaximumLength(2000).WithMessage("الكومنت لازم ميزيدش عن 2000 حرف.");

            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
