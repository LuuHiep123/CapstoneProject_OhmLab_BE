using BusinessLayer.RequestModel.Week;
using BusinessLayer.ResponseModel.Week;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Service.Implement
{
    public class WeekService : IWeekService
    {
        private readonly IWeekRepository _weekRepository;
        public WeekService(IWeekRepository weekRepository)
        {
            _weekRepository = weekRepository;
        }
        public async Task<WeekResponseModel> CreateWeekAsync(CreateWeekRequestModel model)
        {
            var week = new Week
            {
                SemesterId = model.SemesterId,
                WeeksName = model.WeeksName,
                WeeksStartDate = model.WeeksStartDate,
                WeeksEndDate = model.WeeksEndDate,
                WeeksDescription = model.WeeksDescription,
                WeeksStatus = model.WeeksStatus
            };
            var result = await _weekRepository.AddAsync(week);
            return new WeekResponseModel
            {
                WeeksId = result.WeeksId,
                SemesterId = result.SemesterId,
                WeeksName = result.WeeksName,
                WeeksStartDate = result.WeeksStartDate,
                WeeksEndDate = result.WeeksEndDate,
                WeeksDescription = result.WeeksDescription,
                WeeksStatus = result.WeeksStatus
            };
        }

        public async Task<IEnumerable<WeekResponseModel>> GetBySemesterIdAsync(int semesterId)
        {
            var weeks = await _weekRepository.GetBySemesterIdAsync(semesterId);
            return weeks.Select(w => new WeekResponseModel
            {
                WeeksId = w.WeeksId,
                SemesterId = w.SemesterId,
                WeeksName = w.WeeksName,
                WeeksStartDate = w.WeeksStartDate,
                WeeksEndDate = w.WeeksEndDate,
                WeeksDescription = w.WeeksDescription,
                WeeksStatus = w.WeeksStatus
            });
        }

        public async Task<WeekResponseModel> GetByIdAsync(int id)
        {
            var w = await _weekRepository.GetByIdAsync(id);
            if (w == null) return null;
            return new WeekResponseModel
            {
                WeeksId = w.WeeksId,
                SemesterId = w.SemesterId,
                WeeksName = w.WeeksName,
                WeeksStartDate = w.WeeksStartDate,
                WeeksEndDate = w.WeeksEndDate,
                WeeksDescription = w.WeeksDescription,
                WeeksStatus = w.WeeksStatus
            };
        }

        public async Task<IEnumerable<WeekResponseModel>> GetAllAsync()
        {
            var weeks = await _weekRepository.GetAllAsync();
            return weeks.Select(w => new WeekResponseModel
            {
                WeeksId = w.WeeksId,
                SemesterId = w.SemesterId,
                WeeksName = w.WeeksName,
                WeeksStartDate = w.WeeksStartDate,
                WeeksEndDate = w.WeeksEndDate,
                WeeksDescription = w.WeeksDescription,
                WeeksStatus = w.WeeksStatus
            });
        }

        public async Task<WeekResponseModel> UpdateAsync(int id, UpdateWeekRequestModel model)
        {
            var week = new DataLayer.Entities.Week
            {
                WeeksName = model.WeeksName,
                WeeksStartDate = model.WeeksStartDate,
                WeeksEndDate = model.WeeksEndDate,
                WeeksDescription = model.WeeksDescription,
                WeeksStatus = model.WeeksStatus
            };
            var result = await _weekRepository.UpdateAsync(id, week);
            if (result == null) return null;
            return new WeekResponseModel
            {
                WeeksId = result.WeeksId,
                SemesterId = result.SemesterId,
                WeeksName = result.WeeksName,
                WeeksStartDate = result.WeeksStartDate,
                WeeksEndDate = result.WeeksEndDate,
                WeeksDescription = result.WeeksDescription,
                WeeksStatus = result.WeeksStatus
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _weekRepository.DeleteAsync(id);
        }
    }
} 