//using System.Net;
//using System.Net.Http.Json;
//using VotesService.Application.dtos;
//using VotesService.Application.ExternalApiService.UserServiceInterface;

//namespace VotesService.Infrastructure.Clients.UserClientImplementaion
//{
//    public class UserClient : IUserClient
//    {
//        private readonly HttpClient _httpClient;

//        public UserClient(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
//        {
//            using var response = await _httpClient.GetAsync(
//$"api/Authentication/GetUserById/{userId}", cancellationToken);


//            if (response.StatusCode == HttpStatusCode.NotFound)
//                return null;

//            response.EnsureSuccessStatusCode();


//            var wrapper = await response.Content.ReadFromJsonAsync<AuthResponse<UserDto>>(cancellationToken: cancellationToken);

//            return wrapper?.Data;
//        }
//    }
//}
