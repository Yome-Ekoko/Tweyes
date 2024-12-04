using TweyesBackend.Core.Contract;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Domain.Settings;
using Microsoft.Extensions.Options;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Core.Extension;

namespace TweyesBackend.Core.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly AdminOptions _adminOptions;
        private readonly ITemplateService _templateService;
        private readonly IQueueManager _queueManager;
        private readonly IAPIImplementation _apiImplementation;

        public NotificationService(IOptions<AdminOptions> adminOptions,
            ITemplateService templateService,
            IQueueManager queueManager,
            IAPIImplementation apiImplementation)
        {
            _adminOptions = adminOptions.Value;
            _templateService = templateService;
            _queueManager = queueManager;
            _apiImplementation = apiImplementation;
        }

        /**
         * GENERAL COMMENTS ON THIS SERVICE
         * 
        **/
        public async Task SendPasswordResetToken(string userName, string url, string firstName, string email)
        {
            WelcomeEmailTemplateRequest templateRequest = new WelcomeEmailTemplateRequest()
            {
                UserName = userName,
                Name = firstName,
                Url = url
            };
            var emailRequest = new SendMailRequest()
            {
                from = _adminOptions.BroadcastEmail,
                to = email,
                subject = "Coronation Portal Password Reset Token",
                mailMessage = _templateService.GenerateHtmlStringFromViewsAsync("WelcomeNotification", templateRequest)
            };

            await _queueManager.PushEmailAsync(emailRequest);
        }
        public async Task<SendMailResponse> SendOtp(SendOtpRequest request)
        {
            var emailRequest = new SendMailRequest()
            {
                from = _adminOptions.BroadcastEmail,
                to = request.email,
                subject = "CarPooling  Otp ",
                mailMessage = EmailTemplates.GetOtp(request.email, request.Otp, request.firstname)
            };


            return await _apiImplementation.SendMail(emailRequest);
        }
        public async Task<SendMailResponse> SendWelcomeMail(WelcomeRequest request)
        {
            var emailRequest = new SendMailRequest()
            {
                from = _adminOptions.BroadcastEmail,
                to = request.Email,
                subject = "CarPooling  Password Reset Token",
                mailMessage = EmailTemplates.GetWelcomeEmail(request.Email, request.FirstName)
            };


            return await _apiImplementation.SendMail(emailRequest);
        }
    }
}
