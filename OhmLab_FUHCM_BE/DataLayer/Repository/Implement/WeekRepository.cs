using DataLayer.DBContext;
using DataLayer.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implement
{
    public class WeekRepository : IWeekRepository
    {
        private readonly OhmLab_DBContext _DBContext;
        public WeekRepository(OhmLab_DBContext context)
        {
            _DBContext = context;
        }
        public async Task<Week> GetByIdAsync(int id)
        {
            return await _DBContext.Weeks.FindAsync(id);
        }

        public async Task<Week> AddAsync(Week week)
        {
            _DBContext.Weeks.Add(week);
            await _DBContext.SaveChangesAsync();
            return week;
        }

        public async Task<IEnumerable<Week>> GetBySemesterIdAsync(int semesterId)
        {
            return await _DBContext.Weeks.Where(w => w.SemesterId == semesterId).ToListAsync();
        }

        public async Task<IEnumerable<Week>> GetAllAsync()
        {
            return await _DBContext.Weeks.ToListAsync();
        }

        public async Task<Week> UpdateAsync(int id, Week week)
        {
            var existing = await _DBContext.Weeks.FindAsync(id);
            if (existing == null) return null;
            existing.WeeksName = week.WeeksName;
            existing.WeeksStartDate = week.WeeksStartDate;
            existing.WeeksEndDate = week.WeeksEndDate;
            existing.WeeksDescription = week.WeeksDescription;
            existing.WeeksStatus = week.WeeksStatus;
            await _DBContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var week = await _DBContext.Weeks.FindAsync(id);
            if (week == null) return false;
            _DBContext.Weeks.Remove(week);
            await _DBContext.SaveChangesAsync();
            return true;
        }
    }
} 