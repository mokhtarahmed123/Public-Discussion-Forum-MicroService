using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;

namespace PostsCommentsService.Application.Feature.Posts.Command.Handler
{
    public class DeletePostCommandHandler : ResponseHandler, IRequestHandler<DeletePostCommand, Response<string>>
    {
        private readonly IPostsService postsService;
        private readonly ICurrentUserService currentUserService;

        public DeletePostCommandHandler(IPostsService postsService, ICurrentUserService currentUserService)
        {
            this.postsService = postsService;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<string>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {

            var userId = currentUserService.UserId;
            var post = await postsService.GetByIdAsync(request.Id, cancellationToken);

            if (post is null || post.IsDeleted)
                return NotFound<string>("The post was not found.");


            if (post.UserId.ToString() != userId)
                return Forbidden<string>("You are not allowed to delete this post.");


            post.IsDeleted = true;
            post.DeletedAt = DateTime.UtcNow;

            await postsService.UpdateAsync(post, cancellationToken);

            return Deleted<string>();
        }
    }
}