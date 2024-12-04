using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.Contract.Repository;
using TweyesBackend.Core.Exceptions;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Persistence;

namespace TweyesBackend.Core.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScheduleRepository(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Schedule>> GetAll()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                return await dbContext.Schedules.ToListAsync();
            }
        }

        public async Task<Schedule> GetById(string id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                return await dbContext.Schedules.FirstOrDefaultAsync(s => s.Id == id)
                    ?? throw new ApiException("Schedule not found");
            }
        }

        public async Task<List<Tutor>> GetAllTutorScheduleAsync(string tutorId, string day, string time, string time1)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                // Fetch all tutors first
                var tutors = await dbContext.Tutor
                    .Where(t => t.Id == tutorId)
                    .ToListAsync();

                // Apply additional filtering in memory
                var filteredTutors = tutors
                    .Where(t => t.AvailableDays.Contains(day)
                             && t.AvailableTime.Contains(time)
                             && t.AvailableTime1.Contains(time1))
                    .ToList();

                if (filteredTutors == null || !filteredTutors.Any())
                {
                    throw new ApiException("No tutors found for the given criteria.");
                }

                return filteredTutors;
            }
        }


        public async Task<List<Schedule>> GetByUserId(string userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                return await dbContext.Schedules
                    .Where(s => s.StudentId == userId || s.TutorId == userId)
                    .ToListAsync();
            }
        }

        public async Task<Schedule> Add(Schedule schedule)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                if (schedule == null)
                {
                    throw new ApiException("No schedule to add");
                }

                var result = await dbContext.Schedules.AddAsync(schedule);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<Schedule> Update(Schedule schedule)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var result = dbContext.Schedules.Update(schedule);
                await dbContext.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task Delete(string id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var schedule = await dbContext.Schedules
                    .FirstOrDefaultAsync(s => s.Id == id) ?? throw new ApiException("Schedule not found");

                dbContext.Schedules.Remove(schedule);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<Schedule>> GetByTutorId(string tutorId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                return await dbContext.Schedules
                    .Where(s => s.TutorId == tutorId)
                    .ToListAsync();
            }
        }
    }
}

