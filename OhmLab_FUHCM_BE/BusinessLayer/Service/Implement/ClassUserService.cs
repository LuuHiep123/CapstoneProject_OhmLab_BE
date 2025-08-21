    using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.User;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.RequestModel.Class;
using DataLayer.Repository;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using AutoMapper;

namespace BusinessLayer.Service.Implement
{
    public class ClassUserService : IClassUserService
    {
        private readonly IClassUserRepository _classUserRepository;
        private readonly IUserRepositoty _userRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassUserService(IClassUserRepository classUserRepository, IUserRepositoty userRepository, IClassRepository classRepository, IMapper mapper)
        {
            _classUserRepository = classUserRepository;
            _userRepository = userRepository;
            _classRepository = classRepository;
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

                // Tìm ClassUser để đổi trạng thái
                var classUsers = await _classUserRepository.GetByClassIdAsync(classId);
                var classUserToUpdate = classUsers.FirstOrDefault(cu => cu.UserId == userId);
                
                if (classUserToUpdate != null)
                {
                    
                    classUserToUpdate.ClassUserStatus = "delete";
                    await _classUserRepository.UpdateAsync(classUserToUpdate);
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Đã đổi trạng thái người dùng trong lớp học thành Delete!",
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

        public async Task<BaseResponse<object>> ImportUsersFromExcelAsync(IFormFile file, int classId)
        {
            try
            {
                // Kiểm tra class có tồn tại không
                var classEntity = await _classRepository.GetByIdAsync(classId);
                if (classEntity == null)
                {
                    return new BaseResponse<object>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                // Log thông tin file
                Console.WriteLine($"Import Excel: File size = {file.Length}, ClassId = {classId}");

                var result = new ImportResultResponseModel();
                var successItems = new List<ImportSuccessItem>();
                var errorItems = new List<ImportErrorItem>();

                // Đọc dữ liệu Excel trước
                var excelData = new List<(string studentId, string fullName, string email)>();
                
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    // Set license for EPPlus 8.0.10
                    ExcelPackage.License.SetNonCommercialPersonal("OhmLab System");

                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            
                            // Lấy số dòng có dữ liệu
                            var dimension = worksheet.Dimension;
                            if (dimension == null)
                            {
                                return new BaseResponse<object>
                                {
                                    Code = 400,
                                    Success = false,
                                    Message = "File Excel không có dữ liệu!",
                                    Data = null
                                };
                            }

                            var rowCount = dimension.End.Row;
                            Console.WriteLine($"Excel có {rowCount} dòng");

                            if (rowCount < 2)
                            {
                                return new BaseResponse<object>
                                {
                                    Code = 400,
                                    Success = false,
                                    Message = "File Excel không có dữ liệu!",
                                    Data = null
                                };
                            }

                            result.TotalRows = rowCount - 1;

                                            // Đọc tất cả dữ liệu từ Excel trước
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var studentId = worksheet.Cells[row, 1].Text?.Trim();
                        var fullName = worksheet.Cells[row, 2].Text?.Trim();
                        var email = worksheet.Cells[row, 3].Text?.Trim();

                        // Chỉ thêm dòng có dữ liệu
                        if (!string.IsNullOrEmpty(studentId) || !string.IsNullOrEmpty(fullName) || !string.IsNullOrEmpty(email))
                        {
                            excelData.Add((studentId, fullName, email));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Bỏ qua dòng lỗi và tiếp tục
                        continue;
                    }
                }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new BaseResponse<object>
                        {
                            Code = 500,
                            Success = false,
                            Message = $"Lỗi đọc file Excel: {ex.Message}",
                            Data = null
                        };
                    }
                }

                // Xử lý dữ liệu từng dòng với timeout
                var timeout = TimeSpan.FromMinutes(5); // Timeout 5 phút
                var startTime = DateTime.UtcNow;

                Console.WriteLine($"Bắt đầu xử lý {excelData.Count} dòng dữ liệu");



                for (int i = 0; i < excelData.Count; i++)
                {
                    // Kiểm tra timeout
                    if (DateTime.UtcNow - startTime > timeout)
                    {
                        Console.WriteLine($"Import timeout sau {i} dòng");
                        return new BaseResponse<object>
                        {
                            Code = 408,
                            Success = false,
                            Message = "Import bị timeout. Vui lòng thử lại với file nhỏ hơn!",
                            Data = new { ProcessedRows = i, TotalRows = excelData.Count }
                        };
                    }

                    var row = i + 2; // Vì bắt đầu từ dòng 2
                    var (studentId, fullName, email) = excelData[i];

                    try
                    {
                        // Validate dữ liệu
                        if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
                        {
                            errorItems.Add(new ImportErrorItem
                            {
                                RowNumber = row,
                                UserNumberCode = studentId ?? "",
                                FullName = fullName ?? "",
                                ErrorMessage = "Thiếu thông tin bắt buộc (MSSV, Họ tên, Email)"
                            });
                            continue;
                        }

                        // Validate email format
                        if (!IsValidEmail(email))
                        {
                            errorItems.Add(new ImportErrorItem
                            {
                                RowNumber = row,
                                UserNumberCode = studentId,
                                FullName = fullName,
                                ErrorMessage = "Email không đúng định dạng"
                            });
                            continue;
                        }

                        // Kiểm tra user đã tồn tại chưa
                        var existingUser = await _userRepository.GetUserByEmail(email);
                        if (existingUser == null)
                        {
                            // Tạo user mới
                            var newUser = new User
                            {
                                UserId = Guid.NewGuid(),
                                UserRollNumber = studentId,
                                UserEmail = email,
                                UserFullName = fullName,
                                UserNumberCode = studentId ?? string.Empty,
                                UserRoleName = "Student",
                                Status = "IsActive"
                            };

                            await _userRepository.CreateUser(newUser);
                            existingUser = newUser;
                        }

                        // Kiểm tra user đã trong class chưa
                        var isInClass = await _classUserRepository.IsUserInClassAsync(existingUser.UserId, classId);
                        if (isInClass)
                        {
                            errorItems.Add(new ImportErrorItem
                            {
                                RowNumber = row,
                                UserNumberCode = studentId,
                                FullName = fullName,
                                ErrorMessage = "Sinh viên đã có trong lớp học"
                            });
                            continue;
                        }



                        // Thêm user vào class
                        var classUser = new ClassUser
                        {
                            ClassId = classId,
                            UserId = existingUser.UserId,
                            ClassUserStatus = "Active"
                        };

                        await _classUserRepository.CreateAsync(classUser);

                        successItems.Add(new ImportSuccessItem
                        {
                            RowNumber = row,
                            UserNumberCode = studentId,
                            FullName = fullName,
                            Message = "Thêm thành công"
                        });
                    }
                    catch (Exception ex)
                    {
                        errorItems.Add(new ImportErrorItem
                        {
                            RowNumber = row,
                            UserNumberCode = studentId ?? string.Empty,
                            FullName = fullName ?? string.Empty,
                            ErrorMessage = $"Lỗi xử lý: {ex.Message}"
                        });
                    }
                }

                result.SuccessCount = successItems.Count;
                result.FailedCount = errorItems.Count;
                result.SuccessItems = successItems;
                result.ErrorItems = errorItems;

                Console.WriteLine($"Import hoàn tất: Thành công {result.SuccessCount}, Lỗi {result.FailedCount}");

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = $"Import hoàn tất. Thành công: {result.SuccessCount}, Lỗi: {result.FailedCount}",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi import: {ex.Message}",
                    Data = null
                };
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
} 