using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IAccessoryRepository
    {
        Task<bool> CreateAccessory(Accessory accessory);
        Task<bool> UpdateAccessory(Accessory accessory);
        Task<bool> DeleteAccessory(Accessory accessory);
        Task<List<Accessory>> GetAllAccessory();
        Task<Accessory> GetAccessoryById(int id);
    }
}
