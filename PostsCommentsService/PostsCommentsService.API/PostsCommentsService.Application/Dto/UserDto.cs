namespace PostsCommentsService.Application.Dto
{
    public record UserDto(
        Guid Id,
        string? UserName,
        string? Email);
    public record AuthResponse<T>(bool Succeeded, int StatusCode, string? Message, T? Data);

}
