using AutoMapper;
using MediatR;
using PostsCommentsService.Application.Bases;
using PostsCommentsService.Application.Feature.Comments.Query.Model;
using PostsCommentsService.Application.Feature.Comments.Query.Results;
using PostsCommentsService.Application.ServiceInterface;

namespace PostsCommentsService.Application.Feature.Comments.Query.Handler
{
    public class GetCommentsByUserQueryHandler : ResponseHandler,
        IRequestHandler<GetCommentsByUserQuery, Response<List<GetCommentsByUserResult>>>
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public GetCommentsByUserQueryHandler(ICommentsService commentsService, IMapper mapper)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetCommentsByUserResult>>> Handle(
            GetCommentsByUserQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return BadRequest<List<GetCommentsByUserResult>>("رقم المستخدم غير صحيح.");

            var comments = await commentsService.GetByUserAsync(request.UserId, cancellationToken);

            var result = mapper.Map<List<GetCommentsByUserResult>>(comments);

            return Success(result);
        }
    }
}