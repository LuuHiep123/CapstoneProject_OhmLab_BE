using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly OhmLab_DBContext _DBContext;

        public ScheduleRepository(OhmLab_DBContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                    .Include(s => s.Weeks)
                    .Include(s => s.Reports)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetAllAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<Schedule> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                    .Include(s => s.Weeks)
                    .Include(s => s.Reports)
                    .FirstOrDefaultAsync(s => s.ScheduleId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetByClassIdAsync(int classId)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                    .Include(s => s.Weeks)
                    .Include(s => s.Reports)
                    .Where(s => s.ClassId == classId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByClassIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetByLecturerIdAsync(Guid lecturerId)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                    .Include(s => s.Weeks)
                    .Include(s => s.Reports)
                    .Where(s => s.Class.LecturerId == lecturerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByLecturerIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetByWeekIdAsync(int weekId)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                    .Include(s => s.Weeks)
                    .Include(s => s.Reports)
                    .Where(s => s.WeeksId == weekId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByWeekIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            try
            {
                await _DBContext.Schedules.AddAsync(schedule);
                await _DBContext.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                throw;
            }
        }

        public async Task<Schedule> UpdateAsync(Schedule schedule)
        {
            try
            {
                _DBContext.Schedules.Update(schedule);
                await _DBContext.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][UpdateAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var schedule = await GetByIdAsync(id);
                if (schedule != null)
                {
                    _DBContext.Schedules.Remove(schedule);
                    await _DBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][DeleteAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _DBContext.Schedules.AnyAsync(s => s.ScheduleId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][ExistsAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<int> GetScheduleCountByClassAsync(int classId)
        {
            try
            {
                return await _DBContext.Schedules
                    .CountAsync(s => s.ClassId == classId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetScheduleCountByClassAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
} 