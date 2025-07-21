using DataLayer.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface IClassRepository
    {
        Task<Class> GetByIdAsync(int id);
        Task<List<Class>> GetByLecturerIdAsync(Guid lecturerId);
    }
} 