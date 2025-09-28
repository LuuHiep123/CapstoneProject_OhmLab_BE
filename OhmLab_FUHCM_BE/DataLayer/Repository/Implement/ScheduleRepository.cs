using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public ScheduleRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<List<Schedule>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ClassUsers)
                    .Include(s => s.Reports)
                    .OrderBy(s => s.ScheduleDate)
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
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
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
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .Where(s => s.ClassId == classId)
                    .OrderBy(s => s.ScheduleDate)
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
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .Where(s => s.Class.LecturerId == lecturerId)
                    .OrderBy(s => s.ScheduleDate)
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
                // Vì không có bảng Week riêng, hàm này sẽ trả về tất cả schedules
                // hoặc có thể được sử dụng để lọc theo logic khác
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .OrderBy(s => s.ScheduleDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByWeekIdAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .Where(s => s.ScheduleDate.Date >= startDate.Date && s.ScheduleDate.Date <= endDate.Date)
                    .OrderBy(s => s.ScheduleDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByDateRangeAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetByDateAsync(DateTime date)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .Where(s => s.ScheduleDate.Date == date.Date)
                    .OrderBy(s => s.ScheduleDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByDateAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
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

        public async Task<List<Schedule>> GetByDateWithIncludesAsync(DateTime date)
        {
            try
            {
                return await _DBContext.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Lecturer)
                    .Include(s => s.Class)
                        .ThenInclude(c => c.ScheduleType)
                            .ThenInclude(st => st.Slot)
                    .Include(s => s.Reports)
                    .Where(s => s.ScheduleDate.Date == date.Date)
                    .OrderBy(s => s.ScheduleDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetByDateWithIncludesAsync] {ex.Message} | Inner: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}