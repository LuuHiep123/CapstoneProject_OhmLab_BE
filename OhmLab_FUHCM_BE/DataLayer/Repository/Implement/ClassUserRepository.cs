using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class ClassUserRepository : IClassUserRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public ClassUserRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }

        public async Task<List<ClassUser>> GetAllAsync()
        {
            try
            {
                return await _DBContext.ClassUsers
                    .Include(cu => cu.Class)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(s => s.SemesterSubjects.Where(ss => ss.Semester != null))
                                .ThenInclude(ss => ss.Semester)
                    .Include(cu => cu.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClassUser> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.ClassUsers
                    .Include(cu => cu.Class)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(s => s.SemesterSubjects.Where(ss => ss.Semester != null))
                                .ThenInclude(ss => ss.Semester)
                    .Include(cu => cu.User)
                    .FirstOrDefaultAsync(cu => cu.ClassUserId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ClassUser>> GetByClassIdAsync(int classId)
        {
            try
            {
                return await _DBContext.ClassUsers
                    .Include(cu => cu.Class)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(s => s.SemesterSubjects.Where(ss => ss.Semester != null))
                                .ThenInclude(ss => ss.Semester)
                    .Include(cu => cu.User)
                    .Where(cu => cu.ClassId == classId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ClassUser>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _DBContext.ClassUsers
                    .Include(cu => cu.Class)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(s => s.SemesterSubjects.Where(ss => ss.Semester != null))
                                .ThenInclude(ss => ss.Semester)
                    .Include(cu => cu.User)
                    .Where(cu => cu.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClassUser> CreateAsync(ClassUser classUser)
        {
            try
            {
                await _DBContext.ClassUsers.AddAsync(classUser);
                await _DBContext.SaveChangesAsync();
                return classUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClassUser> UpdateAsync(ClassUser classUser)
        {
            try
            {
                _DBContext.ClassUsers.Update(classUser);
                await _DBContext.SaveChangesAsync();
                return classUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var classUser = await GetByIdAsync(id);
                if (classUser != null)
                {
                    _DBContext.ClassUsers.Remove(classUser);
                    await _DBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _DBContext.ClassUsers.AnyAsync(cu => cu.ClassUserId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsUserInClassAsync(Guid userId, int classId)
        {
            try
            {
                return await _DBContext.ClassUsers.AnyAsync(cu => cu.UserId == userId && cu.ClassId == classId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 