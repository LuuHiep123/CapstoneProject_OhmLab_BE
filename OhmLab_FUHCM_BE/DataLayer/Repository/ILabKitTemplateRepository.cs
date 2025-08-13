using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface ILabKitTemplateRepository
    {
        Task<List<LabKitTemplate>> GetByLabIdAsync(int labId);
        Task<LabKitTemplate?> GetByIdAsync(int id);
        Task<LabKitTemplate> CreateAsync(LabKitTemplate labKitTemplate);
        Task<LabKitTemplate> UpdateAsync(LabKitTemplate labKitTemplate);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int labId, string kitTemplateId);
    }
}
