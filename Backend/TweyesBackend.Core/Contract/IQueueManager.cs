using TweyesBackend.Core.DTO.Request;
using System.Diagnostics.CodeAnalysis;

namespace TweyesBackend.Core.Contract
{
    public interface IQueueManager
    {
        ValueTask<SendMailRequest> PullEmailAsync(CancellationToken cancellationToken);
        ValueTask PushEmailAsync([NotNull] SendMailRequest request);
    }
}
