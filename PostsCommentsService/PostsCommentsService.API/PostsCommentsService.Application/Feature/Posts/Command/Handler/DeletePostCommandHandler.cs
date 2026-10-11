using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Command.Handler
{
    public class DeletePostCommandHandler : ResponseHandler, IRequestHandler<DeletePostCommand, Response<string>>
    {
        private readonly IPostsService postsService;


        public DeletePostCommandHandler(IPostsService postsService)
        {
            this.postsService = postsService;

        }

        public async Task<Response<string>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {


            var post = await postsService.GetByIdAsync(request.Id, cancellationToken);

            if (post is null || post.IsDeleted)
                return NotFound<string>("البوست مش موجود.");

            if (post.UserId != request.UserId)
                return Forbidden<string>("مش مسموحلك تمسح البوست ده.");


            post.IsDeleted = true;
            post.DeletedAt = DateTime.UtcNow;

            await postsService.UpdateAsync(post, cancellationToken);

            return Deleted<string>();
        }
    }
}