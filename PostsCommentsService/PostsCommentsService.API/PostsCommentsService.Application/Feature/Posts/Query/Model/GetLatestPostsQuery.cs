using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Results;

namespace PostsCommentsService.Application.Feature.Posts.Query.Model
{
    public record GetLatestPostsQuery(int page, int pageSize) : IRequest<Response<List<GetLatestPostsResult>>>
;
}
