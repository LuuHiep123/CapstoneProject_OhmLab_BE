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
    public class ScheduleTypeRepository : IScheduleTypeRepository
    {

        private readonly db_abadcb_ohmlabContext _DBContext;

        public ScheduleTypeRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<ScheduleType> CreateAsync(ScheduleType scheduleTypes)
        {
            try
            {
                await _DBContext.ScheduleTypes.AddAsync(scheduleTypes);
                await _DBContext.SaveChangesAsync();
                return scheduleTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ScheduleType> GetByIdAsync(int id)
        {
            try
            {
                var scheduleType = await _DBContext.ScheduleTypes
                    .Include(st => st.Slot)
                    .FirstOrDefaultAsync(st => st.ScheduleTypeId == id);
                return scheduleType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ScheduleType>> GetAllAsync()
        {
            try
            {
                return await _DBContext.ScheduleTypes
                    .Include(st => st.Slot)
                    .OrderBy(st => st.ScheduleTypeName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ScheduleType> UpdateAsync(ScheduleType scheduleType)
        {
            try
            {
                _DBContext.ScheduleTypes.Update(scheduleType);
                await _DBContext.SaveChangesAsync();
                return scheduleType;
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
                var scheduleType = await _DBContext.ScheduleTypes.FindAsync(id);
                if (scheduleType == null)
                {
                    return false;
                }

                _DBContext.ScheduleTypes.Remove(scheduleType);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
