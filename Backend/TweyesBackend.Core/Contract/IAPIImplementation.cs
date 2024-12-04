using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;

namespace TweyesBackend.Core.Contract
{
    public interface IAPIImplementation
    {
        Task<SendMailResponse> SendMail(SendMailRequest request);
    }
}
