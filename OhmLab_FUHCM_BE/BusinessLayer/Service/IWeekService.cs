using BusinessLayer.RequestModel.Week;
using BusinessLayer.ResponseModel.Week;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IWeekService
    {
        Task<WeekResponseModel> CreateWeekAsync(CreateWeekRequestModel model);
        Task<IEnumerable<WeekResponseModel>> GetBySemesterIdAsync(int semesterId);
        Task<WeekResponseModel> GetByIdAsync(int id);
        Task<IEnumerable<WeekResponseModel>> GetAllAsync();
        Task<WeekResponseModel> UpdateAsync(int id, UpdateWeekRequestModel model);
        Task<bool> DeleteAsync(int id);
    }
} 