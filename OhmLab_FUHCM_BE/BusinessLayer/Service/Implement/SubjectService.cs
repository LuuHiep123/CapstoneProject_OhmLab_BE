using AutoMapper;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Subject;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.ResponseModel.Semester;
using DataLayer.DBContext;

namespace BusinessLayer.Service.Implement
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly IMapper _mapper;
        private readonly db_abadcb_ohmlabContext _DBContext;

        public SubjectService(ISubjectRepository subjectRepository, 
                            IClassRepository classRepository, 
                            ISemesterRepository semesterRepository,
                            ISemesterSubjectRepository semesterSubjectRepository,
                                                         IMapper mapper,
                             db_abadcb_ohmlabContext DBContext)
        {
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
            _semesterRepository = semesterRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _mapper = mapper;
            _DBContext = DBContext;
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
                    SemesterSubject1 = "valid"
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

        // ✅ THÊM MỚI: Method debug để kiểm tra tại sao semester bị null
        public async Task<string> DebugSubjectSemester(int subjectId)
        {
            try
            {
                var debugInfo = new List<string>();
                
                // 1. Kiểm tra Subject có tồn tại không
                var subject = await _subjectRepository.GetSubjectById(subjectId);
                if (subject == null)
                {
                    return $"❌ Subject với ID {subjectId} không tồn tại!";
                }
                debugInfo.Add($"✅ Subject tồn tại: {subject.SubjectName} ({subject.SubjectCode})");

                // 2. Kiểm tra tất cả record trong SemesterSubject
                var allSemesterSubjects = await _DBContext.SemesterSubjects
                    .Where(ss => ss.SubjectId == subjectId)
                    .ToListAsync();
                
                if (!allSemesterSubjects.Any())
                {
                    debugInfo.Add($"❌ Không có record nào trong SemesterSubject với SubjectId = {subjectId}");
                    debugInfo.Add("💡 Cần tạo record SemesterSubject cho subject này!");
                }
                else
                {
                    debugInfo.Add($"📊 Tìm thấy {allSemesterSubjects.Count} record trong SemesterSubject:");
                    foreach (var ss in allSemesterSubjects)
                    {
                        debugInfo.Add($"   - ID: {ss.SemesterSubjectId}, SemesterId: {ss.SemesterId}, Status: '{ss.SemesterSubject1}'");
                    }
                }

                // 3. Kiểm tra record "valid"
                var validSemesterSubject = await _semesterSubjectRepository.GetBySubjectIdAsync(subjectId);
                if (validSemesterSubject == null)
                {
                    debugInfo.Add($"❌ Không có record nào với SemesterSubject1 = 'valid'");
                    debugInfo.Add("💡 Cần cập nhật status hoặc tạo record mới!");
                }
                else
                {
                    debugInfo.Add($"✅ Tìm thấy record valid: SemesterId = {validSemesterSubject.SemesterId}");
                    
                    // 4. Kiểm tra Semester có tồn tại không
                    var semester = await _semesterRepository.GetByIdAsync(validSemesterSubject.SemesterId);
                    if (semester == null)
                    {
                        debugInfo.Add($"❌ Semester với ID {validSemesterSubject.SemesterId} không tồn tại!");
                    }
                    else
                    {
                        debugInfo.Add($"✅ Semester tồn tại: {semester.SemesterName}");
                    }
                }

                return string.Join("\n", debugInfo);
            }
            catch (Exception ex)
            {
                return $"❌ Lỗi khi debug: {ex.Message}";
            }
        }

        // ✅ THÊM MỚI: Method để fix vấn đề semester null
        public async Task<bool> FixSubjectSemester(int subjectId, int semesterId)
        {
            try
            {
                // 1. Kiểm tra Subject và Semester có tồn tại không
                var subject = await _subjectRepository.GetSubjectById(subjectId);
                if (subject == null)
                {
                    throw new ArgumentException($"Subject với ID {subjectId} không tồn tại!");
                }

                var semester = await _semesterRepository.GetByIdAsync(semesterId);
                if (semester == null)
                {
                    throw new ArgumentException($"Semester với ID {semesterId} không tồn tại!");
                }

                // 2. Kiểm tra xem đã có record SemesterSubject chưa
                var existingRecord = await _DBContext.SemesterSubjects
                    .FirstOrDefaultAsync(ss => ss.SubjectId == subjectId);

                if (existingRecord != null)
                {
                    // Cập nhật record hiện tại
                    existingRecord.SemesterId = semesterId;
                    existingRecord.SemesterSubject1 = "valid";
                    _DBContext.SemesterSubjects.Update(existingRecord);
                }
                else
                {
                    // Tạo record mới
                    var newSemesterSubject = new SemesterSubject
                    {
                        SubjectId = subjectId,
                        SemesterId = semesterId,
                        SemesterSubject1 = "valid"
                    };
                    _DBContext.SemesterSubjects.Add(newSemesterSubject);
                }

                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi fix semester cho subject: {ex.Message}");
            }
        }

        // ✅ THÊM MỚI: Method để lấy tất cả semester có sẵn
        public async Task<List<SemesterResponseModel>> GetAllAvailableSemesters()
        {
            try
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
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách semester: {ex.Message}");
            }
        }
    }
} 