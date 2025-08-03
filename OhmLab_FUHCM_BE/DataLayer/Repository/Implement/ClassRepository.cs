using DataLayer.DBContext;
using DataLayer.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implement
{
    public class ClassRepository : IClassRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;
        public ClassRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }
        public async Task<Class> GetByIdAsync(int id)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                .Include(c => c.Teams)
                .FirstOrDefaultAsync(c => c.ClassId == id);
        }

        public async Task<List<Class>> GetByLecturerIdAsync(Guid lecturerId)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                .Include(c => c.Teams)
                .Where(c => c.LecturerId == lecturerId)
                .ToListAsync();
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                .Include(c => c.Teams)
                .ToListAsync();
        }

        public async Task<Class> CreateAsync(Class classEntity)
        {
            await _DBContext.Classes.AddAsync(classEntity);
            await _DBContext.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Class> UpdateAsync(Class classEntity)
        {
            _DBContext.Classes.Update(classEntity);
            await _DBContext.SaveChangesAsync();
            return classEntity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await GetByIdAsync(id);
            if (classEntity != null)
            {
                _DBContext.Classes.Remove(classEntity);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _DBContext.Classes.AnyAsync(c => c.ClassId == id);
        }
    }
} 