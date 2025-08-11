using BusinessLayer.RequestModel.Semester;
using BusinessLayer.ResponseModel.Semester;
using BusinessLayer.ResponseModel.BaseResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ISemesterService
    {
        Task<SemesterResponseModel> CreateSemesterAsync(CreateSemesterRequestModel model);
        Task<SemesterResponseModel> GetByIdAsync(int id);
        Task<DynamicResponse<SemesterResponseModel>> GetAllAsync();
        Task<SemesterResponseModel> UpdateAsync(int id, UpdateSemesterRequestModel model);
        Task<bool> DeleteAsync(int id);
    }
} 