//using TweyesBackend.Core.DTO.Request;
//using TweyesBackend.Domain.Common;
//using TweyesBackend.Domain.Entities;

//namespace TweyesBackend.Core.Contract.Repository
//{
//    public interface IAccountLogRepository
//    {
//        Task<List<BankToBrokerLog>> GetByDateRange(GetLogsByDateReq request, CancellationToken cancellationToken = default);
//        Task<BankToBrokerLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
//        Task<PagedList<BankToBrokerLog>> GetPagedByDateRange(GetPagedTxnsByDateReq request, CancellationToken cancellationToken = default);
//    }
//}
