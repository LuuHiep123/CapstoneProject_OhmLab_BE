using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IScheduleTypeRepository
    {
        Task<ScheduleType> CreateAsync(ScheduleType scheduleTypes);
        Task<ScheduleType> GetByIdAsync(int id);
        Task<IEnumerable<ScheduleType>> GetAllAsync();
        Task<ScheduleType> UpdateAsync(ScheduleType scheduleType);
        Task<bool> DeleteAsync(int id);
    }
}
