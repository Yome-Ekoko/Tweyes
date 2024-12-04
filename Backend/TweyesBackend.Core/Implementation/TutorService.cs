using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TweyesBackend.Domain.Common;
using Microsoft.AspNetCore.Identity;
using TweyesBackend.Core.Contract;
using TweyesBackend.Core.Contract.Repository;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Core.Exceptions;
using Microsoft.IdentityModel.Tokens;
using TweyesBackend.Persistence;
using AutoMapper;

namespace TweyesBackend.Core.Implementation
{
    public class TutorService : ITutorService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<T_User> _userManager;
        private readonly ITutorRepository _tutorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;
        private readonly IAPIImplementation _apiImplementation;


        public TutorService(IMapper mapper, UserManager<T_User> userManager,
            ITutorRepository tutorRepository,
            IHttpContextAccessor httpContextAccessor,
            IAPIImplementation apiImplementation
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _tutorRepository = tutorRepository;
            _httpContextAccessor = httpContextAccessor;
            // _context = context;
            _apiImplementation = apiImplementation;


        }

        public async Task<Response<AddTutorResponse>> AddTutor(AddTutorRequest request)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("User not found.");
            }

            var existingTutor = await _tutorRepository.GetTutorAsync();
            if (existingTutor != null)
            {
                throw new ApiException("Tutor already exists for this user.");
            }

            var newTutor = new Tutor
            {
                AvailableDays = request.AvailableDays,
                AvailableTime = request.AvailableTime,
                AvailableTime1 = request.AvailableTime1,
                Introduction = request.Introduction,
                Qualifications = request.Qualifications,
                HourlyRate = request.HourlyRate,
                Languages = request.Languages,
                PreferredCurrency = request.PreferredCurrency,
                UserId = userId,
                PreferredCommunication = request.PreferredCommunication,
                TargetedClass = request.TargetedClass
            };

            var response = await _tutorRepository.Add(newTutor);
            if (response == null)
            {
                throw new ApiException("Unable to save Tutor.");
            }

            var result = _mapper.Map<AddTutorResponse>(response);
            return new Response<AddTutorResponse>(result);
        }

        public async Task<Response<List<AddTutorResponse>>> GetAllTutor()
        {
            var student = await _tutorRepository.GetAll();
            var result = _mapper.Map<List<AddTutorResponse>>(student);

            return new Response<List<AddTutorResponse>>(result);
        }

        public async Task<Response<AddTutorResponse>> GetTutor()
        {
            var student = await _tutorRepository.GetTutorAsync();
            var result = _mapper.Map<AddTutorResponse>(student);

            return new Response<AddTutorResponse>(result);
        }

        public async Task<Response<AddTutorResponse>> GetById(string id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "";
            var student = await _tutorRepository.GetById(id);
            var result = _mapper.Map<AddTutorResponse>(student);

            return new Response<AddTutorResponse>(result);
        }

        public async Task<string> SoftDeletePayer(string id)
        {
            await _tutorRepository.Delete(id);
            return "Deleted";
        }


        public async Task<Response<AddTutorResponse>> UpdateTutor(UpdateTutorRequest request)
        {
            var UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "";

            var existingTutor = await _tutorRepository.GetById(request.Id) ?? throw new ApiException("Tutor Not Found");

            existingTutor.Qualifications = (request.Qualifications == null || !request.Qualifications.Any()) ? existingTutor.Qualifications : request.Qualifications;
            existingTutor.Languages = (request.Languages == null || !request.Languages.Any()) ? existingTutor.Languages : request.Languages;
            existingTutor.Introduction = string.IsNullOrEmpty(request.Introduction) ? existingTutor.Introduction : request.Introduction;
            existingTutor.AvailableDays = (request.AvailableDays == null || !request.AvailableDays.Any()) ? existingTutor.AvailableDays : request.AvailableDays;
            existingTutor.AvailableTime = (request.AvailableTime == null || !request.AvailableTime.Any()) ? existingTutor.AvailableTime : request.AvailableTime;
            existingTutor.AvailableTime1 = (request.AvailableTime1 == null || !request.AvailableTime1.Any()) ? existingTutor.AvailableTime1 : request.AvailableTime1;
            existingTutor.HourlyRate = string.IsNullOrEmpty(request.HourlyRate) ? existingTutor.HourlyRate : request.HourlyRate;
            existingTutor.PreferredCurrency = string.IsNullOrEmpty(request.PreferredCurrency) ? existingTutor.PreferredCurrency : request.PreferredCurrency;


            var result = await _tutorRepository.Update(existingTutor) ?? throw new ApiException("Unable to update Tutor");

            var response = _mapper.Map<AddTutorResponse>(result);

            return new Response<AddTutorResponse>(response);
        }
    }

}

