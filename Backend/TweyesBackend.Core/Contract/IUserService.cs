using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.QueryParameters;

namespace TweyesBackend.Core.Contract
{
    public interface IUserService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken);
        Task<Response<string>> LogoutAsync();
        Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken);
        Task<Response<string>> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> ResetUserAsync(ResetUserRequest request);
        Task<Response<string>> ChangePasswordWithToken(ChangePasswordRequest request);
        Task<Response<string>> ResetUserLockoutAsync(ResetUserRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        Task<Response<string>> SendOtp(OtpRequest request);
        Task<Response<string>> VerifyOtp(VerifyOtpRequest request);
        Task<Response<UserResponse>> CreatePassword(CreatePasswordRequest request);
    }
}
