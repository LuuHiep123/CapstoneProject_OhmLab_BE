using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ISlotRepository
    {
        Task<List<Slot>> GetAllAsync();
        Task<Slot> GetByIdAsync(int id);
        Task<Slot> CreateAsync(Slot slot);
        Task<Slot> UpdateAsync(Slot slot);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 