using AutoMapper;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.ResponseModel.User;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;

        public ClassService(IScheduleRepository scheduleRepository, ISemesterSubjectRepository semesterSubjectRepository, ISemesterRepository semesterRepository, ISubjectRepository subjectRepository, IScheduleTypeRepository scheduleTypeRepository, IClassRepository classRepository, IClassUserRepository classUserRepository, ILabRepository labRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _semesterRepository = semesterRepository;
            _subjectRepository = subjectRepository;
            _scheduleTypeRepository = scheduleTypeRepository;
            _classRepository = classRepository;
            _classUserRepository = classUserRepository;
            _labRepository = labRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClassResponseModel>> CreateClassAsync(CreateClassRequestModel model)
        {
            try
            {
                var classEntity = new Class
                {
                    SubjectId = model.SubjectId,
                    ClassName = model.ClassName,
                    ClassDescription = model.ClassDescription,
                    ClassStatus = "Valid"
                };

                var result = await _classRepository.CreateAsync(classEntity);
                var response = _mapper.Map<ClassResponseModel>(result);

                return new BaseResponse<ClassResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo lớp học thành công!",
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

        public async Task<BaseResponse<List<ClassResponseModel>>> GetAllClassesAsync()
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

                return new BaseResponse<List<ClassResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học thành công!",
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

        public async Task<BaseResponse<List<ClassResponseModel>>> GetClassesByLecturerIdAsync(Guid lecturerId)
        {
            try
            {
                var classes = await _classRepository.GetByLecturerIdAsync(lecturerId);
                var response = _mapper.Map<List<ClassResponseModel>>(classes);

                // Lấy ClassUsers cho từng class
                foreach (var classResponse in response)
                {
                    var classUsers = await _classUserRepository.GetByClassIdAsync(classResponse.ClassId);
                    classResponse.ClassUsers = _mapper.Map<List<ClassUserResponseModel>>(classUsers);
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

        public async Task<BaseResponse<ClassResponseModel>> UpdateClassAsync(int id, UpdateClassRequestModel model)
        {
            try
            {
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

                existingClass.SubjectId = model.SubjectId;
                existingClass.ClassName = model.ClassName;
                existingClass.ClassDescription = model.ClassDescription;
                existingClass.ClassStatus = model.ClassStatus;

                var result = await _classRepository.UpdateAsync(existingClass);
                var response = _mapper.Map<ClassResponseModel>(result);

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

        public async Task<BaseResponse<bool>> DeleteClassAsync(int id)
        {
            try
            {
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

                // Kiểm tra Class có đang được sử dụng bởi Schedule nào không
                var schedules = await _scheduleRepository.GetByClassIdAsync(id);
                if (schedules.Any())
                {
                    var scheduleCount = schedules.Count();
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = $"Không thể xóa lớp học này vì đang có {scheduleCount} lịch học được tạo!",
                        Data = false
                    };
                }

                // Kiểm tra Class có đang được sử dụng bởi ClassUser nào không
                var classUsers = await _classUserRepository.GetByClassIdAsync(id);
                if (classUsers.Any())
                {
                    var userCount = classUsers.Count();
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = $"Không thể xóa lớp học này vì đang có {userCount} sinh viên đăng ký!",
                        Data = false
                    };
                }

                var result = await _classRepository.DeleteAsync(id);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa lớp học thành công!",
                    Data = result
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
                var Class = await _classRepository.GetByIdAsync(model.ClassId);
                if(Class == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
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
                        Message = "Lớp đã có lịch học!",
                        Data = false
                    };
                }

                // Kiểm tra ScheduleType đã được sử dụng bởi lớp khác chưa
                var listClass = await _classRepository.GetAllAsync();
                var existingClass = listClass.FirstOrDefault(c => c.ScheduleTypeId == model.ScheduleTypeId);
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

                // Lấy thông tin subject
                var subject = await _subjectRepository.GetSubjectById(Class.SubjectId);
                if (subject == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy môn học!",
                        Data = false
                    };
                }

                // Kiểm tra môn học có lab không
                var labs = await _labRepository.GetLabsBySubjectId(subject.SubjectId);
                if (!labs.Any())
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = $"Môn học {subject.SubjectName} không có bài lab nào! Không thể tạo lịch thực hành.",
                        Data = false
                    };
                }

                // Lọc chỉ những lab có trạng thái Active
                var activeLabs = labs.Where(l => l.LabStatus.ToLower() == "active").ToList();
                if (!activeLabs.Any())
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = $"Môn học {subject.SubjectName} không có bài lab nào đang hoạt động!",
                        Data = false
                    };
                }

                var semesterSubject = await _semesterSubjectRepository.GetBySubjectIdAsync(subject.SubjectId);
                if (semesterSubject == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thông tin học kỳ của môn học!",
                        Data = false
                    };
                }

                var semester = await _semesterRepository.GetByIdAsync(semesterSubject.SemesterId);
                if (semester == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy học kỳ!",
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

                // Xác định ngày bắt đầu dựa trên ngày trong tuần
                var startDate = GetStartDateByDayOfWeek(semester.SemesterStartDate, scheduleType.ScheduleTypeDow);
                if (startDate == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không thể xác định ngày bắt đầu cho lịch học!",
                        Data = false
                    };
                }

                // Tạo lịch thực hành dựa trên số lượng lab có sẵn
                var labCount = activeLabs.Count;
                var weekCount = Math.Min(labCount, 5); // Tối đa 5 tuần

                for (var week = 1; week <= weekCount; week++)
                {
                    var currentLab = activeLabs[week - 1]; // Lấy lab tương ứng với tuần
                    
                    // Buổi 1 trong tuần
                    var schedule1 = new Schedule()
                    {
                        ClassId = model.ClassId,
                        ScheduleName = $"Buổi thực hành {week}.1 - {currentLab.LabName}",
                        ScheduleDescription = $"Buổi thực hành tuần {week} - {currentLab.LabName} - {scheduleType.ScheduleTypeName}",
                        ScheduleDate = startDate.Value.AddDays((week - 1) * 7),
                    };
                    await _scheduleRepository.CreateAsync(schedule1);

                    // Buổi 2 trong tuần (cách 3 ngày) - chỉ tạo nếu còn lab
                    if (week < labCount)
                    {
                        var nextLab = activeLabs[week]; // Lấy lab tiếp theo
                        var schedule2 = new Schedule()
                        {
                            ClassId = model.ClassId,
                            ScheduleName = $"Buổi thực hành {week}.2 - {nextLab.LabName}",
                            ScheduleDescription = $"Buổi thực hành tuần {week} - {nextLab.LabName} - {scheduleType.ScheduleTypeName}",
                            ScheduleDate = startDate.Value.AddDays((week - 1) * 7 + 3),
                        };
                        await _scheduleRepository.CreateAsync(schedule2);
                    }
                }

                // Cập nhật ScheduleTypeId cho lớp
                Class.ScheduleTypeId = model.ScheduleTypeId;
                await _classRepository.UpdateAsync(Class);

                return new BaseResponse<bool>()
                {
                    Code = 200,
                    Success = true,
                    Message = $"Tạo lịch thực hành thành công! Đã tạo {labCount} buổi thực hành cho {labCount} bài lab.",
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