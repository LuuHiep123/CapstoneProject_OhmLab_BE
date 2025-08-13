using DataLayer.DBContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class SemesterSubjectRepository : ISemesterSubjectRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public SemesterSubjectRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<SemesterSubject> GetBySubjectIdAsync(int subjectId)
        {
            try
            {
                return await _DBContext.SemesterSubjects
                    .FirstOrDefaultAsync(ss => ss.SubjectId == subjectId && ss.SemesterSubject1.ToLower().Equals("valid"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetBySubjectIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<SemesterSubject>> GetAllAsync()
        {
            try
            {
                return await _DBContext.SemesterSubjects
                    .Where(ss => ss.SemesterSubject1.ToLower().Equals("valid"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetAllAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<SemesterSubject> AddAsync(SemesterSubject semesterSubject)
        {
            try
            {
                _DBContext.SemesterSubjects.Add(semesterSubject);
                await _DBContext.SaveChangesAsync();
                return semesterSubject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][AddAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
