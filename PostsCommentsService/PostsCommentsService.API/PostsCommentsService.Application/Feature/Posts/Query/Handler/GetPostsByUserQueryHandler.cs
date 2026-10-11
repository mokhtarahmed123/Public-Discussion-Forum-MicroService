using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Model;
using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Query.Handler
{
    public class GetPostsByUserQueryHandler : ResponseHandler,
        IRequestHandler<GetPostsByUserQuery, Response<List<GetPostsByUserResult>>>
    {
        private readonly IPostsService postsService;
        private readonly IMapper mapper;

        public GetPostsByUserQueryHandler(IPostsService postsService, IMapper mapper)
        {
            this.postsService = postsService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetPostsByUserResult>>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
        {
            if (request.userid == Guid.Empty)
                return BadRequest<List<GetPostsByUserResult>>("رقم المستخدم غير صحيح.");

            var posts = await postsService.GetByUserAsync(request.userid, cancellationToken);

            var result = mapper.Map<List<GetPostsByUserResult>>(posts);

            return Success(result);
        }
    }
}