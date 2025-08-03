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
    public class KitRepository : IKitRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public KitRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateKit(Kit kit)
        {
            try
            {
                await _context.Kits.AddAsync(kit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKit(Kit kit)
        {
            try
            {
                _context.Kits.Remove(kit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Kit>> GetAllKit()
        {
            try
            {
                var listkit = await _context.Kits
                    .Include(k => k.KitTemplate)
                    .ToListAsync();
                return listkit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Kit>> GetAllKitByKitTemplateId(string kitTemplateId)
        {
            var listkit = await _context.Kits
                .Include(k => k.KitTemplate)
                .Where(k => k.KitTemplateId.Equals(kitTemplateId))
                .ToListAsync();
            return listkit;
        }

        public async Task<Kit> GetKitById(string id)
        {
            try
            {
                var kit = await _context.Kits
                    .FirstOrDefaultAsync(k => k.KitId.Equals(id));
                return kit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Kit> GetKitByName(string name)
        {
            try
            {
                var kit = await _context.Kits
                    .FirstOrDefaultAsync(k => k.KitName.Equals(name));
                return kit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateKit(Kit kit)
        {
            try
            {
                _context.Kits.Update(kit);
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
