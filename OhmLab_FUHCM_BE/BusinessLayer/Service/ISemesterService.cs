using BusinessLayer.RequestModel.Semester;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Semester;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ISemesterService
    {
        Task<SemesterResponseModel> CreateSemesterAsync(CreateSemesterRequestModel model);
        Task<SemesterResponseModel> GetByIdAsync(int id);
        Task<IEnumerable<SemesterResponseModel>> GetAllAsync();
        Task<DynamicResponse<SemesterResponseModel>> GetAllAsync(GetAllSemesterRequestModel model);
        Task<SemesterResponseModel> UpdateAsync(int id, UpdateSemesterRequestModel model);
        Task<bool> DeleteAsync(int id);
    }
} 