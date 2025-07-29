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
        private readonly IClassUserRepository _classUserRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IClassUserRepository classUserRepository, IMapper mapper)
        {
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
                    LecturerId = model.LecturerId,
                    ScheduleTypeId = model.ScheduleTypeId,
                    ClassName = model.ClassName,
                    ClassDescription = model.ClassDescription,
                    ClassStatus = model.ClassStatus
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

        public async Task<BaseResponse<ClassResponseModel>> UpdateClassAsync(int id, CreateClassRequestModel model)
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
                existingClass.LecturerId = model.LecturerId;
                existingClass.ScheduleTypeId = model.ScheduleTypeId;
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
    }
} 