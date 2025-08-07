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
    public class KitTemplateRepository : IKitTemplateRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public KitTemplateRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateKitTemplate(KitTemplate kitTemplate)
        {
            try
            {
                await _context.KitTemplates.AddAsync(kitTemplate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKitTemplate(KitTemplate kitTemplate)
        {
            try
            {
                _context.KitTemplates.Remove(kitTemplate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<KitTemplate>> GetAllKitTemplate()
        {
            try
            {
                var listkitTemplate = await _context.KitTemplates
                    .ToListAsync();
                return listkitTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KitTemplate> GetKitTemplateById(string id)
        {
            try
            {
                var kitTemplate = await _context.KitTemplates
                    .FirstOrDefaultAsync(kt => kt.KitTemplateId.Equals(id));
                return kitTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KitTemplate> GetKitTemplateByName(string name)
        {
            try
            {
                var kitTemplate = await _context.KitTemplates
                    .FirstOrDefaultAsync(kt => kt.KitTemplateName.Equals(name));
                return kitTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateKitTemplate(KitTemplate kitTemplate)
        {
            try
            {
                _context.KitTemplates.Update(kitTemplate);
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
