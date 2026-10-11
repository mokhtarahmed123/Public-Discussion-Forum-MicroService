using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;

namespace PostsCommentsService.Application.Feature.Posts.Command.Handler
{
    public class UpdatePostCommandHandler : ResponseHandler, IRequestHandler<UpdatePostCommand, Response<string>>
    {
        private readonly IPostsService postsService;
        private readonly ICurrentUserService currentUserService;

        public UpdatePostCommandHandler(IPostsService postsService, ICurrentUserService currentUserService)
        {
            this.postsService = postsService;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<string>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var post = await postsService.GetByIdAsync(request.Id, cancellationToken);

            if (post is null || post.IsDeleted)
                return NotFound<string>("The post was not found.");

            if (post.IsLocked)
                return BadRequest<string>("This post is locked and cannot be edited.");

            if (post.UserId.ToString() != userId)
                return Forbidden<string>("You are not allowed to edit this post.");

            post.Title = request.Title;
            post.Content = request.Content;
            post.TypeOfPosts = request.TypeOfPosts;
            post.IsLocked = request.IsLocked;
            post.UpdatedAt = DateTime.UtcNow;

            await postsService.UpdateAsync(post, cancellationToken);


            return Success(post.Id);
        }
    }
}
