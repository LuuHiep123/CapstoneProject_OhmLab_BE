using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class LabEquipmentTypeRepository : ILabEquipmentTypeRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public LabEquipmentTypeRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<List<LabEquipmentType>> GetByLabIdAsync(int labId)
        {
            return await _context.LabEquipmentTypes
                .Include(le => le.EquipmentType)
                .Where(le => le.LabId == labId)
                .ToListAsync();
        }

        public async Task<LabEquipmentType?> GetByIdAsync(int id)
        {
            return await _context.LabEquipmentTypes
                .Include(le => le.EquipmentType)
                .FirstOrDefaultAsync(le => le.LabEquipmentTypeId == id);
        }

        public async Task<LabEquipmentType> CreateAsync(LabEquipmentType labEquipmentType)
        {
            await _context.LabEquipmentTypes.AddAsync(labEquipmentType);
            await _context.SaveChangesAsync();
            return labEquipmentType;
        }

        public async Task<LabEquipmentType> UpdateAsync(LabEquipmentType labEquipmentType)
        {
            _context.LabEquipmentTypes.Update(labEquipmentType);
            await _context.SaveChangesAsync();
            return labEquipmentType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var labEquipmentType = await _context.LabEquipmentTypes.FindAsync(id);
            if (labEquipmentType == null)
                return false;

            _context.LabEquipmentTypes.Remove(labEquipmentType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int labId, string equipmentTypeId)
        {
            return await _context.LabEquipmentTypes
                .AnyAsync(le => le.LabId == labId && le.EquipmentTypeId == equipmentTypeId);
        }
    }
}


