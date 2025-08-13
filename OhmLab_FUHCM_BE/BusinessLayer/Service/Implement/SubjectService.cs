using AutoMapper;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Subject;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, 
                            IClassRepository classRepository, 
                            ISemesterRepository semesterRepository,
                            ISemesterSubjectRepository semesterSubjectRepository,
                            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
            _semesterRepository = semesterRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _mapper = mapper;
        }

        public async Task AddSubject(CreateSubjectRequestModel subjectModel)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(subjectModel.SubjectName))
                {
                    throw new ArgumentException("Tên môn học không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(subjectModel.SubjectCode))
                {
                    throw new ArgumentException("Mã môn học không được để trống!");
                }

                if (subjectModel.SemesterId <= 0)
                {
                    throw new ArgumentException("Semester ID phải lớn hơn 0!");
                }

                // Kiểm tra semester tồn tại
                var semester = await _semesterRepository.GetByIdAsync(subjectModel.SemesterId);
                if (semester == null)
                {
                    throw new ArgumentException($"Không tìm thấy semester với ID: {subjectModel.SemesterId}");
                }

                // Tạo subject
                var subject = _mapper.Map<Subject>(subjectModel);
                subject.SubjectStatus = "Active"; // Default status
                await _subjectRepository.AddSubject(subject);

                // Tạo SemesterSubject relationship
                var semesterSubject = new SemesterSubject
                {
                    SubjectId = subject.SubjectId,
                    SemesterId = subjectModel.SemesterId,
                    SemesterSubject1 = $"{semester.SemesterName} - {subject.SubjectName}"
                };
                await _semesterSubjectRepository.AddAsync(semesterSubject);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo môn học: {ex.Message}");
            }
        }

        public async Task DeleteSubject(int id)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject != null)
            {
                // Kiểm tra Subject có đang được sử dụng bởi Class nào không
                var classes = await _classRepository.GetAllAsync();
                var usingClasses = classes.Where(c => c.SubjectId == id).ToList();
                
                if (usingClasses.Any())
                {
                    // Nếu có Class đang sử dụng, không cho phép xóa
                    throw new System.InvalidOperationException($"Không thể xóa môn học này vì đang được sử dụng bởi {usingClasses.Count} lớp học!");
                }

                subject.SubjectStatus = "Inactive";
                await _subjectRepository.UpdateSubject(subject);
            }
        }

        public async Task<SubjectResponseModel> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject == null)
                return null;

            // Lấy thông tin semester
            var semesterSubject = await _semesterSubjectRepository.GetBySubjectIdAsync(id);
            var semester = semesterSubject != null ? await _semesterRepository.GetByIdAsync(semesterSubject.SemesterId) : null;

            return new SubjectResponseModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                SubjectCode = subject.SubjectCode,
                SubjectDescription = subject.SubjectDescription,
                SubjectStatus = subject.SubjectStatus,
                SemesterId = semester?.SemesterId,
                SemesterName = semester?.SemesterName
            };
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjects();
            var subjectResponses = new List<SubjectResponseModel>();

            foreach (var subject in subjects)
            {
                // Lấy thông tin semester cho mỗi subject
                var semesterSubject = await _semesterSubjectRepository.GetBySubjectIdAsync(subject.SubjectId);
                var semester = semesterSubject != null ? await _semesterRepository.GetByIdAsync(semesterSubject.SemesterId) : null;

                subjectResponses.Add(new SubjectResponseModel
                {
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    SubjectCode = subject.SubjectCode,
                    SubjectDescription = subject.SubjectDescription,
                    SubjectStatus = subject.SubjectStatus,
                    SemesterId = semester?.SemesterId,
                    SemesterName = semester?.SemesterName
                });
            }

            return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách môn học thành công!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<SubjectResponseModel>
                {
                    PageData = subjectResponses,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = subjectResponses.Count,
                        TotalItem = subjectResponses.Count,
                        TotalPage = 1
                    },
                    SearchInfo = null
                }
            };
        }

        public async Task UpdateSubject(int id, UpdateSubjectRequestModel subjectModel)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject != null)
            {
                _mapper.Map(subjectModel, subject);
                subject.SubjectId = id;
                await _subjectRepository.UpdateSubject(subject);
            }
        }
    }
} 