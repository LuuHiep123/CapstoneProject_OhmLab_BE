using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.ResponseModel.Subject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ISubjectService
    {
        Task<SubjectResponseModel> GetSubjectById(int id);
        Task<List<SubjectResponseModel>> GetAllSubjects();
        Task AddSubject(CreateSubjectRequestModel subject);
        Task UpdateSubject(int id, UpdateSubjectRequestModel subject);
        Task DeleteSubject(int id);
    }
} 