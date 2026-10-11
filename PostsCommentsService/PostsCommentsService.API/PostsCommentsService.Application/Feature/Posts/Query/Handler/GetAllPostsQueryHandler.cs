using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Model;
using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Query.Handler
{
    public class GetAllPostsQueryHandler :
        ResponseHandler, IRequestHandler<GetAllPostsQuery, Response<List<GetAllPostsResult>>>
    {
        private readonly IPostsService postsService;
        private readonly IMapper mapper;

        public GetAllPostsQueryHandler(IPostsService postsService, IMapper mapper)
        {
            this.postsService = postsService;
            this.mapper = mapper;
        }
        public async Task<Response<List<GetAllPostsResult>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await postsService.GetAll(cancellationToken);

            var result = mapper.Map<List<GetAllPostsResult>>(posts);

            return Success(result);
        }
    }
}
