using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ILabService
    {
        Task<LabResponseModel> GetLabById(int id);
        Task<List<LabResponseModel>> GetLabsBySubjectId(int subjectId);
        Task AddLab(CreateLabRequestModel lab);
        Task UpdateLab(int id, UpdateLabRequestModel lab);
        Task DeleteLab(int id);
    }
} 