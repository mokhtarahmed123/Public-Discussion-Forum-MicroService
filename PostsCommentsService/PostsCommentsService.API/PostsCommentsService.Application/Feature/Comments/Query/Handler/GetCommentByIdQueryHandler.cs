using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Query.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Comments.Query.Handler
{
    public class GetCommentByIdQueryHandler : ResponseHandler, IRequestHandler<GetCommentByIdQuery, Response<GetCommentByIdResult>>
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public GetCommentByIdQueryHandler(ICommentsService commentsService, IMapper mapper)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        public async Task<Response<GetCommentByIdResult>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await commentsService.GetByIdAsync(request.CommentId, request.PostId, cancellationToken);

            if (comment is null || comment.IsDeleted)
                return NotFound<GetCommentByIdResult>("الكومنت مش موجود.");

            var result = mapper.Map<GetCommentByIdResult>(comment);

            return Success(result);
        }
    }
}