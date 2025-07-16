using DataLayer.Repository;
using BusinessLayer.ResponseModel.BaseResponse;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.User;
using DataLayer.Repository;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth;
using X.PagedList;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using DataLayer.Entities;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepositoty _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public UserService(IUserRepositoty userRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public string GenerateJwtToken(string username, string roleName, Guid userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<BaseResponse<LoginResponseModel>> LoginMail(LoginMailModel model)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(model.GoogleId);
                var email = payload.Email;
                var user = await _userRepository.GetUserByEmail(email);
                if (user != null)
                {
                    if (user.Status.ToLower().Equals("block"))
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "User has been block!.",
                            Data = null,
                        };
                    }
                    if (user.Status.ToLower().Equals("delete"))
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "User has been delete!.",
                            Data = null,
                        };
                    }

                    string token = GenerateJwtToken(user.UserFullName, user.UserRoleName, user.UserId);
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Login success!",
                        Data = new LoginResponseModel()
                        {
                            token = token,
                            user = _mapper.Map<UserResponseModel>(user)
                        },
                    };
                }
                else
                {
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = null,
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!.",
                    Data = null,
                };
            }

        }

        public async Task<DynamicResponse<UserResponseModel>> GetListUser(GetAllUserRequestModel model)
        {
            try
            {
                var listUser = await _userRepository.GetAllUser();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<User> listUserByName = listUser.Where(u => u.UserFullName.Contains(model.keyWord)).ToList();

                    List<User> listUserByEmail = listUser.Where(u => u.UserEmail.Contains(model.keyWord)).ToList();

                    listUser = listUserByName
                               .Concat(listUserByEmail)
                               .GroupBy(u => u.UserId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.role))
                {
                    if (!model.role.Equals("ALL") && !model.role.Equals("all") && !model.role.Equals("All"))
                    {
                        listUser = listUser.Where(u => u.UserRoleName.Equals(model.role)).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listUser = listUser.Where(u => u.Status.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<UserResponseModel>>(listUser);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.UserId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<UserResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<UserResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedUsers.PageNumber,
                            Size = pagedUsers.PageSize,
                            Sort = "Ascending",
                            Order = "Id",
                            TotalPage = pagedUsers.PageCount,
                            TotalItem = pagedUsers.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.keyWord,
                            role = model.role,
                            status = model.status,
                        },
                        PageData = pagedUsers.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    var result = _mapper.Map<UserResponseModel>(user);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> UpdateUser(Guid id, UpdateRequestModel model)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    var result = _mapper.Map(model, user);
                    await _userRepository.UpdateUser(result);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<UserResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    user.Status = "delete";
                    await _userRepository.UpdateUser(user);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete success!.",
                        Data = _mapper.Map<UserResponseModel>(user)
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> RegisterUser(RegisterRequestModel model)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(model.UserEmail);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                var User = _mapper.Map<User>(model);
                User.UserId = Guid.NewGuid();
                User.Status = "IsActive";
                User.UserRoleName = "Student";
                bool check = await _userRepository.CreateUser(User);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }

                var response = _mapper.Map<UserResponseModel>(User);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Add Student success!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> CreateAccountAdmin(string email, string name)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(email);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                User user = new User()
                {
                    UserId = Guid.NewGuid(),
                    UserRollNumber = "adm",
                    UserNumberCode = "adm",
                    UserEmail = email,
                    UserFullName = name,
                    Status = "Isactive",
                    UserRoleName = "Admin",
                };
                bool check = await _userRepository.CreateUser(user);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(user);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register admin success!.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
        public async Task<BaseResponse<UserResponseModel>> CreateAccountHeadOfDepartment(string email, string name)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(email);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                User user = new User()
                {
                    UserId = Guid.NewGuid(),
                    UserRollNumber = "stf",
                    UserNumberCode = "stf",
                    UserEmail = email,
                    UserFullName = name,
                    Status = "Isactive",
                    UserRoleName = "HeadOfDepartment",
                };
                bool check = await _userRepository.CreateUser(user);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(user);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register staff success!.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse> BlockUser(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!."
                    };
                }
                else
                {
                    user.Status = "Block";
                    await _userRepository.UpdateUser(user);
                    return new BaseResponse()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Block User sucessful!."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!."
                };
            }

        }

        public async Task<BaseResponse<LoginResponseModel>> LoginTest(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user != null)
                {
                    string token = GenerateJwtToken(user.UserFullName, user.UserRoleName, user.UserId);
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Login success!",
                        Data = new LoginResponseModel()
                        {
                            token = token,
                            user = _mapper.Map<UserResponseModel>(user)
                        },
                    };
                }
                else
                {
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!.",
                    Data = null,
                };
            }

        }
    }
}
