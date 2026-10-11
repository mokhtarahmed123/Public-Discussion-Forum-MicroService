namespace PostsCommentsService.Domain
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string? Email { get; }
        IEnumerable<string> Roles { get; }
        string UserIdFromJWT { get; }
        public string? UserIdFromJWTWithNull { get; }
    }
}
