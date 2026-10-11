using AutoMapper;
using PostsCommentsService.Application.Feature.Comments.Command.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Results;
using PostsCommentsService.Domain.Entities;

namespace PostsCommentsService.Application.Mapping.CommentMapping
{
    public partial class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateCommentMapping();
            GetCommentById();
            GetCommentsByPost();
            GetCommentsByUser();
            GetReplies();
        }

        private void CreateCommentMapping()
        {

            CreateMap<CreateCommentCommand, Domain.Entities.Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ParentCommentId, opt => opt.MapFrom(src => src.ParentCommentId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }


        private void GetCommentById()
        {
            CreateMap<Comment, GetCommentByIdResult>();
        }
        private void GetCommentsByPost()
        {
            CreateMap<Comment, GetCommentsByPostResult>();
        }
        private void GetCommentsByUser()
        {
            CreateMap<Comment, GetCommentsByUserResult>();
        }
        private void GetReplies()
        {
            CreateMap<Comment, GetRepliesResult>();
        }


    }
}