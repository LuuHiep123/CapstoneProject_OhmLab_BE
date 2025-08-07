    using BusinessLayer.RequestModel.Semester;
using BusinessLayer.ResponseModel.Semester;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;

        public SemesterService(ISemesterRepository semesterRepository, ISemesterSubjectRepository semesterSubjectRepository)
        {
            _semesterRepository = semesterRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
        }

        public async Task<SemesterResponseModel> CreateSemesterAsync(CreateSemesterRequestModel model)
        {
            var semester = new Semester
            {
                SemesterName = model.SemesterName,
                SemesterStartDate = model.SemesterStartDate,
                SemesterEndDate = model.SemesterEndDate,
                SemesterDescription = model.SemesterDescription,
                SemesterStatus = model.SemesterStatus
            };
            var result = await _semesterRepository.AddAsync(semester);
            return new SemesterResponseModel
            {
                SemesterId = result.SemesterId,
                SemesterName = result.SemesterName,
                SemesterStartDate = result.SemesterStartDate,
                SemesterEndDate = result.SemesterEndDate,
                SemesterDescription = result.SemesterDescription,
                SemesterStatus = result.SemesterStatus
            };
        }

        public async Task<SemesterResponseModel> GetByIdAsync(int id)
        {
            var s = await _semesterRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new SemesterResponseModel
            {
                SemesterId = s.SemesterId,
                SemesterName = s.SemesterName,
                SemesterStartDate = s.SemesterStartDate,
                SemesterEndDate = s.SemesterEndDate,
                SemesterDescription = s.SemesterDescription,
                SemesterStatus = s.SemesterStatus
            };
        }

        public async Task<IEnumerable<SemesterResponseModel>> GetAllAsync()
        {
            var semesters = await _semesterRepository.GetAllAsync();
            return semesters.Select(s => new SemesterResponseModel
            {
                SemesterId = s.SemesterId,
                SemesterName = s.SemesterName,
                SemesterStartDate = s.SemesterStartDate,
                SemesterEndDate = s.SemesterEndDate,
                SemesterDescription = s.SemesterDescription,
                SemesterStatus = s.SemesterStatus
            });
        }

        public async Task<SemesterResponseModel> UpdateAsync(int id, UpdateSemesterRequestModel model)
        {
            var semester = new Semester
            {
                SemesterName = model.SemesterName,
                SemesterStartDate = model.SemesterStartDate,
                SemesterEndDate = model.SemesterEndDate,
                SemesterDescription = model.SemesterDescription,
                SemesterStatus = model.SemesterStatus
            };
            var result = await _semesterRepository.UpdateAsync(id, semester);
            if (result == null) return null;
            return new SemesterResponseModel
            {
                SemesterId = result.SemesterId,
                SemesterName = result.SemesterName,
                SemesterStartDate = result.SemesterStartDate,
                SemesterEndDate = result.SemesterEndDate,
                SemesterDescription = result.SemesterDescription,
                SemesterStatus = result.SemesterStatus
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Kiểm tra Semester có tồn tại không
            var semester = await _semesterRepository.GetByIdAsync(id);
            if (semester == null)
            {
                return false;
            }

            // Kiểm tra Semester có đang được sử dụng bởi SemesterSubject nào không
            var semesterSubjects = await _semesterSubjectRepository.GetAllAsync();
            var usingSemesterSubjects = semesterSubjects.Where(ss => ss.SemesterId == id).ToList();
            
            if (usingSemesterSubjects.Any())
            {
                // Nếu có SemesterSubject đang sử dụng, không cho phép xóa
                return false;
            }

            return await _semesterRepository.DeleteAsync(id);
        }
    }
} 