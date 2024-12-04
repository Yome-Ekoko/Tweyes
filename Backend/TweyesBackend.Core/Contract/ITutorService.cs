using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;

namespace TweyesBackend.Core.Contract
{
    public interface ITutorService
    {
        Task<Response<AddTutorResponse>> AddTutor(AddTutorRequest request);
        Task<Response<List<AddTutorResponse>>> GetAllTutor();
        Task<Response<AddTutorResponse>> GetTutor();
        Task<Response<AddTutorResponse>> GetById(string id);
        Task<string> SoftDeletePayer(string id);
        Task<Response<AddTutorResponse>> UpdateTutor(UpdateTutorRequest request);
    }
}
