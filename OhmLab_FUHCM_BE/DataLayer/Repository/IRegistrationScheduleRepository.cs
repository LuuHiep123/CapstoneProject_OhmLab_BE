using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRegistrationScheduleRepository
    {
        Task<bool> CreateRegistrationSchedule(RegistrationSchedule registrationSchedule);
        Task<bool> UpdateRegistrationSchedule(RegistrationSchedule registrationSchedule);
        Task<bool> DeleteRegistrationSchedule(RegistrationSchedule registrationSchedule);
        Task<List<RegistrationSchedule>> GetAllRegistrationSchedule();
        Task<List<RegistrationSchedule>> GetAllRegistrationScheduleByTeacherId(Guid teacherId);
        Task<RegistrationSchedule> GetRegistrationScheduleById(int id);
        Task<List<RegistrationSchedule>> GetByDateWithIncludesAsync(DateTime date);
        Task<List<RegistrationSchedule>> GetByTeacherIdAndDateWithIncludesAsync(Guid teacherId, DateTime date);
        Task<List<RegistrationSchedule>> GetByClassIdAndDateWithIncludesAsync(int classId, DateTime date);
    }
}
