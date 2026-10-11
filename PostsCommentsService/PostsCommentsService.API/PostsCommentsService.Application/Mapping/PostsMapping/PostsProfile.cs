using AutoMapper;
using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Mapping.PostsMapping
{
    public partial class PostsProfile : Profile
    {
        public PostsProfile()
        {
            Create();
            GetByUser();
            GetAll();
            GetById();
            CreateMap<Post, GetLatestPostsResult>();
        }
    }
}
