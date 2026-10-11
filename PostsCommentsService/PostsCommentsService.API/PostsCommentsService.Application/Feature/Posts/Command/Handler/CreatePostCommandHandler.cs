using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Feature.Posts.Command
{
    public class CreatePostCommandHandler : ResponseHandler, IRequestHandler<CreatePostCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IPostsService postsService;
        private readonly ICurrentUserService currentUserService;



        public CreatePostCommandHandler(IMapper mapper, IPostsService postsService, ICurrentUserService currentUserService)
        {
            this.mapper = mapper;
            this.postsService = postsService;
            this.currentUserService = currentUserService;

        }
        public async Task<Response<string>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {

            var userId = currentUserService.UserId;
            if (userId == null)
            {
                return NotFound<string>("The user was not found.");
            }
            var post = mapper.Map<Post>(request);
            post.UserId = Guid.Parse(userId);
            var created = await postsService.AddAsync(post, cancellationToken);

            return Success(created.Id);

        }
    }
}
