using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.QueryParameters;

namespace TweyesBackend.Core.Contract
{
    public interface IWithdrawLogService
    {
        Task<byte[]> DownloadLogs(DownloadLogsRequest request, CancellationToken cancellationToken);
        Task<PagedResponse<List<WithdrawLogResponse>>> GetLogs(LogQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<WithdrawLogResponse>> GetLogById(long id, CancellationToken cancellationToken);
    }
}
