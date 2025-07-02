using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class LabRepository : ILabRepository
    {
        private readonly OhmLab_DBContext _context;

        public LabRepository(OhmLab_DBContext context)
        {
            _context = context;
        }

        public async Task AddLab(Lab lab)
        {
            _context.Labs.Add(lab);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLab(int id)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab != null)
            {
                _context.Labs.Remove(lab);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Lab> GetLabById(int id)
        {
            return await _context.Labs.FindAsync(id);
        }

        public async Task<List<Lab>> GetLabsBySubjectId(int subjectId)
        {
            return await _context.Labs.Where(l => l.SubjectId == subjectId).ToListAsync();
        }

        public async Task UpdateLab(Lab lab)
        {
            _context.Entry(lab).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Lab>> GetAllLabs()
        {
            return await _context.Labs.ToListAsync();
        }
    }
} 