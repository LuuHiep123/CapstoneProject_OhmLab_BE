    using BusinessLayer.RequestModel.Semester;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Semester;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;

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

        public async Task<DynamicResponse<SemesterResponseModel>> GetAllAsync(GetAllSemesterRequestModel model)
        {
            try
            {
                var semesters = await _semesterRepository.GetAllAsync();
                var listSemesters = semesters.ToList();

                // Lọc theo keyword nếu có
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    listSemesters = listSemesters.Where(s => s.SemesterName.ToLower().Contains(model.keyWord.ToLower()) ||
                                                             (!string.IsNullOrEmpty(s.SemesterDescription) && s.SemesterDescription.ToLower().Contains(model.keyWord.ToLower())))
                                                 .ToList();
                }

                // Lọc theo status nếu có
                if (!string.IsNullOrEmpty(model.status))
                {
                    listSemesters = listSemesters.Where(s => s.SemesterStatus.ToLower().Equals(model.status.ToLower())).ToList();
                }

                // Sắp xếp theo tên học kỳ
                listSemesters = listSemesters.OrderBy(s => s.SemesterName).ToList();

                // Thực hiện phân trang
                var pagedSemesters = listSemesters.ToPagedList(model.pageNum, model.pageSize);

                var response = pagedSemesters.Select(s => new SemesterResponseModel
                {
                    SemesterId = s.SemesterId,
                    SemesterName = s.SemesterName,
                    SemesterStartDate = s.SemesterStartDate,
                    SemesterEndDate = s.SemesterEndDate,
                    SemesterDescription = s.SemesterDescription,
                    SemesterStatus = s.SemesterStatus
                }).ToList();

                return new DynamicResponse<SemesterResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách học kỳ thành công!",
                    Data = new MegaData<SemesterResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedSemesters.PageNumber,
                            Size = pagedSemesters.PageSize,
                            Sort = "Ascending",
                            Order = "Name",
                            TotalPage = pagedSemesters.PageCount,
                            TotalItem = pagedSemesters.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.keyWord,
                            role = null,
                            status = model.status
                        },
                        PageData = response,
                    },
                };
            }
            catch (System.Exception ex)
            {
                return new DynamicResponse<SemesterResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null,
                };
            }
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