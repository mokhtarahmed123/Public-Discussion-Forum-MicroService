using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.ExternalApiService.UserServiceInterface;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Feature.Comments.Command.Handler
{
    public class CreateCommentCommandHandler : ResponseHandler, IRequestHandler<CreateCommentCommand, Response<string>>
    {
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;
        private readonly IMapper mapper;
        private readonly IUserClient userClient;

        public CreateCommentCommandHandler(
            ICommentsService commentsService,
            IPostsService postsService,
            IMapper mapper,
            IUserClient userClient)
        {
            this.commentsService = commentsService;
            this.postsService = postsService;
            this.mapper = mapper;
            this.userClient = userClient;
        }

        public async Task<Response<string>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {

            var post = await postsService.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null || post.IsDeleted)
                return NotFound<string>("البوست مش موجود.");


            if (post.IsLocked)
                return BadRequest<string>("التعليقات مقفولة على البوست ده.");


            Comment? parent = null;
            if (request.ParentCommentId is not null)
            {
                parent = await commentsService.GetByIdAsync(request.ParentCommentId, request.PostId, cancellationToken);

                if (parent is null || parent.IsDeleted || parent.PostId != request.PostId)
                    return NotFound<string>("الكومنت الأب مش موجود.");
            }


            var user = await userClient.GetUserAsync(request.UserId, cancellationToken);
            if (user is null || user.Id == Guid.Empty)
                return NotFound<string>("المستخدم مش موجود.");


            var comment = mapper.Map<Comment>(request);
            comment.UserId = request.UserId;


            var created = await commentsService.AddAsync(comment, cancellationToken);


            await postsService.IncrementCommentsCountAsync(request.PostId, 1, cancellationToken);

            if (parent is not null)
                await commentsService.IncrementRepliesCountAsync(parent.Id, 1, cancellationToken);

            return Success(created.Id);
        }
    }
}