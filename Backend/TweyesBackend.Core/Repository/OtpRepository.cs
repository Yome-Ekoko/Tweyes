using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.Contract.Repository;
using TweyesBackend.Core.Exceptions;

namespace TweyesBackend.Core.Repository
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IMemoryCache _cache;

        public OtpRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task StoreOtpAsync(string email, string otp, int validityPeriod = 3000)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                Console.WriteLine($"Invalid email or OTP. Email: {email}, OTP: {otp}");
                throw new ApiException("Invalid email or OTP.");
            }

            var expiryTime = TimeSpan.FromSeconds(validityPeriod);
            Console.WriteLine($"Storing OTP for email: {email}, OTP: {otp}, Expiry: {expiryTime}");
            _cache.Set(email, otp, expiryTime);

            return Task.CompletedTask;
        }

        public Task<bool> VerifyOtpAsync(string email, string otp)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                return Task.FromResult(false);
            }

            if (_cache.TryGetValue(email, out string storedOtp))
            {
                Console.WriteLine($"Stored OTP: {storedOtp}");
                if (storedOtp == otp)
                {
                    _cache.Remove(email);
                    return Task.FromResult(true);
                }
                else
                {
                    Console.WriteLine("OTP does not match.");
                }
            }
            else
            {
                Console.WriteLine("OTP not found in cache.");
            }
            return Task.FromResult(false);
        }
    

    }
}
