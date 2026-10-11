using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.ServiceInterface;
using PostsCommentsService.Domain;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Feature.Comments.Command.Handler
{
    public class CreateCommentCommandHandler : ResponseHandler, IRequestHandler<CreateCommentCommand, Response<string>>
    {
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;



        public CreateCommentCommandHandler(
            ICommentsService commentsService,
            IPostsService postsService,
            IMapper mapper, ICurrentUserService currentUserService
            )
        {
            this.commentsService = commentsService;
            this.postsService = postsService;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<string>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {

            var post = await postsService.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null || post.IsDeleted)
                return NotFound<string>("The post was not found.");


            if (post.IsLocked)
                return BadRequest<string>("Comments are disabled for this post.");


            Comment? parent = null;
            if (request.ParentCommentId is not null)
            {
                parent = await commentsService.GetByIdAsync(request.ParentCommentId, request.PostId, cancellationToken);

                if (parent is null || parent.IsDeleted || parent.PostId != request.PostId)
                    return NotFound<string>("The parent comment was not found.");
            }

            var userId = currentUserService.UserId;
            if (userId is null)
                return NotFound<string>("The current user was not found.");

            var comment = mapper.Map<Comment>(request);
            comment.UserId = Guid.Parse(userId);


            var created = await commentsService.AddAsync(comment, cancellationToken);


            await postsService.IncrementCommentsCountAsync(request.PostId, 1, cancellationToken);

            if (parent is not null)
                await commentsService.IncrementRepliesCountAsync(parent.Id, 1, cancellationToken);

            return Success(created.Id);
        }
    }
}