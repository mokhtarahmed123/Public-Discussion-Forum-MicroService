using AutoMapper;
using MediatR;
using MongoDB.Bson;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Posts.Query.Model;
using PostsCommentsService.Application.Feature.Posts.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Posts.Query.Handler
{
    public class GetPostByIdQueryHandler : ResponseHandler, IRequestHandler<GetPostByIdQuery, Response<GetPostByIdResult>>
    {
        private readonly IPostsService postsService;
        private readonly IMapper mapper;

        public GetPostByIdQueryHandler(IPostsService postsService, IMapper mapper)
        {
            this.postsService = postsService;
            this.mapper = mapper;
        }

        public async Task<Response<GetPostByIdResult>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Id))
                return BadRequest<GetPostByIdResult>("رقم البوست مطلوب.");

            if (!ObjectId.TryParse(request.Id, out _))
                return BadRequest<GetPostByIdResult>("رقم البوست غير صحيح.");

            var post = await postsService.GetByIdAsync(request.Id, cancellationToken);

            if (post is null || post.IsDeleted)
                return NotFound<GetPostByIdResult>("البوست مش موجود.");

            var result = mapper.Map<GetPostByIdResult>(post);

            return Success(result);
        }
    }
}