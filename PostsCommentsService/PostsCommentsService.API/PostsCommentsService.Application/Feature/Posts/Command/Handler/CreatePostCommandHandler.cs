using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.ExternalApiService.UserServiceInterface;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Feature.Posts.Command
{
    public class CreatePostCommandHandler : ResponseHandler, IRequestHandler<CreatePostCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IPostsService postsService;
        private readonly IUserClient userClient;

        public CreatePostCommandHandler(IMapper mapper, IPostsService postsService, IUserClient userClient)
        {
            this.mapper = mapper;
            this.postsService = postsService;
            this.userClient = userClient;
        }
        public async Task<Response<string>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var user = await userClient.GetUserAsync(request.UserId, cancellationToken);

            if (user is null)
                return NotFound<string>("المستخدم مش موجود.");

            var post = mapper.Map<Post>(request);   // UserId جاي من الـ Command (من التوكن)

            var created = await postsService.AddAsync(post, cancellationToken);

            return Success(created.Id);

        }
    }
}
