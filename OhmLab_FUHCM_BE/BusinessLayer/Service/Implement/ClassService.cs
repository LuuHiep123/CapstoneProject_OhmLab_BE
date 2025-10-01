using AutoMapper;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.ResponseModel.User;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
        public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassUserRepository _classUserRepository;
        private readonly IScheduleTypeRepository _scheduleTypeRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly ILabRepository _labRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly db_abadcb_ohmlabContext _dbContext;
        private readonly IMapper _mapper;

        public ClassService(IScheduleRepository scheduleRepository, ISemesterSubjectRepository semesterSubjectRepository, ISemesterRepository semesterRepository, ISubjectRepository subjectRepository, IScheduleTypeRepository scheduleTypeRepository, IClassRepository classRepository, IClassUserRepository classUserRepository, ILabRepository labRepository, IUserRepository userRepository, IReportRepository reportRepository, ITeamRepository teamRepository, db_abadcb_ohmlabContext dbContext, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _semesterRepository = semesterRepository;
            _subjectRepository = subjectRepository;
            _scheduleTypeRepository = scheduleTypeRepository;
            _classRepository = classRepository;
            _classUserRepository = classUserRepository;
            _labRepository = labRepository;
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _teamRepository = teamRepository;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClassResponseModel>> CreateClassAsync(CreateClassRequestModel model)
        {
            try
            {
                // Validation cho SubjectId
                if (model.SubjectId <= 0)
                    {
                        return new BaseResponse<ClassResponseModel>
                        {
                            Code = 400,
                            Success = false,
                        Message = "SubjectId không hợp lệ!",
                            Data = null
                        };
                    }

                var subject = await _subjectRepository.GetSubjectById(model.SubjectId);
                if (subject == null)
                    {
                        return new BaseResponse<ClassResponseModel>
                        {
                            Code = 400,
                            Success = false,
                        Message = "Không tìm thấy môn học với ID đã cung cấp!",
                            Data = null
                        };
                }

                if (subject.SubjectStatus.ToLower() != "active")
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Môn học này không còn hoạt động!",
                        Data = null
                    };
                }

                // Validation cho LecturerId nếu có
                if (model.LecturerId.HasValue)
                {
                    var lecturer = await _userRepository.GetUserById(model.LecturerId.Value);
                    if (lecturer == null)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                            Code = 400,
                        Success = false,
                            Message = "Không tìm thấy giảng viên với ID đã cung cấp!",
                        Data = null
                    };
                }

                    if (lecturer.UserRoleName.ToLower() != "lecturer")
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                            Code = 400,
                        Success = false,
                            Message = "Người dùng này không phải là giảng viên!",
                        Data = null
                    };
                }
                }
                // Validation cho ScheduleTypeId nếu có và khác 0
                if (model.ScheduleTypeId.HasValue && model.ScheduleTypeId.Value > 0)
                {
                    var scheduleTypeCheck = await _scheduleTypeRepository.GetByIdAsync(model.ScheduleTypeId.Value);
                    if (scheduleTypeCheck == null)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                            Code = 400,
                        Success = false,
                            Message = "Không tìm thấy loại lịch học với ID đã cung cấp!",
                        Data = null
                    };
                }

                    if (scheduleTypeCheck.ScheduleTypeStatus.ToLower() != "active")
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                            Code = 400,
                        Success = false,
                            Message = "Loại lịch học này không còn hoạt động!",
                        Data = null
                    };
                }
                }

                // Validation cho ClassName
                if (string.IsNullOrWhiteSpace(model.ClassName))
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Tên lớp học không được để trống!",
                        Data = null
                    };
                }

                if (model.ClassName.Length > 50)
                {
                        return new BaseResponse<ClassResponseModel>
                        {
                        Code = 400,
                        Success = false,
                        Message = "Tên lớp học không được vượt quá 50 ký tự!",
                        Data = null
                    };
                }

                var classCheckName = await _classRepository.GetByName(model.ClassName);
                if(classCheckName != null)
                {
                    return new BaseResponse<ClassResponseModel>() 
                    {
                        Code = 409,
                        Success = false,
                        Message = "Tên lớp học đã tồn tại!",
                        Data = null
                    };
                }

                var classEntity = new Class
                {
                    SubjectId = model.SubjectId,
                    LecturerId = model.LecturerId,
                    ScheduleTypeId = model.ScheduleTypeId.HasValue && model.ScheduleTypeId.Value > 0 ? model.ScheduleTypeId : null,
                    ClassName = model.ClassName,
                    ClassDescription = model.ClassDescription,
                    ClassStatus = "Active"
                };


                // Kiểm tra ScheduleType đã được sử dụng bởi lớp khác chưa (chỉ khi có ScheduleTypeId và khác 0)
                if (model.ScheduleTypeId.HasValue && model.ScheduleTypeId.Value > 0)
                {
                    var listClass = await _classRepository.GetAllAsync();
                    var existingClass = listClass.FirstOrDefault(c => c.ScheduleTypeId == model.ScheduleTypeId && c.ClassStatus.Equals("Active"));
                    if (existingClass != null)
                    {
                        return new BaseResponse<ClassResponseModel>
                        {
                            Code = 409,
                            Success = false,
                            Message = $"Lịch học này đã được sử dụng bởi lớp {existingClass.ClassName}!",
                            Data = null
                        };
                    }
                }

                var result = await _classRepository.CreateAsync(classEntity);

                // Lấy lại class với đầy đủ thông tin navigation properties
                var classWithDetails = await _classRepository.GetByIdAsync(result.ClassId);
                

                
                var response = _mapper.Map<ClassResponseModel>(classWithDetails);
                
                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }
                
                if (currentSemester != null)
                {
                    response.SemesterName = currentSemester.SemesterName;
                    response.SemesterStartDate = currentSemester.SemesterStartDate;
                    response.SemesterEndDate = currentSemester.SemesterEndDate;
                }
                string message = "Tạo lớp học thành công!";
                if (model.ScheduleTypeId.HasValue && model.ScheduleTypeId.Value > 0)
                {
                    message += " Đã gán loại lịch học cho lớp.";
                }
                else
                {
                    message += " Bạn có thể thêm lịch học sau.";
                }

                        return new BaseResponse<ClassResponseModel>
                        {
                            Code = 200,
                            Success = true,
                    Message = message,
                            Data = response
                };

            }
            catch (System.Exception ex)
            {
                return new BaseResponse<ClassResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ClassResponseModel>> GetClassByIdAsync(int id)
        {
            try
            {
                // Validation cho ID
                if (id <= 0)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ID lớp học không hợp lệ!",
                        Data = null
                    };
                }

                var classEntity = await _classRepository.GetByIdAsync(id);
                if (classEntity == null)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                var response = _mapper.Map<ClassResponseModel>(classEntity);
                
                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }
                
                if (currentSemester != null)
                {
                    response.SemesterName = currentSemester.SemesterName;
                    response.SemesterStartDate = currentSemester.SemesterStartDate;
                    response.SemesterEndDate = currentSemester.SemesterEndDate;
                }
                
                // Lấy danh sách ClassUsers
                var classUsers = await _classUserRepository.GetByClassIdAsync(id);
                response.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);

                return new BaseResponse<ClassResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin lớp học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<ClassResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ClassResponseModel>> GetAllClassesAsync()
        {
            try
            {
                var classes = await _classRepository.GetAllAsync();
                var response = _mapper.Map<List<ClassResponseModel>>(classes);

                // Lấy ClassUsers cho từng class
                foreach (var classResponse in response)
                {
                    var classUsers = await _classUserRepository.GetByClassIdAsync(classResponse.ClassId);
                    classResponse.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);
                }

                // Thực hiện phân trang với giá trị mặc định
                var pagedClasses = response
                    .OrderBy(c => c.ClassName) // Sắp xếp theo tên lớp học
                    .ToPagedList(1, 1000); // Mặc định page 1, size 10

                return new DynamicResponse<ClassResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học thành công!",
                    Data = new MegaData<ClassResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedClasses.PageNumber,
                            Size = pagedClasses.PageSize,
                            Sort = "Ascending",
                            Order = "Name",
                            TotalPage = pagedClasses.PageCount,
                            TotalItem = pagedClasses.TotalItemCount,
                        },
                        SearchInfo = null,
                        PageData = pagedClasses.ToList(),
                    },
                };
            }
            catch (System.Exception ex)
            {
                return new DynamicResponse<ClassResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null,
                };
            }
        }

        public async Task<DynamicResponse<ClassResponseModel>> GetAllClassesAsync(GetAllClassRequestModel model)
        {
            try
            {
                var classes = await _classRepository.GetAllAsync();
                var listClass = classes.ToList();

                // Lọc theo keyword nếu có
                if (!string.IsNullOrEmpty(model.SearchTerm))
                {
                    listClass = listClass.Where(c => c.ClassName.ToLower().Contains(model.SearchTerm.ToLower()) ||
                                                    (!string.IsNullOrEmpty(c.ClassDescription) && c.ClassDescription.ToLower().Contains(model.SearchTerm.ToLower())))
                                       .ToList();
                }

                // Lọc theo status nếu có
                if (!string.IsNullOrEmpty(model.Status))
                {
                    listClass = listClass.Where(c => c.ClassStatus.ToLower().Equals(model.Status.ToLower())).ToList();
                }

                // Lọc theo subjectId nếu có (tạm thời bỏ qua vì không có trong model mới)
                // if (model.subjectId.HasValue)
                // {
                //     listClass = listClass.Where(c => c.SubjectId == model.subjectId.Value).ToList();
                // }

                // Lọc theo lecturerId nếu có (tạm thời bỏ qua vì không có trong model mới)
                // if (model.lecturerId.HasValue)
                // {
                //     listClass = listClass.Where(c => c.LecturerId == model.lecturerId.Value).ToList();
                // }

                var result = _mapper.Map<List<ClassResponseModel>>(listClass);

                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }

                // Lấy ClassUsers cho từng class và cập nhật semester
                foreach (var classResponse in result)
                {
                    var classUsers = await _classUserRepository.GetByClassIdAsync(classResponse.ClassId);
                    classResponse.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);
                    
                    // Cập nhật thông tin semester
                    if (currentSemester != null)
                    {
                        classResponse.SemesterName = currentSemester.SemesterName;
                        classResponse.SemesterStartDate = currentSemester.SemesterStartDate;
                        classResponse.SemesterEndDate = currentSemester.SemesterEndDate;
                    }
                }

                // Thực hiện phân trang
                var page = model.Page ?? 1;
                var pageSize = model.PageSize ?? 10;
                var pagedClasses = result
                    .OrderBy(c => c.ClassName) // Sắp xếp theo tên lớp học
                    .ToPagedList(page, pageSize);

                return new DynamicResponse<ClassResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học thành công!",
                    Data = new MegaData<ClassResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedClasses.PageNumber,
                            Size = pagedClasses.PageSize,
                            Sort = "Ascending",
                            Order = "Name",
                            TotalPage = pagedClasses.PageCount,
                            TotalItem = pagedClasses.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.SearchTerm,
                            role = null,
                            status = model.Status,
                        },
                        PageData = pagedClasses.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<ClassResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<List<ClassResponseModel>>> GetClassesByLecturerIdAsync(Guid lecturerId)
        {
            try
            {
                // Validation cho LecturerId
                if (lecturerId == Guid.Empty)
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "LecturerId không hợp lệ!",
                        Data = null
                    };
                }

                // Kiểm tra lecturer có tồn tại không
                var lecturer = await _userRepository.GetUserById(lecturerId);
                if (lecturer == null)
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy giảng viên!",
                        Data = null
                    };
                }

                if (lecturer.UserRoleName.ToLower() != "lecturer")
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Người dùng này không phải là giảng viên!",
                        Data = null
                    };
                }

                var classes = await _classRepository.GetByLecturerIdAsync(lecturerId);
                var response = _mapper.Map<List<ClassResponseModel>>(classes);

                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }

                // Lấy ClassUsers cho từng class và cập nhật semester
                foreach (var classResponse in response)
                {
                    var classUsers = await _classUserRepository.GetByClassIdAsync(classResponse.ClassId);
                    classResponse.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);
                    
                    // Cập nhật thông tin semester
                    if (currentSemester != null)
                    {
                        classResponse.SemesterName = currentSemester.SemesterName;
                        classResponse.SemesterStartDate = currentSemester.SemesterStartDate;
                        classResponse.SemesterEndDate = currentSemester.SemesterEndDate;
                    }
                }

                return new BaseResponse<List<ClassResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học theo giảng viên thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<ClassResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<ClassResponseModel>>> GetClassesByStudentIdAsync(Guid studentId)
        {
            try
            {
                // Validation cho StudentId
                if (studentId == Guid.Empty)
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "StudentId không hợp lệ!",
                        Data = null
                    };
                }

                // Kiểm tra student có tồn tại không
                var student = await _userRepository.GetUserById(studentId);
                if (student == null)
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy sinh viên!",
                        Data = null
                    };
                }

                if (student.UserRoleName.ToLower() != "student")
                {
                    return new BaseResponse<List<ClassResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Người dùng này không phải là sinh viên!",
                        Data = null
                    };
                }

                var classes = await _classRepository.GetByStudentIdAsync(studentId);
                var response = _mapper.Map<List<ClassResponseModel>>(classes);

                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }

                // Lấy ClassUsers cho từng class và cập nhật semester
                foreach (var classResponse in response)
                {
                    var classUsers = await _classUserRepository.GetByClassIdAsync(classResponse.ClassId);
                    classResponse.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);
                    
                    // Cập nhật thông tin semester
                    if (currentSemester != null)
                    {
                        classResponse.SemesterName = currentSemester.SemesterName;
                        classResponse.SemesterStartDate = currentSemester.SemesterStartDate;
                        classResponse.SemesterEndDate = currentSemester.SemesterEndDate;
                    }
                }

                return new BaseResponse<List<ClassResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học theo sinh viên thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<ClassResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ClassResponseModel>> UpdateClassAsync(int id, UpdateClassRequestModel model)
        {
            try
            {
                // Validation cho ID
                if (id <= 0)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ID lớp học không hợp lệ!",
                        Data = null
                    };
                }

                var existingClass = await _classRepository.GetByIdAsync(id);
                if (existingClass == null)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                // Validation cho SubjectId
                if (model.SubjectId <= 0)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "SubjectId không hợp lệ!",
                        Data = null
                    };
                }

                var subject = await _subjectRepository.GetSubjectById(model.SubjectId);
                if (subject == null)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không tìm thấy môn học với ID đã cung cấp!",
                        Data = null
                    };
                }

              

                // Validation cho ClassName
                if (string.IsNullOrWhiteSpace(model.ClassName))
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Tên lớp học không được để trống!",
                        Data = null
                    };
                }

                if (model.ClassName.Length > 50)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Tên lớp học không được vượt quá 50 ký tự!",
                        Data = null
                    };
                }

                // Kiểm tra tên lớp học trùng lặp (trừ lớp hiện tại)
                var classCheckName = await _classRepository.GetByName(model.ClassName);
                if (classCheckName != null && classCheckName.ClassId != id)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Tên lớp học đã tồn tại!",
                        Data = null
                    };
                }
                if (!model.LecturerId.HasValue)
                {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "LecturerId không được để trống!",
                    };
                }
                var existinglecturer = await _userRepository.GetUserById(model.LecturerId.Value);
                if (existingClass == null) {
                    return new BaseResponse<ClassResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không tìm thấy giảng viên với ID đã cung cấp!",
                    };
}
                existingClass.SubjectId = model.SubjectId;
                existingClass.ClassName = model.ClassName;
                existingClass.ClassDescription = model.ClassDescription;
                existingClass.ClassStatus = model.ClassStatus;
                existingClass.LecturerId = model.LecturerId;
                
                
                // Cập nhật ScheduleTypeId
                if (model.ScheduleTypeId.HasValue && model.ScheduleTypeId.Value > 0)
                {
                    // Kiểm tra ScheduleType tồn tại
                    var scheduleType = await _scheduleTypeRepository.GetByIdAsync(model.ScheduleTypeId.Value);
                    if (scheduleType == null)
                    {
                        return new BaseResponse<ClassResponseModel>
                        {
                            Code = 400,
                            Success = false,
                            Message = $"Không tìm thấy loại lịch học với ID: {model.ScheduleTypeId.Value}",
                            Data = null
                        };
                    }
                    existingClass.ScheduleTypeId = model.ScheduleTypeId.Value;
                }
                else
                {
                    existingClass.ScheduleTypeId = null;
                }

                var result = await _classRepository.UpdateAsync(existingClass);
                
                // Lấy lại class với đầy đủ thông tin navigation properties
                var classWithDetails = await _classRepository.GetByIdAsync(result.ClassId);
                var response = _mapper.Map<ClassResponseModel>(classWithDetails);
                
                // Lấy thông tin semester trực tiếp
                var allSemesters = await _semesterRepository.GetAllAsync();
                var currentSemester = allSemesters.FirstOrDefault(s => s.SemesterStatus.ToLower() == "active");
                if (currentSemester == null)
                {
                    currentSemester = allSemesters.FirstOrDefault();
                }
                
                if (currentSemester != null)
                {
                    response.SemesterName = currentSemester.SemesterName;
                    response.SemesterStartDate = currentSemester.SemesterStartDate;
                    response.SemesterEndDate = currentSemester.SemesterEndDate;
                }

                return new BaseResponse<ClassResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật lớp học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<ClassResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateClassStatusAsync(int id, string status)
        {
            try
            {
                // Validation cho ID
                if (id <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ID lớp học không hợp lệ!",
                        Data = false
                    };
                }

                // Validation cho status
                if (string.IsNullOrWhiteSpace(status))
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Status không được để trống!",
                        Data = false
                    };
                }

                var validStatuses = new[] { "Active", "Inactive", "Deleted" };
                if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Status không hợp lệ! Chỉ chấp nhận: Active, Inactive, Deleted",
                        Data = false
                    };
                }

                var existingClass = await _classRepository.GetByIdAsync(id);
                if (existingClass == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = false
                    };
                }

                // Kiểm tra nếu đang chuyển từ Active sang Inactive
                if (existingClass.ClassStatus.ToLower() == "active" && status.ToLower() == "inactive")
                {
                    // Có thể thêm logic kiểm tra nếu cần
                    // Ví dụ: kiểm tra xem có lịch học nào đang diễn ra không
                }

                // Cập nhật status
                existingClass.ClassStatus = status;
                var result = await _classRepository.UpdateAsync(existingClass);

                var statusText = status.ToLower() == "active" ? "kích hoạt" : 
                               status.ToLower() == "inactive" ? "tạm ngưng" : "xóa";

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = $"Cập nhật trạng thái lớp học thành công! Lớp học đã được {statusText}.",
                    Data = result != null
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<bool>> AddScheduleForClassAsync(AddScheduleForClassRequestModel model)
        {
            try
            {
                // Validation cho ClassId
                if (model.ClassId <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ClassId không hợp lệ!",
                        Data = false
                    };
                }

                // Validation cho ScheduleTypeId
                if (model.ScheduleTypeId <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ScheduleTypeId không hợp lệ!",
                        Data = false
                    };
                }

                var Class = await _classRepository.GetByIdAsync(model.ClassId);
                if (Class == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = false
                    };
                }

                // Kiểm tra trạng thái lớp học
                if (Class.ClassStatus.ToLower() != "active")
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Lớp học này không còn hoạt động!",
                        Data = false
                    };
                }

                // Kiểm tra lớp đã có lịch chưa
                if (Class.ScheduleTypeId != null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Lớp đã có lịch học! Bạn có thể cập nhật lịch học thay vì thêm mới.",
                        Data = false
                    };
                }

                // Kiểm tra ScheduleType đã được sử dụng bởi lớp khác chưa
                var listClass = await _classRepository.GetAllAsync();
                var existingClass = listClass.FirstOrDefault(c => c.ScheduleTypeId == model.ScheduleTypeId && c.ClassStatus.Equals("Active"));
                if (existingClass != null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = $"Lịch học này đã được sử dụng bởi lớp {existingClass.ClassName}!",
                        Data = false
                    };
                }

                // Lấy thông tin ScheduleType
                var scheduleType = await _scheduleTypeRepository.GetByIdAsync(model.ScheduleTypeId);
                if (scheduleType == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy loại lịch học!",
                        Data = false
                    };
                }
                var subject = await _subjectRepository.GetSubjectById(Class.SubjectId);
                if (subject == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy môn!",
                        Data = false
                    };
                }
                // Lấy semester cua class
                var currentSemester = await _semesterRepository.GetByIdAsync(Class.Subject.SemesterSubjects.FirstOrDefault().SemesterId);
                
                if (currentSemester == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy học kỳ nào trong hệ thống!",
                        Data = false
                    };
                }
                if (scheduleType.ScheduleTypeDow.ToLower().Contains("mon"))
                {
                    var startDate = currentSemester.SemesterStartDate;
                    for (var i = 0; i < 10; i++)
                    {
                        var schedule = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule);

                        startDate = startDate.AddDays(3);
                        var schedule1 = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule1);
                        startDate = startDate.AddDays(4);
                    }
                    Class.ScheduleTypeId = model.ScheduleTypeId;
                    await _classRepository.UpdateAsync(Class);
                    return new BaseResponse<bool>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "add schedule success!",
                        Data = true
                    };
                }
                if (scheduleType.ScheduleTypeDow.ToLower().Contains("tue"))
                {
                    var startDate = currentSemester.SemesterStartDate.AddDays(1);
                    for (var i = 0; i < 10; i++)
                    {
                        var schedule = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule);

                        startDate = startDate.AddDays(3);
                        var schedule1 = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule1);

                        startDate = startDate.AddDays(4);
                    }

                    Class.ScheduleTypeId = model.ScheduleTypeId;
                    await _classRepository.UpdateAsync(Class);

                    return new BaseResponse<bool>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "add schedule success!",
                        Data = true
                    };
                }
                if (scheduleType.ScheduleTypeDow.ToLower().Contains("wed"))
                {
                    var startDate = currentSemester.SemesterStartDate.AddDays(2);
                    for (var i = 0; i < 10; i++)
                    {
                        var schedule = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule);

                        startDate = startDate.AddDays(3);
                        var schedule1 = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = "schedule",
                            ScheduleDescription = "schedule for Lab room FPT HCM",
                            ScheduleDate = startDate,
                        };
                        await _scheduleRepository.CreateAsync(schedule1);
                        startDate = startDate.AddDays(4);
                    }

                    Class.ScheduleTypeId = model.ScheduleTypeId;
                    await _classRepository.UpdateAsync(Class);

                    return new BaseResponse<bool>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "add schedule success!",
                        Data = true
                    };
                }

                return new BaseResponse<bool>()
                {
                    Code = 401,
                    Success = true,
                    Message = "xếp lịch thất bại",
                    Data = true
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = false
                };
            }
        }
            
        public async Task<BaseResponse<List<LabResponseModel>>> GetLabsByClassIdAsync(int classId)
        {
            try
            {
                // Validation cho ClassId
                if (classId <= 0)
                {
                    return new BaseResponse<List<LabResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "ClassId không hợp lệ!",
                        Data = null
                    };
                }

                var Class = await _classRepository.GetByIdAsync(classId);
                if (Class == null)
                {
                    return new BaseResponse<List<LabResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                // Kiểm tra trạng thái lớp học
                if (Class.ClassStatus.ToLower() != "active")
                {
                    return new BaseResponse<List<LabResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Lớp học này không còn hoạt động!",
                        Data = null
                    };
                }

                // Lấy danh sách lab của môn học
                var labs = await _labRepository.GetLabsBySubjectId(Class.SubjectId);
                var activeLabs = labs.Where(l => l.LabStatus.ToLower() == "active").ToList();
                
                var labResponses = _mapper.Map<List<LabResponseModel>>(activeLabs);

                return new BaseResponse<List<LabResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = $"Lấy danh sách lab thành công! Tìm thấy {activeLabs.Count} bài lab.",
                    Data = labResponses
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<LabResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        private DateTime? GetStartDateByDayOfWeek(DateTime semesterStartDate, string dayOfWeek)
        {
            var dow = dayOfWeek.ToLower();
            var currentDate = semesterStartDate;

            // Tìm ngày đầu tiên của ngày trong tuần từ ngày bắt đầu học kỳ
            for (int i = 0; i < 7; i++)
            {
                var checkDate = currentDate.AddDays(i);
                var dayName = checkDate.DayOfWeek.ToString().ToLower();

                if (dow.Contains("mon") && dayName == "monday") return checkDate;
                if (dow.Contains("tue") && dayName == "tuesday") return checkDate;
                if (dow.Contains("wed") && dayName == "wednesday") return checkDate;
                if (dow.Contains("thu") && dayName == "thursday") return checkDate;
                if (dow.Contains("fri") && dayName == "friday") return checkDate;
                if (dow.Contains("sat") && dayName == "saturday") return checkDate;
                if (dow.Contains("sun") && dayName == "sunday") return checkDate;
            }

            return null;
        }
    }
} 