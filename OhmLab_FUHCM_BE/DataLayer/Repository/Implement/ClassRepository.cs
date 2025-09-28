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
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .AsSplitQuery() // Use split query for better performance
                .FirstOrDefaultAsync(c => c.ClassId == id);
        }

        public async Task<List<Class>> GetByLecturerIdAsync(Guid lecturerId)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .Where(c => c.LecturerId == lecturerId)
                .AsSplitQuery() // Use split query for better performance
                .ToListAsync();
        }

        public async Task<List<Class>> GetByStudentIdAsync(Guid studentId)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .Where(c => c.ClassUsers.Any(cu => cu.UserId == studentId))
                .AsSplitQuery() // Use split query for better performance
                .ToListAsync();
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .AsSplitQuery() // Use split query for better performance
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

        public async Task<Class> GetByName(string name)
        {
            var Class = await _DBContext.Classes.FirstOrDefaultAsync(c => c.ClassName.Equals(name));
            return Class;
        }

        public async Task<List<Class>> GetByStudentIdWithIncludesAsync(Guid studentId)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .Where(c => c.ClassUsers.Any(cu => cu.UserId == studentId))
                .AsSplitQuery() // Use split query for better performance
                .ToListAsync();
        }

        public async Task<List<Class>> GetByLecturerIdWithIncludesAsync(Guid lecturerId)
        {
            return await _DBContext.Classes
                .Include(c => c.Subject)
                    .ThenInclude(s => s.SemesterSubjects)
                        .ThenInclude(ss => ss.Semester)
                .Include(c => c.Lecturer)
                .Include(c => c.ScheduleType)
                    .ThenInclude(st => st.Slot)
                .Include(c => c.ClassUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Teams)
                .Where(c => c.LecturerId == lecturerId)
                .AsSplitQuery() // Use split query for better performance
                .ToListAsync();
        }

        public async Task<bool> CheckLecturerExistsAsync(Guid lecturerId)
        {
            try
            {
                return await _DBContext.Classes
                    .AnyAsync(c => c.LecturerId == lecturerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}