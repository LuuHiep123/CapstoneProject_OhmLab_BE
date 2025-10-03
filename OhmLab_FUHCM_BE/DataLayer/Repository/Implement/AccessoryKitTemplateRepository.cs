using DataLayer.DBContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class AccessoryKittemplateRepository : IAccessoryKitTemplateRepository
    {

        private readonly db_abadcb_ohmlabContext _context;
        public AccessoryKittemplateRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate)
        {
            try
            {
                await _context.AccessoryKitTemplates.AddAsync(accessoryKitTemplate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate)
        {
            try
            {
                _context.AccessoryKitTemplates.Remove(accessoryKitTemplate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AccessoryKitTemplate> GetAccessoryKitTemplateById(int id)
        {
            try
            {
                var accessoryKitTemplate = await _context.AccessoryKitTemplates
                    .Include(ak => ak.KitTemplate)
                    .Include(ak => ak.Accessory)
                    .FirstOrDefaultAsync(a => a.AccessoryKitTemplateId.Equals(id));
                return accessoryKitTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AccessoryKitTemplate>> GetAllAccessoryKitTemplate()
        {
            try
            {
                var listAccessoryKitTemplate = await _context.AccessoryKitTemplates
                    .Include(ak => ak.KitTemplate)
                    .Include(ak => ak.Accessory)
                    .ToListAsync();
                return listAccessoryKitTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAccessoryKitTemplate(AccessoryKitTemplate accessoryKitTemplate)
        {
            try
            {
                _context.AccessoryKitTemplates.Update(accessoryKitTemplate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
