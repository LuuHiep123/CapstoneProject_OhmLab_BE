using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public SubjectRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task AddSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task UpdateSubject(Subject subject)
        {
            _context.Entry(subject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
} 