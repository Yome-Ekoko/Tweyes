//using TweyesBackend.Core.DTO.Request;
//using TweyesBackend.Domain.Common;
//using TweyesBackend.Domain.Entities;

//namespace TweyesBackend.Core.Contract.Repository
//{
//    public interface IWithdrawLogRepository
//    {
//        Task<List<BankToBrokerWithdrawalLog>> GetByDateRange(GetLogsByDateReq request, CancellationToken cancellationToken = default);
//        Task<BankToBrokerWithdrawalLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
//        Task<PagedList<BankToBrokerWithdrawalLog>> GetPagedByDateRange(GetPagedTxnsByDateReq request, CancellationToken cancellationToken = default);
//    }
//}
