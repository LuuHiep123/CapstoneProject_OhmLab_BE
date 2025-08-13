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
        private readonly db_abadcb_ohmlabContext _context;

        public LabRepository(db_abadcb_ohmlabContext context)
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
            return await _context.Labs
                .Include(l => l.Subject)
                .Include(l => l.LabEquipmentTypes)
                    .ThenInclude(le => le.EquipmentType)
                .Include(l => l.LabKitTemplates)
                    .ThenInclude(lk => lk.KitTemplate)
                .FirstOrDefaultAsync(l => l.LabId == id);
        }

        public async Task<List<Lab>> GetLabsBySubjectId(int subjectId)
        {
            return await _context.Labs
                .Include(l => l.Subject)
                .Include(l => l.LabEquipmentTypes)
                    .ThenInclude(le => le.EquipmentType)
                .Include(l => l.LabKitTemplates)
                    .ThenInclude(lk => lk.KitTemplate)
                .Where(l => l.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task<List<Lab>> GetLabsByLecturerId(string lecturerId)
        {
            // Lấy tất cả SubjectId mà lecturer này dạy (thông qua Class)
            var subjectIds = await _context.Classes
                .Where(c => c.LecturerId.ToString() == lecturerId)
                .Select(c => c.SubjectId)
                .Distinct()
                .ToListAsync();

            // Lấy tất cả labs của các subject đó
            return await _context.Labs
                .Include(l => l.Subject)
                .Include(l => l.LabEquipmentTypes)
                    .ThenInclude(le => le.EquipmentType)
                .Include(l => l.LabKitTemplates)
                    .ThenInclude(lk => lk.KitTemplate)
                .Where(l => subjectIds.Contains(l.SubjectId))
                .ToListAsync();
        }

        public async Task<List<Lab>> GetLabsByClassId(int classId)
        {
            // Lấy SubjectId từ class
            var classEntity = await _context.Classes
                .Where(c => c.ClassId == classId)
                .Select(c => c.SubjectId)
                .FirstOrDefaultAsync();

            if (classEntity == 0)
                return new List<Lab>();

            // Lấy tất cả labs của subject đó
            return await _context.Labs
                .Include(l => l.Subject)
                .Include(l => l.LabEquipmentTypes)
                    .ThenInclude(le => le.EquipmentType)
                .Include(l => l.LabKitTemplates)
                    .ThenInclude(lk => lk.KitTemplate)
                .Where(l => l.SubjectId == classEntity)
                .ToListAsync();
        }

        public async Task UpdateLab(Lab lab)
        {
            _context.Entry(lab).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Lab>> GetAllLabs()
        {
            return await _context.Labs
                .Include(l => l.Subject)
                .Include(l => l.LabEquipmentTypes)
                    .ThenInclude(le => le.EquipmentType)
                .Include(l => l.LabKitTemplates)
                    .ThenInclude(lk => lk.KitTemplate)
                .ToListAsync();
        }
    }
} 