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
    public class KitAccessoryRepository : IKitAccessoryRepository
    {

        private readonly db_abadcb_ohmlabContext _context;
        public KitAccessoryRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateKitAccessory(KitAccessory kitAccessory)
        {
            try
            {
                await _context.KitAccessories.AddAsync(kitAccessory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKitAccessory(KitAccessory kitAccessory)
        {
            try
            {
                _context.KitAccessories.Remove(kitAccessory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<KitAccessory>> GetAllKitAccessory()
        {
            try
            {
                var listKitAccessory = await _context.KitAccessories
                    .Include(ka => ka.Kit)
                    .Include(ka => ka.Accessory)
                    .ToListAsync();
                return listKitAccessory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KitAccessory> GetKitAccessoryById(int id)
        {
            try
            {
                var KitAccessory = await _context.KitAccessories
                    .Include(ka => ka.Kit)
                    .Include(ka => ka.Accessory)
                    .FirstOrDefaultAsync(a => a.KitAccessoryId.Equals(id));
                return KitAccessory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateKitAccessory(KitAccessory kitAccessory)
        {
            try
            {
                _context.KitAccessories.Update(kitAccessory);
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
