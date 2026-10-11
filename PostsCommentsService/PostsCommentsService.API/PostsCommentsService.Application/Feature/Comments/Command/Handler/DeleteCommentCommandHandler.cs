using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;

namespace PostsCommentsService.Application.Feature.Comments.Command.Handler
{
    public class DeleteCommentCommandHandler : ResponseHandler, IRequestHandler<DeleteCommentCommand, Response<string>>
    {
        private const string DeletedContent = "[deleted]";
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;
        private readonly ICurrentUserService currentUserService;

        public DeleteCommentCommandHandler(ICommentsService commentsService, IPostsService postsService, ICurrentUserService currentUserService)
        {
            this.commentsService = commentsService;
            this.postsService = postsService;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;


            var comment = await commentsService.GetByIdAsync(request.CommentId, request.PostId, cancellationToken);

            if (comment is null || comment.IsDeleted || comment.Content == DeletedContent)
                return NotFound<string>("The comment was not found.");


            if (comment.UserId.ToString() != userId)
                return Forbidden<string>("You are not allowed to delete this comment.");

            if (comment.RepliesCount > 0)
            {
                comment.Content = DeletedContent;
                comment.UpdatedAt = DateTime.UtcNow;
                await commentsService.UpdateAsync(comment, cancellationToken);
                return Deleted<string>();
            }


            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;
            await commentsService.UpdateAsync(comment, cancellationToken);


            await postsService.IncrementCommentsCountAsync(comment.PostId, -1, cancellationToken);

            if (comment.ParentCommentId is not null)
                await commentsService.IncrementRepliesCountAsync(comment.ParentCommentId, -1, cancellationToken);

            return Deleted<string>();
        }
    }
}