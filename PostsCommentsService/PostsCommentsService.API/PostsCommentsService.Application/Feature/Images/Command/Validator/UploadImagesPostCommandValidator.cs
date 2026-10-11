using FluentValidation;
using MongoDB.Bson;
using PostsCommentsService.Application.Feature.Images.Command.Model;

namespace PostsCommentsService.Application.Feature.Images.Command.Validator
{
    public class UploadImagesPostCommandValidator : AbstractValidator<UploadImagesPostCommand>
    {
        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg", "image/png", "image/webp"
        };

        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private const int MaxFilesCount = 30;
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public UploadImagesPostCommandValidator()
        {
            RuleFor(x => x.PostId)
              .NotEmpty().WithMessage("PostId is required.")
            .Must(BeValidObjectId).WithMessage("PostId is not a valid id.");

            RuleFor(x => x.Files)
                .NotNull().WithMessage("Files are required.")
                .NotEmpty().WithMessage("At least one image is required.")
                .Must(f => f.Count <= MaxFilesCount)
                    .WithMessage($"Maximum {MaxFilesCount} images per upload.");

            RuleForEach(x => x.Files).ChildRules(file =>
            {
                file.RuleFor(f => f.Length)
                    .GreaterThan(0).WithMessage("File is empty.")
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                        .WithMessage("File exceeds the 5MB limit.");

                file.RuleFor(f => f.ContentType)
                    .Must(ct => AllowedContentTypes.Contains(ct))
                    .WithMessage("Unsupported file type. Allowed: jpeg, png, webp.");

                file.RuleFor(f => f.FileName)
                    .Must(name => AllowedExtensions.Contains(
                        Path.GetExtension(name).ToLowerInvariant()))
                    .WithMessage("Unsupported file extension.");
            });
        }

        private static bool BeValidObjectId(string id)
            => ObjectId.TryParse(id, out _);
    }
}