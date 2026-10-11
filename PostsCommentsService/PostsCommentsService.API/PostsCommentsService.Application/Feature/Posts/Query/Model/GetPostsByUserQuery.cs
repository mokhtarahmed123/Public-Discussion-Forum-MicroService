using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Results;

namespace PostsCommentsService.Application.Feature.Posts.Query.Model
{
    public record GetPostsByUserQuery(Guid userid) : IRequest<Response<List<GetPostsByUserResult>>>;


}
