using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.Contract.Repository
{
    public interface IOtpRepository
    {
        Task StoreOtpAsync(string email, string otp, int validityPeriod = 300);
        Task<bool> VerifyOtpAsync(string email, string otp);
    }
}
