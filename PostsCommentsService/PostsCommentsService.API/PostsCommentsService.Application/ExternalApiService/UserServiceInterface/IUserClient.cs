using PostsCommentsService.Application.Dto;

namespace PostsCommentsService.Application.ExternalApiService.UserServiceInterface
{
    public interface IUserClient
    {
        Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}
