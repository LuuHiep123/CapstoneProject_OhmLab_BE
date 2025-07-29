using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class SlotRepository : ISlotRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public SlotRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }

        public async Task<List<Slot>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Slots
                    .Include(s => s.ScheduleTypes)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Slot> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.Slots
                    .Include(s => s.ScheduleTypes)
                    .FirstOrDefaultAsync(s => s.SlotId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Slot> CreateAsync(Slot slot)
        {
            try
            {
                await _DBContext.Slots.AddAsync(slot);
                await _DBContext.SaveChangesAsync();
                return slot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Slot> UpdateAsync(Slot slot)
        {
            try
            {
                _DBContext.Slots.Update(slot);
                await _DBContext.SaveChangesAsync();
                return slot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var slot = await GetByIdAsync(id);
                if (slot != null)
                {
                    _DBContext.Slots.Remove(slot);
                    await _DBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _DBContext.Slots.AnyAsync(s => s.SlotId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 