using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ILabRepository
    {
        Task<Lab> GetLabById(int id);
        Task<List<Lab>> GetLabsBySubjectId(int subjectId);
        Task AddLab(Lab lab);
        Task UpdateLab(Lab lab);
        Task DeleteLab(int id);
        Task<List<Lab>> GetAllLabs();
    }
} 