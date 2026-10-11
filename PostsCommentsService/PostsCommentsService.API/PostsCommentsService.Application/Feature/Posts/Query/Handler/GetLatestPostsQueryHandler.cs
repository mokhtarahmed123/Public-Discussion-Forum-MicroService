using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Model;
using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Query.Handler
{
    public class GetLatestPostsQueryHandler : ResponseHandler, IRequestHandler<GetLatestPostsQuery, Response<List<GetLatestPostsResult>>>
    {
        private readonly IPostsService postsService;
        private readonly IMapper mapper;

        public GetLatestPostsQueryHandler(IPostsService postsService, IMapper mapper)
        {
            this.postsService = postsService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetLatestPostsResult>>> Handle(GetLatestPostsQuery request, CancellationToken cancellationToken)
        {
            var page = request.page < 1 ? 1 : request.page;
            var pageSize = Math.Clamp(request.pageSize, 1, 50);

            var posts = await postsService.GetLatestAsync(page, pageSize, cancellationToken);

            var result = mapper.Map<List<GetLatestPostsResult>>(posts);

            return Success(result);
        }
    }
}