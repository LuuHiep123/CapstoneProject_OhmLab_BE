using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface ILabEquipmentTypeRepository
    {
        Task<List<LabEquipmentType>> GetByLabIdAsync(int labId);
        Task<LabEquipmentType?> GetByIdAsync(int id);
        Task<LabEquipmentType> CreateAsync(LabEquipmentType labEquipmentType);
        Task<LabEquipmentType> UpdateAsync(LabEquipmentType labEquipmentType);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int labId, string equipmentTypeId);
    }
}

