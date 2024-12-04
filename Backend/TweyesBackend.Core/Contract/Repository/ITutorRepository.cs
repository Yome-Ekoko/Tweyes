using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Domain.Entities;

namespace TweyesBackend.Core.Contract.Repository
{
    public interface ITutorRepository
    {
        Task<List<Tutor>> GetAll();
        Task<Tutor> GetById(string id);
        Task<Tutor> Add(Tutor tutor);
        Task<Tutor> Update(Tutor tutor);
        Task Delete(string id);
        Task<Tutor> GetTutorAsync();
    }
}
