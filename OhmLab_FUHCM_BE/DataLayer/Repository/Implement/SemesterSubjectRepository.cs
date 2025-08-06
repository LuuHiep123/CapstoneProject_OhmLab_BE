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
                Console.WriteLine($"[ERROR][GetAllAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
