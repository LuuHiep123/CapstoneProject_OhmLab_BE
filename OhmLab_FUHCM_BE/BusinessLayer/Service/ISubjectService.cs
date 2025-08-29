using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.ResponseModel.Subject;
using BusinessLayer.ResponseModel.Semester;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ISubjectService
    {
        Task<SubjectResponseModel> GetSubjectById(int id);
        Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>> GetAllSubjects();
        Task AddSubject(CreateSubjectRequestModel subject);
        Task UpdateSubject(int id, UpdateSubjectRequestModel subject);
        Task DeleteSubject(int id);
        
        // ✅ THÊM MỚI: Method debug và fix semester
        Task<string> DebugSubjectSemester(int subjectId);
        Task<bool> FixSubjectSemester(int subjectId, int semesterId);
        Task<List<SemesterResponseModel>> GetAllAvailableSemesters();
    }
} 