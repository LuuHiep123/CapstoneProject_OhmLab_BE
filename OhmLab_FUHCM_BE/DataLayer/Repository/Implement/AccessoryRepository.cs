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
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly db_abadcb_ohmlabContext _context;
        public AccessoryRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccessory(Accessory accessory)
        {
            try
            {
                await _context.Accessories.AddAsync(accessory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public async Task<bool> DeleteAccessory(Accessory accessory)
        {
            try
            {
                _context.Accessories.Remove(accessory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Accessory> GetAccessoryById(int id)
        {
            try
            {
                var accessory = await _context.Accessories
                    .FirstOrDefaultAsync(a => a.AccessoryId.Equals(id));
                return accessory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Accessory>> GetAllAccessory()
        {
            try
            {
                var listAccessory = await _context.Accessories
                    .ToListAsync();
                return listAccessory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAccessory(Accessory accessory)
        {
            try
            {
                _context.Accessories.Update(accessory);
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
