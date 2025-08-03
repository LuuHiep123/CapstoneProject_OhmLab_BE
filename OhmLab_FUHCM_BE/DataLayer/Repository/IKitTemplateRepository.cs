using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IKitTemplateRepository
    {
        Task<bool> CreateKitTemplate(KitTemplate kitTemplate);
        Task<bool> UpdateKitTemplate(KitTemplate kitTemplate);
        Task<bool> DeleteKitTemplate(KitTemplate kitTemplate);
        Task<List<KitTemplate>> GetAllKitTemplate();
        Task<KitTemplate> GetKitTemplateById(string id);
    }
}
