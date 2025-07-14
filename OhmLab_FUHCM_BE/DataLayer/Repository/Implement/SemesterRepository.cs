using DataLayer.DBContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly OhmLab_DBContext _DBContext;
        public SemesterRepository(OhmLab_DBContext context)
        {
            _DBContext = context;
        }
        public async Task<Semester> GetByIdAsync(int id)
        {
            return await _DBContext.Semesters.FindAsync(id);
        }
        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            return await _DBContext.Semesters.ToListAsync();
        }
        public async Task<Semester> AddAsync(Semester semester)
        {
            _DBContext.Semesters.Add(semester);
            await _DBContext.SaveChangesAsync();
            return semester;
        }
        public async Task<Semester> UpdateAsync(int id, Semester semester)
        {
            var existing = await _DBContext.Semesters.FindAsync(id);
            if (existing == null) return null;
            existing.SemesterName = semester.SemesterName;
            existing.SemesterStartDate = semester.SemesterStartDate;
            existing.SemesterEndDate = semester.SemesterEndDate;
            existing.SemesterDescription = semester.SemesterDescription;
            existing.SemesterStatus = semester.SemesterStatus;
            await _DBContext.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var semester = await _DBContext.Semesters.FindAsync(id);
            if (semester == null) return false;
            _DBContext.Semesters.Remove(semester);
            await _DBContext.SaveChangesAsync();
            return true;
        }
    }
} 