using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;

namespace PostsCommentsService.Application.Feature.Comments.Command.Handler
{
    public class UpdateCommentCommandHandler : ResponseHandler, IRequestHandler<UpdateCommentCommand, Response<string>>
    {
        private const string DeletedContent = "[deleted]";

        private readonly ICommentsService commentsService;
        private readonly ICurrentUserService currentUserService;
        private readonly IPostsService postsService;

        public UpdateCommentCommandHandler(ICommentsService commentsService, ICurrentUserService currentUserService, IPostsService postsService)
        {
            this.commentsService = commentsService;
            this.currentUserService = currentUserService;
            this.postsService = postsService;
        }

        public async Task<Response<string>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var post = await postsService.GetByIdAsync(request.PostId, cancellationToken);
            if (post.IsDeleted || post.IsLocked || post is null)
            {
                return BadRequest<string>(
                    "This post is no longer available for modification."
                );
            }
            var comment = await commentsService.GetByIdAsync(request.Id, request.PostId, cancellationToken);


            if (comment is null || comment.IsDeleted || comment.Content == DeletedContent)
                return NotFound<string>("The comment was not found.");

            if (comment.UserId.ToString() != userId)
                return Forbidden<string>("You are not allowed to edit this comment.");

            comment.Content = request.Content;
            comment.IsEdited = true;
            comment.UpdatedAt = DateTime.UtcNow;

            await commentsService.UpdateAsync(comment, cancellationToken);

            return Success(comment.Id);
        }
    }
}