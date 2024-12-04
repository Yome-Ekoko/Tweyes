using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.QueryParameters;

namespace TweyesBackend.Core.Contract
{
    public interface IAccountLogService
    {
        Task<byte[]> DownloadLogs(DownloadLogsRequest request, CancellationToken cancellationToken);
        Task<PagedResponse<List<AccountLogResponse>>> GetLogs(LogQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<AccountLogResponse>> GetLogById(long id, CancellationToken cancellationToken);
    }
}
