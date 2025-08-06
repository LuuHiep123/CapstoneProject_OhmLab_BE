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
                var scheduleType = await _DBContext.ScheduleTypes.FirstOrDefaultAsync(st => st.ScheduleTypeId == id);
                await _DBContext.SaveChangesAsync();
                return scheduleType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
