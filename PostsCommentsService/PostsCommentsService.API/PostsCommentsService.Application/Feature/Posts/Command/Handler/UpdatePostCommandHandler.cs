using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Command.Handler
{
    public class UpdatePostCommandHandler : ResponseHandler, IRequestHandler<UpdatePostCommand, Response<string>>
    {
        private readonly IPostsService postsService;

        public UpdatePostCommandHandler(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<Response<string>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {

            var post = await postsService.GetByIdAsync(request.Id, cancellationToken);

            if (post is null || post.IsDeleted)
                return NotFound<string>("البوست مش موجود.");
            if (post.IsLocked)
                return BadRequest<string>("البوست مقفول ومينفعش يتعدل.");


            if (post.UserId != request.UserId)
                return Forbidden<string>("مش مسموحلك تعدل البوست ده.");

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
