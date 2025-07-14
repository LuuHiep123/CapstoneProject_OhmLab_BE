using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ISemesterRepository
    {
        Task<Semester> GetByIdAsync(int id);
        Task<IEnumerable<Semester>> GetAllAsync();
        Task<Semester> AddAsync(Semester semester);
        Task<Semester> UpdateAsync(int id, Semester semester);
        Task<bool> DeleteAsync(int id);
    }
} 