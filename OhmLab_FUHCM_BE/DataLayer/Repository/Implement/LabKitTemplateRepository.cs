using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class LabKitTemplateRepository : ILabKitTemplateRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public LabKitTemplateRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<List<LabKitTemplate>> GetByLabIdAsync(int labId)
        {
            return await _context.LabKitTemplates
                .Include(lk => lk.KitTemplate)
                .Where(lk => lk.LabId == labId)
                .ToListAsync();
        }

        public async Task<LabKitTemplate?> GetByIdAsync(int id)
        {
            return await _context.LabKitTemplates
                .Include(lk => lk.KitTemplate)
                .FirstOrDefaultAsync(lk => lk.LabKitTemplateId == id);
        }

        public async Task<LabKitTemplate> CreateAsync(LabKitTemplate labKitTemplate)
        {
            await _context.LabKitTemplates.AddAsync(labKitTemplate);
            await _context.SaveChangesAsync();
            return labKitTemplate;
        }

        public async Task<LabKitTemplate> UpdateAsync(LabKitTemplate labKitTemplate)
        {
            _context.LabKitTemplates.Update(labKitTemplate);
            await _context.SaveChangesAsync();
            return labKitTemplate;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var labKitTemplate = await _context.LabKitTemplates.FindAsync(id);
            if (labKitTemplate == null)
                return false;

            _context.LabKitTemplates.Remove(labKitTemplate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int labId, string kitTemplateId)
        {
            return await _context.LabKitTemplates
                .AnyAsync(lk => lk.LabId == labId && lk.KitTemplateId == kitTemplateId);
        }
    }
}
