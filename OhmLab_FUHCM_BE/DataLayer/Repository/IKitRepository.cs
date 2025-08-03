using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IKitRepository
    {
        Task<bool> CreateKit(Kit kit);
        Task<bool> UpdateKit(Kit kit);
        Task<bool> DeleteKit(Kit kit);
        Task<List<Kit>> GetAllKit();
        Task<List<Kit>> GetAllKitByKitTemplateId(string kitTemplateId);
        Task<Kit> GetKitById(string id);
        Task<Kit> GetKitByName(string name);
    }
}
