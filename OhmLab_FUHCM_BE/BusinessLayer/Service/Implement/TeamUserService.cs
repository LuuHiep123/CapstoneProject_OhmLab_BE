using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Team;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TeamUserService : ITeamUserService
    {
        private readonly ITeamUserRepository _teamUserRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClassUserRepository _classUserRepository;

        public TeamUserService(ITeamUserRepository teamUserRepository, ITeamRepository teamRepository, IUserRepository userRepository, IClassUserRepository classUserRepository)
        {
            _teamUserRepository = teamUserRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _classUserRepository = classUserRepository;
        }

        public async Task<BaseResponse<TeamUserResponseModel>> AddUserToTeamAsync(int teamId, Guid userId)
        {
            try
            {
                
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy nhóm!",
                        Data = null
                    };
                }
                var isUserInClass = await _classUserRepository.IsUserInClassAsync(userId, team.ClassId);
                if (!isUserInClass)
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Người dùng không thuộc lớp học của nhóm này!",
                        Data = null
                    };
                }

               
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy người dùng!",
                        Data = null
                    };
                }

                
                var userTeams = await _teamUserRepository.GetByUserIdAsync(userId);
                if (userTeams.Any(tu => tu.TeamId != teamId))
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Người dùng đã có trong một nhóm khác!",
                        Data = null
                    };
                }

                
                var isUserInTeam = await _teamUserRepository.IsUserInTeamAsync(userId, teamId);
                if (isUserInTeam)
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Người dùng đã có trong nhóm này!",
                        Data = null
                    };
                }

                var teamUser = new TeamUser
                {
                    TeamId = teamId,
                    UserId = userId,
                    TeamUserStatus = "Active"
                };

                var result = await _teamUserRepository.CreateAsync(teamUser);
                var response = new TeamUserResponseModel
                {
                    TeamUserId = result.TeamUserId,
                    UserId = result.UserId,
                    UserName = user.UserFullName,
                    UserEmail = user.UserEmail,
                    TeamUserStatus = result.TeamUserStatus
                };

                return new BaseResponse<TeamUserResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm người dùng vào nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<TeamUserResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<TeamUserResponseModel>> GetTeamUserByIdAsync(int id)
        {
            try
            {
                var teamUser = await _teamUserRepository.GetByIdAsync(id);
                if (teamUser == null)
                {
                    return new BaseResponse<TeamUserResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thành viên nhóm!",
                        Data = null
                    };
                }

                var response = new TeamUserResponseModel
                {
                    TeamUserId = teamUser.TeamUserId,
                    UserId = teamUser.UserId,
                    UserName = teamUser.User?.UserFullName,
                    UserEmail = teamUser.User?.UserEmail,
                    TeamUserStatus = teamUser.TeamUserStatus
                };

                return new BaseResponse<TeamUserResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin thành viên nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<TeamUserResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamUserResponseModel>>> GetTeamUsersByTeamIdAsync(int teamId)
        {
            try
            {
                var teamUsers = await _teamUserRepository.GetByTeamIdAsync(teamId);
                var response = teamUsers.Select(tu => new TeamUserResponseModel
                {
                    TeamUserId = tu.TeamUserId,
                    UserId = tu.UserId,
                    UserName = tu.User?.UserFullName,
                    UserEmail = tu.User?.UserEmail,
                    TeamUserStatus = tu.TeamUserStatus
                }).ToList();

                return new BaseResponse<List<TeamUserResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách thành viên nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<TeamUserResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamUserResponseModel>>> GetTeamUsersByUserIdAsync(Guid userId)
        {
            try
            {
                var teamUsers = await _teamUserRepository.GetByUserIdAsync(userId);
                var response = teamUsers.Select(tu => new TeamUserResponseModel
                {
                    TeamUserId = tu.TeamUserId,
                    UserId = tu.UserId,
                    UserName = tu.User?.UserFullName,
                    UserEmail = tu.User?.UserEmail,
                    TeamUserStatus = tu.TeamUserStatus
                }).ToList();

                return new BaseResponse<List<TeamUserResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách nhóm của người dùng thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<TeamUserResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> RemoveUserFromTeamAsync(int teamUserId)
        {
            try
            {
                var result = await _teamUserRepository.DeleteAsync(teamUserId);
                if (!result)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thành viên nhóm!",
                        Data = false
                    };
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa thành viên khỏi nhóm thành công!",
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

        public async Task<BaseResponse<bool>> IsUserInTeamAsync(Guid userId, int teamId)
        {
            try
            {
                var result = await _teamUserRepository.IsUserInTeamAsync(userId, teamId);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Kiểm tra thành công!",
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

        public async Task<BaseResponse<bool>> IsUserInClassAsync(Guid userId, int classId)
        {
            try
            {
                var result = await _classUserRepository.IsUserInClassAsync(userId, classId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Kiểm tra thành công!",
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