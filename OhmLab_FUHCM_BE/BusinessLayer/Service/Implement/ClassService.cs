using AutoMapper;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.ResponseModel.User;
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
        private readonly IMapper _mapper;

        public ClassService(IScheduleRepository scheduleRepository, ISemesterSubjectRepository semesterSubjectRepository, ISemesterRepository semesterRepository, ISubjectRepository subjectRepository, IScheduleTypeRepository scheduleTypeRepository, IClassRepository classRepository, IClassUserRepository classUserRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _semesterRepository = semesterRepository;
            _subjectRepository = subjectRepository;   
            _scheduleTypeRepository = scheduleTypeRepository;
            _classRepository = classRepository;
            _classUserRepository = classUserRepository;
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
                        Message = "không tìm thấy lớp",
                        Data = false
                    };
                }
                var listClass = await _classRepository.GetAllAsync();

                if (Class.ScheduleTypeId != null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 401,
                        Success = false,
                        Message = "lớp đã có lịch",
                        Data = false
                    };
                }
                foreach (var item in listClass)
                {
                    if(item.ScheduleTypeId == model.ScheduleTypeId)
                    {
                        return new BaseResponse<bool>
                        {
                            Code = 401,
                            Success = false,
                            Message = "lịch đã có lớp",
                            Data = false
                        };
                    }
                }
                var subject = await _subjectRepository.GetSubjectById(Class.SubjectId);
                var semesterSubject = await _semesterSubjectRepository.GetBySubjectIdAsync(subject.SubjectId);
                var semester = await _semesterRepository.GetByIdAsync(semesterSubject.SemesterId);

                var scheduleType = await _scheduleTypeRepository.GetByIdAsync(model.ScheduleTypeId);
                if (scheduleType.ScheduleTypeDow.ToLower().Contains("mon"))
                {
                    var startDate = semester.SemesterStartDate;
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
                    var startDate = semester.SemesterStartDate.AddDays(1);
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
                    var startDate = semester.SemesterStartDate.AddDays(2);
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
                    Message = "Server Error!",
                    Data = false
                };
            }
        }
    }
} 