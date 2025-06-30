using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ISubjectRepository
    {
        Task<Subject> GetSubjectById(int id);
        Task<List<Subject>> GetAllSubjects();
        Task AddSubject(Subject subject);
        Task UpdateSubject(Subject subject);
        Task DeleteSubject(int id);
    }
} 