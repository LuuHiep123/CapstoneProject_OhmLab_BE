    using AutoMapper;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class ClassUserService : IClassUserService
    {
        private readonly IClassUserRepository _classUserRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepositoty _userRepository;
        private readonly IMapper _mapper;

        public ClassUserService(IClassUserRepository classUserRepository, IClassRepository classRepository, IUserRepositoty userRepository, IMapper mapper)
        {
            _classUserRepository = classUserRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClassUserResponseModel>> AddUserToClassAsync(Guid userId, int classId)
        {
            try
            {
                // Kiểm tra class có tồn tại không
                var classEntity = await _classRepository.GetByIdAsync(classId);
                if (classEntity == null)
                {
                    return new BaseResponse<ClassUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                // Kiểm tra user có tồn tại không
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<ClassUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy người dùng!",
                        Data = null
                    };
                }

                // Kiểm tra user đã trong class chưa
                bool isUserInClass = await _classUserRepository.IsUserInClassAsync(userId, classId);
                if (isUserInClass)
                {
                    return new BaseResponse<ClassUserResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Người dùng đã có trong lớp học này!",
                        Data = null
                    };
                }

                var classUser = new ClassUser
                {
                    ClassId = classId,
                    UserId = userId
                };

                var result = await _classUserRepository.CreateAsync(classUser);
                var response = _mapper.Map<ClassUserResponseModel>(result);

                return new BaseResponse<ClassUserResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm người dùng vào lớp học thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassUserResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ClassUserResponseModel>> GetClassUserByIdAsync(int id)
        {
            try
            {
                var classUser = await _classUserRepository.GetByIdAsync(id);
                if (classUser == null)
                {
                    return new BaseResponse<ClassUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thông tin!",
                        Data = null
                    };
                }

                var response = _mapper.Map<ClassUserResponseModel>(classUser);
                return new BaseResponse<ClassUserResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassUserResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<ClassUserResponseModel>>> GetClassUsersByClassIdAsync(int classId)
        {
            try
            {
                var classUsers = await _classUserRepository.GetByClassIdAsync(classId);
                var response = _mapper.Map<List<ClassUserResponseModel>>(classUsers);

                return new BaseResponse<List<ClassUserResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách người dùng trong lớp thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ClassUserResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<ClassUserResponseModel>>> GetClassUsersByUserIdAsync(Guid userId)
        {
            try
            {
                var classUsers = await _classUserRepository.GetByUserIdAsync(userId);
                var response = _mapper.Map<List<ClassUserResponseModel>>(classUsers);

                return new BaseResponse<List<ClassUserResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp học của người dùng thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ClassUserResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> RemoveUserFromClassAsync(Guid userId, int classId)
        {
            try
            {
                // Kiểm tra user có trong class không
                bool isUserInClass = await _classUserRepository.IsUserInClassAsync(userId, classId);
                if (!isUserInClass)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Người dùng không có trong lớp học này!",
                        Data = false
                    };
                }

                // Tìm ClassUser để xóa
                var classUsers = await _classUserRepository.GetByClassIdAsync(classId);
                var classUserToDelete = classUsers.FirstOrDefault(cu => cu.UserId == userId);
                
                if (classUserToDelete != null)
                {
                    await _classUserRepository.DeleteAsync(classUserToDelete.ClassUserId);
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa người dùng khỏi lớp học thành công!",
                    Data = true
                };
            }
            catch (Exception ex)
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

        public async Task<BaseResponse<bool>> IsUserInClassAsync(Guid userId, int classId)
        {
            try
            {
                bool isUserInClass = await _classUserRepository.IsUserInClassAsync(userId, classId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Kiểm tra thành công!",
                    Data = isUserInClass
                };
            }
            catch (Exception ex)
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