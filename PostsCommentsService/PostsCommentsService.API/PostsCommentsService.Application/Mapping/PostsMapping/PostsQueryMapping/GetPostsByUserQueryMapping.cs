using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Mapping.PostsMapping
{
    public partial class PostsProfile
    {
        private void GetByUser()
        {
            CreateMap<Post, GetPostsByUserResult>();
        }
    }
}
