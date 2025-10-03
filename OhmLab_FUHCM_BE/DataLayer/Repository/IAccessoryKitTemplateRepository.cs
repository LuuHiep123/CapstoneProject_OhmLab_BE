using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IAccessoryKitTemplateRepository
    {
        Task<bool> CreateAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate);
        Task<bool> UpdateAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate);
        Task<bool> DeleteAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate);
        Task<List<AccessoryKitTemplate>> GetAllAccessoryKitTemplate();
        Task<AccessoryKitTemplate> GetAccessoryKitTemplateById(int id);
    }
}
