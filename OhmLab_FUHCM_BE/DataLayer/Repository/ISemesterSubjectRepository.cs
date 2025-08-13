using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ISemesterSubjectRepository
    {
        Task<SemesterSubject> GetBySubjectIdAsync(int subjectId);
        Task<IEnumerable<SemesterSubject>> GetAllAsync();
        Task<SemesterSubject> AddAsync(SemesterSubject semesterSubject);
    }
}
