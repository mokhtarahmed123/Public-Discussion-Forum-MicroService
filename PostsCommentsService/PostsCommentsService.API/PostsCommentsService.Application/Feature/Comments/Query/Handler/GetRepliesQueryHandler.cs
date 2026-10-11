using AutoMapper;
using MediatR;
using MongoDB.Bson;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Query.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Comments.Query.Handler
{
    public class GetRepliesQueryHandler : ResponseHandler,
        IRequestHandler<GetRepliesCommentsQuery, Response<List<GetRepliesResult>>>
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public GetRepliesQueryHandler(ICommentsService commentsService, IMapper mapper)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetRepliesResult>>> Handle(
            GetRepliesCommentsQuery request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.ParentCommentId, out _))
                return BadRequest<List<GetRepliesResult>>("رقم الكومنت غير صحيح.");

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = Math.Clamp(request.PageSize, 1, 50);

            var replies = await commentsService.GetRepliesAsync(request.ParentCommentId, page, pageSize, cancellationToken);

            var result = mapper.Map<List<GetRepliesResult>>(replies);

            return Success(result);
        }
    }
}