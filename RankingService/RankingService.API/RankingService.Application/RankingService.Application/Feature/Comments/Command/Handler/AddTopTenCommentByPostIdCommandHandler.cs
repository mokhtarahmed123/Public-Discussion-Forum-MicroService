using AutoMapper;
using MediatR;
using RankingService.Application.Bases;
using RankingService.Application.EventInterface;
using RankingService.Application.ExternalApiService.CommentServiceInterface;
using RankingService.Application.Feature.Comments.Command.Model;
using RankingService.Application.ServiceInterface;
using RankingService.Domain.Entities;

namespace RankingService.Application.Feature.Comments.Command.Handler
{
    public class AddTopTenCommentByPostIdCommandHandler : ResponseHandler, IRequestHandler<AddTopTenCommentByPostIdCommand, Response<string>>
    {
        private readonly ICommentClient commentClient;
        private readonly IMapper mapper;
        private readonly ITopTenCommentsService topTenCommentsService;
        private readonly ICommentScoreService commentScoreService;

        public AddTopTenCommentByPostIdCommandHandler(ICommentClient commentClient,
    IMapper mapper,
    ITopTenCommentsService topTenCommentsService, ICommentScoreService commentScoreService)
        {
            this.commentClient = commentClient;
            this.mapper = mapper;
            this.topTenCommentsService = topTenCommentsService;
            this.commentScoreService = commentScoreService;
        }
        public async Task<Response<string>> Handle(AddTopTenCommentByPostIdCommand request, CancellationToken cancellationToken)
        {
            var scores = await commentScoreService.GetTopAsync(request.PostId, 20, cancellationToken);
            var scoreById = scores.ToDictionary(s => s.CommentId, s => s.Score);

            var comments = await commentClient.GetCommentsByPostIdAsync(
        request.PostId, request.PageNumber, request.PageSize, cancellationToken);

            var valid = comments.Where(c => !c.IsDeleted).ToList();

            if (valid.Count == 0)
                return NotFound<string>("مفيش كومنتات للبوست ده.");


            var topTen = valid
                .Select(c =>
                {
                    var item = mapper.Map<TopTenComments>(c);
                    item.Score = scoreById.GetValueOrDefault(c.Id, 0);
                    return item;
                })
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select((x, i) => { x.Rank = i + 1; return x; })
                .ToList();

            await topTenCommentsService.ReplaceForPost(request.PostId, topTen, cancellationToken);

            return Success("تم تحديث أفضل الكومنتات للبوست.");
        }
    }
}
