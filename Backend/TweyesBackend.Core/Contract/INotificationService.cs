using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;

namespace TweyesBackend.Core.Contract
{
    public interface INotificationService
    {
        Task SendPasswordResetToken(string userName, string url, string firstName, string email);
        Task<SendMailResponse> SendOtp(SendOtpRequest request);
        Task<SendMailResponse> SendWelcomeMail(WelcomeRequest request);
    }
}
