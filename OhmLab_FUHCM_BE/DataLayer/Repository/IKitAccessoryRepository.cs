using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IKitAccessoryRepository
    {
        Task<bool> CreateKitAccessory(KitAccessory kitAccessory);
        Task<bool> UpdateKitAccessory(KitAccessory kitAccessory);
        Task<bool> DeleteKitAccessory(KitAccessory kitAccessory);
        Task<List<KitAccessory>> GetAllKitAccessory();
        Task<KitAccessory> GetKitAccessoryById(int id);
    }
}
