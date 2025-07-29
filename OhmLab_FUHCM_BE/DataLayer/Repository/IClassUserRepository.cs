using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IClassUserRepository
    {
        Task<List<ClassUser>> GetAllAsync();
        Task<ClassUser> GetByIdAsync(int id);
        Task<List<ClassUser>> GetByClassIdAsync(int classId);
        Task<List<ClassUser>> GetByUserIdAsync(Guid userId);
        Task<ClassUser> CreateAsync(ClassUser classUser);
        Task<ClassUser> UpdateAsync(ClassUser classUser);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsUserInClassAsync(Guid userId, int classId);
    }
} 