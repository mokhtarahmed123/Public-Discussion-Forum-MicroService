using AutoMapper;
using MediatR;
using MongoDB.Bson;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Query.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Comments.Query.Handler
{
    public class GetCommentsByPostQueryHandler : ResponseHandler,
        IRequestHandler<GetCommentsByPostQuery, Response<List<GetCommentsByPostResult>>>
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public GetCommentsByPostQueryHandler(ICommentsService commentsService, IMapper mapper)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetCommentsByPostResult>>> Handle(
            GetCommentsByPostQuery request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.PostId, out _))
                return BadRequest<List<GetCommentsByPostResult>>("رقم البوست غير صحيح.");

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = Math.Clamp(request.PageSize, 1, 50);

            var comments = await commentsService.GetByPostAsync(request.PostId, page, pageSize, cancellationToken);

            var result = mapper.Map<List<GetCommentsByPostResult>>(comments);

            return Success(result);
        }
    }
}