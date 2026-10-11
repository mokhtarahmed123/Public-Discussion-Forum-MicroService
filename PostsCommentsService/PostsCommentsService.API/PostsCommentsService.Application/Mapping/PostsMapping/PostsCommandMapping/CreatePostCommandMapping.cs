using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Mapping.PostsMapping
{
    public partial class PostsProfile
    {
        private void Create()
        {
            CreateMap<CreatePostCommand, Post>()
           .ForMember(d => d.Id, o => o.Ignore())
      .ForMember(d => d.CreatedAt, o => o.Ignore())
       .ForMember(d => d.UpdatedAt, o => o.Ignore())
      .ForMember(d => d.LikesCount, o => o.Ignore())
      .ForMember(d => d.CommentsCount, o => o.Ignore())
          .ForMember(d => d.IsDeleted, o => o.Ignore())
      .ForMember(d => d.DeletedAt, o => o.Ignore());

        }
    }
}
