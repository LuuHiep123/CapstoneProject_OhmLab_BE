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
    public class RegistrationScheduleRepository : IRegistrationScheduleRepository
    {
        private readonly db_abadcb_ohmlabContext _context;

        public RegistrationScheduleRepository(db_abadcb_ohmlabContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRegistrationSchedule(RegistrationSchedule registrationSchedule)
        {
            try
            {
                await _context.RegistrationSchedules.AddAsync(registrationSchedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteRegistrationSchedule(RegistrationSchedule registrationSchedule)
        {
            try
            {
                _context.RegistrationSchedules.Remove(registrationSchedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistrationSchedule>> GetAllRegistrationSchedule()
        {
            try
            {
                var listRegistrationSchedule = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .ToListAsync();
                return listRegistrationSchedule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistrationSchedule>> GetAllRegistrationScheduleByTeacherId(Guid teacherId)
        {
            try
            {
                var listRegistrationSchedule = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .Where(rs => rs.TeacherId.Equals(teacherId))
                    .ToListAsync();
                return listRegistrationSchedule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RegistrationSchedule> GetRegistrationScheduleById(int id)
        {
            try
            {
                var listRegistrationSchedule = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .FirstOrDefaultAsync(rs => rs.RegistrationScheduleId == id);
                return listRegistrationSchedule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistrationSchedule>> GetByDateWithIncludesAsync(DateTime date)
        {
            try
            {
                var registrationSchedules = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .Where(rs => rs.RegistrationScheduleDate.Date == date.Date)
                    .ToListAsync();
                return registrationSchedules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistrationSchedule>> GetByTeacherIdAndDateWithIncludesAsync(Guid teacherId, DateTime date)
        {
            try
            {
                var registrationSchedules = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .Where(rs => rs.TeacherId == teacherId && rs.RegistrationScheduleDate.Date == date.Date)
                    .ToListAsync();
                return registrationSchedules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegistrationSchedule>> GetByClassIdAndDateWithIncludesAsync(int classId, DateTime date)
        {
            try
            {
                var registrationSchedules = await _context.RegistrationSchedules
                    .Include(rs => rs.Class)
                    .Include(rs => rs.Lab)
                    .Include(rs => rs.User)
                    .Include(rs => rs.Slot)
                    .Where(rs => rs.ClassId == classId && rs.RegistrationScheduleDate.Date == date.Date)
                    .ToListAsync();
                return registrationSchedules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateRegistrationSchedule(RegistrationSchedule registrationSchedule)
        {
            try
            {
                _context.RegistrationSchedules.Update(registrationSchedule);
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
