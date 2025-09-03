using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IClassRepository
    {
        Task<Class> GetByIdAsync(int id);
        Task<Class> GetByName(string name);
        Task<List<Class>> GetByLecturerIdAsync(Guid lecturerId);
        Task<List<Class>> GetByStudentIdAsync(Guid studentId);
        Task<List<Class>> GetAllAsync();
        Task<Class> CreateAsync(Class classEntity);
        Task<Class> UpdateAsync(Class classEntity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 