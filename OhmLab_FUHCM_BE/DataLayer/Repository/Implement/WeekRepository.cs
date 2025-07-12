using DataLayer.DBContext;
using DataLayer.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
} 