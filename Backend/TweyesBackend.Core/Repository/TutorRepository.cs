using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.Contract.Repository;
using TweyesBackend.Persistence;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Core.Extension;
using Microsoft.EntityFrameworkCore;

namespace TweyesBackend.Core.Repository
{
    public class TutorRepository : ITutorRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TutorRepository(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor
)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<List<Tutor>> GetAll()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                return await dbContext.Tutor.ToListAsync();
            }
        }

        public async Task<Tutor> GetById(string id)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var res = await dbContext.Tutor
                        .FirstOrDefaultAsync(u => u.Id == id) ?? throw new ApiException("Tutor not found");


                    return res;
                }

            }
            catch (Exception ex)
            {
                throw new ApiException();
            }
        }

        public async Task<Tutor> GetTutorAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("User not found.");
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var tutor = await dbContext.Tutor.FirstOrDefaultAsync(x => x.UserId == userId);


                return tutor;
            }
        }

        //public async Task<Tutor> GetTutor(string userId)
        //{
        //    using (var scope = _serviceScopeFactory.CreateScope())
        //    {
        //        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        //        var tutor = await dbContext.Tutor.FirstOrDefaultAsync(x => x.UserId == userId);

        //        if (tutor == null)
        //        {
        //            throw new ApiException("Tutor not found");
        //        }

        //        return tutor;
        //    }
        //}

        public async Task<Tutor> Add(Tutor tutor)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                if (tutor == null)
                {
                    throw new ApiException("No Tutor added");
                }

                var result = await dbContext.Tutor.AddAsync(tutor);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<Tutor> Update(Tutor tutor)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var result = dbContext.Tutor.Update(tutor);
                await dbContext.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task Delete(string id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var tutor = await dbContext.Tutor
                    .FirstOrDefaultAsync(u => u.Id == id) ?? throw new ApiException("Tutor not found");

                dbContext.Tutor.Remove(tutor);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

