using AutoMapper;
using BusinessLayer.RequestModel.Team;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Team;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IClassRepository classRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamResponseModel>> CreateTeamAsync(CreateTeamRequestModel model)
        {
            try
            {
                // Kiểm tra class có tồn tại không
                var classEntity = await _classRepository.GetByIdAsync(model.ClassId);
                if (classEntity == null)
                {
                    return new BaseResponse<TeamResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                var team = new Team
                {
                    ClassId = model.ClassId,
                    TeamName = model.TeamName,
                    TeamDescription = model.TeamDescription,
                    
                };

                var result = await _teamRepository.CreateAsync(team);
                var response = _mapper.Map<TeamResponseModel>(result);
                response.ClassName = classEntity.ClassName;

                return new BaseResponse<TeamResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<TeamResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<TeamResponseModel>> GetTeamByIdAsync(int id)
        {
            try
            {
                var team = await _teamRepository.GetByIdAsync(id);
                if (team == null)
                {
                    return new BaseResponse<TeamResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy nhóm!",
                        Data = null
                    };
                }

                var response = _mapper.Map<TeamResponseModel>(team);
                response.ClassName = team.Class?.ClassName;

                return new BaseResponse<TeamResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<TeamResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamResponseModel>>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _teamRepository.GetAllAsync();
                var response = teams.Select(t => new TeamResponseModel
                {
                    TeamId = t.TeamId,
                    ClassId = t.ClassId,
                    TeamName = t.TeamName,
                    TeamDescription = t.TeamDescription,
                    ClassName = t.Class?.ClassName,
                    TeamUsers = t.TeamUsers?.Select(tu => new TeamUserResponseModel
                    {
                        TeamUserId = tu.TeamUserId,
                        UserId = tu.UserId,
                        UserName = tu.User?.UserFullName,
                        UserEmail = tu.User?.UserEmail,
                        UserNumberCode = tu.User?.UserNumberCode,
                        TeamUserStatus = tu.TeamUserStatus
                    }).ToList()
                }).ToList();

                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamResponseModel>>> GetTeamsByClassIdAsync(int classId)
        {
            try
            {
                var teams = await _teamRepository.GetByClassIdAsync(classId);
                var response = teams.Select(t => new TeamResponseModel
                {
                    TeamId = t.TeamId,
                    ClassId = t.ClassId,
                    TeamName = t.TeamName,
                    TeamDescription = t.TeamDescription,
                    ClassName = t.Class?.ClassName,
                    TeamUsers = t.TeamUsers?.Select(tu => new TeamUserResponseModel
                    {
                        TeamUserId = tu.TeamUserId,
                        UserId = tu.UserId,
                        UserName = tu.User?.UserFullName,
                        UserEmail = tu.User?.UserEmail,
                        UserNumberCode = tu.User?.UserNumberCode,
                        TeamUserStatus = tu.TeamUserStatus
                    }).ToList()
                }).ToList();

                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách nhóm theo lớp thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<TeamResponseModel>> UpdateTeamAsync(int id, UpdateTeamRequestModel model)
        {
            try
            {
                var team = await _teamRepository.GetByIdAsync(id);
                if (team == null)
                {
                    return new BaseResponse<TeamResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy nhóm!",
                        Data = null
                    };
                }

                team.TeamName = model.TeamName;
                team.TeamDescription = model.TeamDescription;
                

                var result = await _teamRepository.UpdateAsync(team);
                var response = _mapper.Map<TeamResponseModel>(result);
                response.ClassName = team.Class?.ClassName;

                return new BaseResponse<TeamResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật nhóm thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<TeamResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteTeamAsync(int id)
        {
            try
            {
                var result = await _teamRepository.DeleteAsync(id);
                if (!result)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy nhóm!",
                        Data = false
                    };
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Message = "Xóa nhóm thành công!",
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

        public async Task<BaseResponse<List<TeamResponseModel>>> GetTeamsByLecturerIdAsync(Guid lecturerId)
        {
            try
            {
                // Validation: Check lecturerId is not empty
                if (lecturerId == Guid.Empty)
                {
                    return new BaseResponse<List<TeamResponseModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "LecturerId không được để trống!",
                        Data = null
                    };
                }

                // Validation: Check if lecturer exists
                var lecturerExists = await _classRepository.CheckLecturerExistsAsync(lecturerId);
                if (!lecturerExists)
                {
                    return new BaseResponse<List<TeamResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy giảng viên!",
                        Data = null
                    };
                }

                var teams = await _teamRepository.GetByLecturerIdAsync(lecturerId);
                var teamResponseModels = _mapper.Map<List<TeamResponseModel>>(teams);
                
                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách nhóm theo giảng viên thành công!",
                    Data = teamResponseModels
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<TeamResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}