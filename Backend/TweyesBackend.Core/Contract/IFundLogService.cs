using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.QueryParameters;

namespace TweyesBackend.Core.Contract
{
    public interface IFundLogService
    {
        Task<byte[]> DownloadLogs(DownloadLogsRequest request, CancellationToken cancellationToken);
        Task<PagedResponse<List<FundLogResponse>>> GetLogs(LogQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<FundLogResponse>> GetLogById(long id, CancellationToken cancellationToken);
    }
}
