using TweyesBackend.Core.Contract;
using TweyesBackend.Domain.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TweyesBackend.Core.BackgroundServices
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly IQueueManager _queue;
        private readonly IAPIImplementation _apiImplementation;

        public EmailBackgroundService(ILogger<EmailBackgroundService> logger,
          IQueueManager queue,
          IAPIImplementation apiImplementation)
        {
            _logger = logger;
            _queue = queue;
            _apiImplementation = apiImplementation;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var emailRequest = await _queue.PullEmailAsync(stoppingToken);

                    _logger.LogInformation($"Processing email service for {emailRequest.subject} to {emailRequest.to}");

                    var response = await _apiImplementation.SendMail(emailRequest);

                    _logger.LogInformation("Email sent to {recipient} with response {response}", emailRequest.to, CoreHelpers.ClassToJsonData(response));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failure while processing queue");
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Background Service");
            await base.StopAsync(cancellationToken);
        }
    }
}