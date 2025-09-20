using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Grade;
using BusinessLayer.Service;
using DataLayer.Entities;
using DataLayer.Repository;

namespace BusinessLayer.Service.Implement
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ILabRepository _labRepository;
        private readonly IClassRepository _classRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClassUserRepository _classUserRepository;
        private readonly ITeamUserRepository _teamUserRepository;

        public GradeService(
            IGradeRepository gradeRepository,
            ILabRepository labRepository,
            IClassRepository classRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IClassUserRepository classUserRepository,
            ITeamUserRepository teamUserRepository)
        {
            _gradeRepository = gradeRepository;
            _labRepository = labRepository;
            _classRepository = classRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _classUserRepository = classUserRepository;
            _teamUserRepository = teamUserRepository;
        }

        public async Task<BaseResponse<bool>> GradeTeamLabAsync(GradeTeamLabRequestModel model, int labId, int teamId, Guid lecturerId)
        {
            try
            {
                // Validation
                if (model.Grade < 0 || model.Grade > 10)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Điểm phải từ 0-10!",
                        Data = false
                    };
                }

                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = false
                    };
                }

                // Kiểm tra class tồn tại
                var classEntity = await _classRepository.GetByIdAsync(model.ClassId);
                if (classEntity == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = false
                    };
                }

                // Kiểm tra lecturer có phụ trách lớp này không
                if (classEntity.LecturerId != lecturerId)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Bạn không phụ trách lớp này!",
                        Data = false
                    };
                }

                // Kiểm tra team tồn tại và thuộc lớp này
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null || team.ClassId != model.ClassId)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy team hoặc team không thuộc lớp này!",
                        Data = false
                    };
                }

                // Lấy danh sách thành viên trong team
                var teamMembers = await _teamRepository.GetTeamMembersAsync(teamId);
                
                // Xóa tất cả grade cũ của team này (nếu có)
                var existingGrades = (await _gradeRepository.GetByLabIdAsync(labId))
                    .Where(g => g.TeamId == teamId).ToList();
                
                foreach (var existingGrade in existingGrades)
                {
                    await _gradeRepository.DeleteAsync(existingGrade.GradeId);
                }

                // Tạo grade mới cho tất cả thành viên trong team
                foreach (var member in teamMembers)
                {
                    var newGrade = new Grade
                    {
                        TeamId = teamId,
                        LabId = labId,
                        UserId = member.UserId, // UserId là từng thành viên trong team
                        Grade1 = model.Grade, // Tất cả cùng điểm
                        GradeDescription = model.GradeDescription,
                        GradeStatus = model.GradeStatus
                    };

                    await _gradeRepository.CreateAsync(newGrade);
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Chấm điểm team thành công!",
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

        public async Task<BaseResponse<bool>> GradeTeamMemberAsync(GradeTeamMemberRequestModel model, int labId, int teamId, Guid studentId, Guid lecturerId)
        {
            try
            {
                // Validation
                if (model.IndividualGrade < 0 || model.IndividualGrade > 10)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Điểm cá nhân phải từ 0-10!",
                        Data = false
                    };
                }

                // Kiểm tra team tồn tại
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy team!",
                        Data = false
                    };
                }

                // Kiểm tra lecturer có phụ trách lớp của team này không
                var classEntity = await _classRepository.GetByIdAsync(team.ClassId);
                if (classEntity?.LecturerId != lecturerId)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Bạn không phụ trách lớp này!",
                        Data = false
                    };
                }

                // Kiểm tra student có thuộc team này không
                var teamMembers = await _teamRepository.GetTeamMembersAsync(teamId);
                if (!teamMembers.Any(tm => tm.UserId == studentId))
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Student không thuộc team này!",
                        Data = false
                    };
                }

                // Tạo hoặc cập nhật grade cho member
                var existingGrade = (await _gradeRepository.GetByLabIdAsync(labId))
                    .FirstOrDefault(g => g.TeamId == teamId && g.UserId == studentId);

                if (existingGrade != null)
                {
                    // Cập nhật grade hiện tại
                    existingGrade.Grade1 = model.IndividualGrade;
                    existingGrade.GradeDescription = model.IndividualComment;
                    existingGrade.GradeStatus = "Graded";
                    
                    await _gradeRepository.UpdateAsync(existingGrade);
                }
                else
                {
                    // Tạo grade mới
                    var newGrade = new Grade
                    {
                        TeamId = teamId,
                        LabId = labId,
                        UserId = studentId,
                        Grade1 = model.IndividualGrade,
                        GradeDescription = model.IndividualComment,
                        GradeStatus = "Graded"
                    };

                    await _gradeRepository.CreateAsync(newGrade);
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Chấm điểm member thành công!",
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

        public async Task<BaseResponse<List<PendingTeamGradeModel>>> GetPendingTeamsAsync(int labId, Guid lecturerId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<List<PendingTeamGradeModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Lấy các lớp mà lecturer phụ trách có lab này
                var lecturerClasses = await _classRepository.GetByLecturerIdAsync(lecturerId);
                var classesWithLab = lecturerClasses.Where(c => c.SubjectId == lab.SubjectId).ToList();

                var pendingTeams = new List<PendingTeamGradeModel>();

                foreach (var classEntity in classesWithLab)
                {
                    // Lấy các team trong lớp
                    var teamsInClass = await _teamRepository.GetByClassIdAsync(classEntity.ClassId);
                    
                    foreach (var team in teamsInClass)
                    {
                        // Kiểm tra team đã được chấm điểm chưa
                        var existingGrade = (await _gradeRepository.GetByLabIdAsync(labId))
                            .FirstOrDefault(g => g.TeamId == team.TeamId);

                        if (existingGrade == null || existingGrade.GradeStatus == "Pending")
                        {
                            var pendingTeam = new PendingTeamGradeModel
                            {
                                TeamId = team.TeamId,
                                TeamName = team.TeamName,
                                LabId = labId,
                                LabName = lab.LabName,
                                ClassId = classEntity.ClassId,
                                ClassName = classEntity.ClassName,
                                MemberCount = team.TeamUsers?.Count ?? 0,
                                Status = existingGrade?.GradeStatus ?? "Pending"
                            };

                            pendingTeams.Add(pendingTeam);
                        }
                    }
                }

                return new BaseResponse<List<PendingTeamGradeModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách team cần chấm điểm thành công!",
                    Data = pendingTeams
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<PendingTeamGradeModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<TeamGradeResponseModel>> GetTeamGradeAsync(int labId, int teamId, Guid userId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<TeamGradeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Kiểm tra team tồn tại
                var team = await _teamRepository.GetByIdAsync(teamId);
                if (team == null)
                {
                    return new BaseResponse<TeamGradeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy team!",
                        Data = null
                    };
                }

                // Kiểm tra user có quyền xem điểm của team này không
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<TeamGradeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy user!",
                        Data = null
                    };
                }

                // Kiểm tra user có thuộc team này không hoặc là lecturer của lớp
                var isTeamMember = team.TeamUsers?.Any(tm => tm.UserId == userId) ?? false;
                var classEntity = await _classRepository.GetByIdAsync(team.ClassId);
                var isLecturer = classEntity?.LecturerId == userId;

                if (!isTeamMember && !isLecturer)
                {
                    return new BaseResponse<TeamGradeResponseModel>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Bạn không có quyền xem điểm của team này!",
                        Data = null
                    };
                }

                // Lấy grade của team
                var teamGrades = (await _gradeRepository.GetByLabIdAsync(labId))
                    .Where(g => g.TeamId == teamId).ToList();

                // Debug: In ra số lượng grade tìm được
                Console.WriteLine($"Debug: Tìm thấy {teamGrades.Count} grade cho team {teamId}");

                // Tất cả grade đều là của thành viên trong team (cùng điểm)
                var memberGrades = teamGrades.Where(g => g.UserId != Guid.Empty).ToList();
                var teamGrade = memberGrades.FirstOrDefault(); // Lấy grade đầu tiên làm điểm chung

                // Debug: In ra thông tin grade
                if (teamGrade != null)
                {
                    Console.WriteLine($"Debug: Team grade: {teamGrade.Grade1}, UserId: {teamGrade.UserId}");
                }
                else
                {
                    Console.WriteLine("Debug: Không tìm thấy team grade!");
                }

                var response = new TeamGradeResponseModel
                {
                    TeamId = teamId,
                    TeamName = team.TeamName,
                    LabId = labId,
                    LabName = lab.LabName,
                    TeamGrade = teamGrade?.Grade1 ?? 0,
                    TeamComment = teamGrade?.GradeDescription,
                    GradeStatus = teamGrade?.GradeStatus ?? "Pending",
                    GradedDate = teamGrade != null ? DateTime.Now : null
                };

                // Lấy thông tin các member và điểm của họ
                if (team.TeamUsers != null)
                {
                    foreach (var member in team.TeamUsers)
                    {
                        var memberGrade = memberGrades.FirstOrDefault(g => g.UserId == member.UserId);
                        var memberUser = await _userRepository.GetUserById(member.UserId);

                        response.Members.Add(new TeamMemberGradeModel
                        {
                            StudentId = member.UserId,
                            StudentName = memberUser?.UserFullName ?? "Unknown",
                            IndividualGrade = memberGrade?.Grade1 ?? 0,
                            IndividualComment = memberGrade?.GradeDescription
                        });
                    }
                }

                return new BaseResponse<TeamGradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy điểm team thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamGradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<TeamMemberGradeModel>> GetMyIndividualGradeAsync(int labId, Guid studentId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<TeamMemberGradeModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Lấy team của student
                var studentTeams = await _teamRepository.GetTeamsByUserIdAsync(studentId);
                
                // Tìm team thuộc class có cùng subject với lab
                Team teamWithLab = null;
                foreach (var team in studentTeams)
                {
                    if (team.ClassId > 0)
                    {
                        var teamClass = await _classRepository.GetByIdAsync(team.ClassId);
                        if (teamClass != null && teamClass.SubjectId == lab.SubjectId)
                        {
                            teamWithLab = team;
                            break;
                        }
                    }
                }

                if (teamWithLab == null)
                {
                    return new BaseResponse<TeamMemberGradeModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Bạn không có team nào cho lab này!",
                        Data = null
                    };
                }

                // Lấy điểm cá nhân
                var individualGrade = (await _gradeRepository.GetByLabIdAsync(labId))
                    .FirstOrDefault(g => g.TeamId == teamWithLab.TeamId && g.UserId == studentId);

                var response = new TeamMemberGradeModel
                {
                    StudentId = studentId,
                    StudentName = "You", // Student xem điểm của chính mình
                    IndividualGrade = individualGrade?.Grade1 ?? 0,
                    IndividualComment = individualGrade?.GradeDescription
                };

                return new BaseResponse<TeamMemberGradeModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy điểm cá nhân thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TeamMemberGradeModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetTeamGradeStatisticsAsync(int labId, Guid lecturerId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<object>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Lấy các lớp mà lecturer phụ trách có lab này
                var lecturerClasses = await _classRepository.GetByLecturerIdAsync(lecturerId);
                var classesWithLab = lecturerClasses.Where(c => c.SubjectId == lab.SubjectId).ToList();

                var statistics = new
                {
                    LabId = labId,
                    LabName = lab.LabName,
                    TotalTeams = 0,
                    GradedTeams = 0,
                    PendingTeams = 0,
                    AverageGrade = 0.0,
                    GradeDistribution = new Dictionary<string, int>()
                };

                var allGrades = await _gradeRepository.GetByLabIdAsync(labId);
                // Tất cả grade đều là của thành viên trong team (cùng điểm)
                var teamIds = allGrades.Select(g => g.TeamId).Distinct().ToList();
                var teamGrades = new List<Grade>();
                
                foreach (var teamId in teamIds)
                {
                    var memberGrades = allGrades.Where(g => g.TeamId == teamId && g.UserId != Guid.Empty).ToList();
                    if (memberGrades.Any())
                    {
                        teamGrades.Add(memberGrades.First()); // Lấy grade đầu tiên làm điểm chung
                    }
                }

                statistics = new
                {
                    LabId = labId,
                    LabName = lab.LabName,
                    TotalTeams = teamGrades.Count,
                    GradedTeams = teamGrades.Count(g => g.GradeStatus == "Graded"),
                    PendingTeams = teamGrades.Count(g => g.GradeStatus == "Pending"),
                    AverageGrade = teamGrades.Any() ? teamGrades.Average(g => g.Grade1) : 0.0,
                    GradeDistribution = teamGrades
                        .GroupBy(g => g.Grade1)
                        .ToDictionary(g => $"Grade {g.Key}", g => g.Count())
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thống kê điểm thành công!",
                    Data = statistics
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamGradeResponseModel>>> GetGradeById(int labId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<List<TeamGradeResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Lấy tất cả grade của lab
                var allGrades = await _gradeRepository.GetByLabIdAsync(labId);
                var teamIds = allGrades.Select(g => g.TeamId).Distinct().ToList();

                var allTeamGrades = new List<TeamGradeResponseModel>();

                foreach (var teamId in teamIds)
                {
                    var team = await _teamRepository.GetByIdAsync(teamId);
                    if (team != null)
                    {
                        // Tất cả grade đều là của thành viên trong team (cùng điểm)
                        var memberGrades = allGrades.Where(g => g.TeamId == teamId && g.UserId != Guid.Empty).ToList();
                        var teamGrade = memberGrades.FirstOrDefault(); // Lấy grade đầu tiên làm điểm chung

                        var response = new TeamGradeResponseModel
                        {
                            TeamId = teamId,
                            TeamName = team.TeamName,
                            LabId = labId,
                            LabName = lab.LabName,
                            TeamGrade = teamGrade?.Grade1 ?? 0,
                            TeamComment = teamGrade?.GradeDescription,
                            GradeStatus = teamGrade?.GradeStatus ?? "Pending",
                            GradedDate = teamGrade != null ? DateTime.Now : null
                        };

                        // Lấy thông tin các member
                        if (team.TeamUsers != null)
                        {
                            foreach (var member in team.TeamUsers)
                            {
                                var memberGrade = memberGrades.FirstOrDefault(g => g.UserId == member.UserId);
                                var memberUser = await _userRepository.GetUserById(member.UserId);

                                response.Members.Add(new TeamMemberGradeModel
                                {
                                    StudentId = member.UserId,
                                    StudentName = memberUser?.UserFullName ?? "Unknown",
                                    IndividualGrade = memberGrade?.Grade1 ?? 0,
                                    IndividualComment = memberGrade?.GradeDescription
                                });
                            }
                        }

                        allTeamGrades.Add(response);
                    }
                }

                return new BaseResponse<List<TeamGradeResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả điểm lab thành công!",
                    Data = allTeamGrades
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TeamGradeResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<TeamGradeResponseModel>>> GetAllGrade()
        {
            try
            {
                // Lấy tất cả grade trong hệ thống
                var allGrades = await _gradeRepository.GetAllAsync();
                
                if (!allGrades.Any())
                {
                    return new BaseResponse<List<TeamGradeResponseModel>>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không có điểm nào trong hệ thống!",
                        Data = new List<TeamGradeResponseModel>()
                    };
                }

                // Nhóm grade theo Lab và Team
                var labTeamGroups = allGrades
                    .GroupBy(g => new { g.LabId, g.TeamId })
                    .ToList();

                var allTeamGrades = new List<TeamGradeResponseModel>();

                foreach (var group in labTeamGroups)
                {
                    var labId = group.Key.LabId;
                    var teamId = group.Key.TeamId;

                    // Lấy thông tin lab
                    var lab = await _labRepository.GetLabById(labId);
                    if (lab == null) continue;

                    // Lấy thông tin team
                    var team = await _teamRepository.GetByIdAsync(teamId);
                    if (team == null) continue;

                    // Tất cả grade đều là của thành viên trong team (cùng điểm)
                    var memberGrades = group.Where(g => g.UserId != Guid.Empty).ToList();
                    var teamGrade = memberGrades.FirstOrDefault(); // Lấy grade đầu tiên làm điểm chung

                    var response = new TeamGradeResponseModel
                    {
                        TeamId = teamId,
                        TeamName = team.TeamName,
                        LabId = labId,
                        LabName = lab.LabName,
                        TeamGrade = teamGrade?.Grade1 ?? 0,
                        TeamComment = teamGrade?.GradeDescription,
                        GradeStatus = teamGrade?.GradeStatus ?? "Pending",
                        GradedDate = teamGrade != null ? DateTime.Now : null
                    };

                    // Lấy thông tin các member
                    if (team.TeamUsers != null)
                    {
                        foreach (var member in team.TeamUsers)
                        {
                            var memberGrade = memberGrades.FirstOrDefault(g => g.UserId == member.UserId);
                            var memberUser = await _userRepository.GetUserById(member.UserId);

                            response.Members.Add(new TeamMemberGradeModel
                            {
                                StudentId = member.UserId,
                                StudentName = memberUser?.UserFullName ?? "Unknown",
                                IndividualGrade = memberGrade?.Grade1 ?? 0,
                                IndividualComment = memberGrade?.GradeDescription
                            });
                        }
                    }

                    allTeamGrades.Add(response);
                }

                return new BaseResponse<List<TeamGradeResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả điểm trong hệ thống thành công!",
                    Data = allTeamGrades
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TeamGradeResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<StudentLabGradesResponseModel>> GetStudentLabGradesAsync(Guid studentId, Guid requestUserId, string userRole)
        {
            try
            {
                // Validation: Check if student exists
                var student = await _userRepository.GetUserById(studentId);
                if (student == null)
                {
                    return new BaseResponse<StudentLabGradesResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Sinh viên không tồn tại!"
                    };
                }

                // Access control based on role
                if (userRole == "Student" && studentId != requestUserId)
                {
                    return new BaseResponse<StudentLabGradesResponseModel>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Sinh viên chỉ xem được điểm của chính mình!"
                    };
                }

                // Get student's classes
                var studentClasses = await _classUserRepository.GetByUserIdAsync(studentId);
                var classIds = studentClasses.Select(cu => cu.ClassId).ToList();

                // For Lecturer: Check if they teach any of the student's classes
                if (userRole == "Lecturer")
                {
                    var lecturerClasses = await _classRepository.GetByLecturerIdAsync(requestUserId);
                    var lecturerClassIds = lecturerClasses.Select(c => c.ClassId).ToList();
                    
                    if (!classIds.Any(id => lecturerClassIds.Contains(id)))
                    {
                        return new BaseResponse<StudentLabGradesResponseModel>
                        {
                            Code = 403,
                            Success = false,
                            Message = "Bạn không phụ trách lớp nào của sinh viên này!"
                        };
                    }
                }

                // Get all grades for the student
                var grades = await _gradeRepository.GetGradesByStudentId(studentId);
                
                // Get detailed information for each grade
                var labGrades = new List<StudentLabGradeModel>();
                
                foreach (var grade in grades)
                {
                    var lab = await _labRepository.GetLabById(grade.LabId);
                    var team = await _teamRepository.GetByIdAsync(grade.TeamId);
                    var classEntity = await _classRepository.GetByIdAsync(team.ClassId);
                    
                    // Check if user has access to this grade
                    if (userRole == "Lecturer" && classEntity.LecturerId != requestUserId)
                    {
                        continue; // Skip grades from classes not taught by this lecturer
                    }

                    // Determine if this is a team grade or individual grade
                    var teamMembers = await _teamUserRepository.GetByTeamIdAsync(grade.TeamId);
                    var isTeamGrade = teamMembers.Count > 1;
                    
                    // Check if there are individual grades for team members
                    var hasIndividualGrade = false;
                    if (isTeamGrade)
                    {
                        var teamGrades = await _gradeRepository.GetGradesByLabAndTeam(grade.LabId, grade.TeamId);
                        hasIndividualGrade = teamGrades.Any(g => g.UserId != studentId);
                    }

                    labGrades.Add(new StudentLabGradeModel
                    {
                        GradeId = grade.GradeId,
                        LabId = grade.LabId,
                        LabName = lab.LabName,
                        LabTarget = lab.LabTarget,
                        SubjectName = classEntity.Subject.SubjectName,
                        ClassName = classEntity.ClassName,
                        TeamId = grade.TeamId,
                        TeamName = team.TeamName,
                        GradeScore = grade.Grade1,
                        GradeDescription = grade.GradeDescription,
                        GradeStatus = grade.GradeStatus,
                        
                        IsTeamGrade = isTeamGrade,
                        HasIndividualGrade = hasIndividualGrade,
                        LecturerName = classEntity.Lecturer.UserFullName
                    });
                }

                var response = new StudentLabGradesResponseModel
                {
                    StudentId = studentId,
                    StudentName = student.UserFullName,
                    StudentEmail = student.UserEmail,
                    LabGrades = labGrades.OrderByDescending(g => g).ToList()
                };

                return new BaseResponse<StudentLabGradesResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentLabGradesResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ClassGradesResponseModel>> GetClassGradesAsync(int classId, Guid requestUserId, string userRole)
        {
            try
            {
                // Kiểm tra class tồn tại
                var classEntity = await _classRepository.GetByIdAsync(classId);
                if (classEntity == null)
                {
                    return new BaseResponse<ClassGradesResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                // Validate quyền truy cập
                if (userRole == "Lecturer")
                {
                    // Chỉ lecturer phụ trách lớp mới được xem
                    if (classEntity.LecturerId != requestUserId)
                    {
                        return new BaseResponse<ClassGradesResponseModel>
                        {
                            Code = 403,
                            Success = false,
                            Message = "Bạn không phụ trách lớp này!",
                            Data = null
                        };
                    }
                }
                else if (userRole == "Student")
                {
                    // Chỉ student trong lớp mới được xem
                    var isStudentInClass = await _classUserRepository.IsUserInClassAsync(requestUserId, classId);
                    if (!isStudentInClass)
                    {
                        return new BaseResponse<ClassGradesResponseModel>
                        {
                            Code = 403,
                            Success = false,
                            Message = "Bạn không thuộc lớp này!",
                            Data = null
                        };
                    }
                }
                else
                {
                    return new BaseResponse<ClassGradesResponseModel>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Bạn không có quyền truy cập!",
                        Data = null
                    };
                }

                // Lấy tất cả labs thuộc subject của lớp
                var labs = await _labRepository.GetLabsBySubjectId(classEntity.SubjectId);
                var labInfoList = labs.Select(lab => new LabInfoModel
                {
                    LabId = lab.LabId,
                    LabName = lab.LabName
                }).OrderBy(l => l.LabId).ToList();

                // Lấy tất cả sinh viên trong lớp
                var classUsers = await _classUserRepository.GetByClassIdAsync(classId);
                var studentIds = classUsers.Select(cu => cu.UserId).ToList();
                
                // Lấy thông tin chi tiết của sinh viên
                var students = new List<StudentGradeModel>();
                foreach (var studentId in studentIds)
                {
                    var user = await _userRepository.GetUserById(studentId);
                    if (user != null)
                    {
                        // Lấy team của sinh viên trong lớp này
                        var teamUser = await _teamUserRepository.GetByUserIdAndClassIdAsync(studentId, classId);
                        int? teamId = teamUser?.TeamId;
                        string teamName = teamUser?.Team?.TeamName ?? "Chưa có team";

                        // Lấy điểm của sinh viên cho tất cả các labs
                        var studentGrades = new List<StudentLabGradeDetailModel>();
                        foreach (var lab in labs)
                        {
                            var grade = (await _gradeRepository.GetByUserIdAsync(studentId))
                                .FirstOrDefault(g => g.LabId == lab.LabId);

                            studentGrades.Add(new StudentLabGradeDetailModel
                            {
                                LabId = lab.LabId,
                                Grade = grade?.Grade1,
                                GradeStatus = grade?.GradeStatus ?? "Chưa chấm điểm",
                                GradeDescription = grade?.GradeDescription,
                                IsTeamGrade = grade != null,
                                HasIndividualGrade = false // Có thể thêm logic sau nếu cần
                            });
                        }

                        students.Add(new StudentGradeModel
                        {
                            StudentId = studentId,
                            StudentName = user.UserFullName,
                            StudentEmail = user.UserEmail,
                            TeamId = teamId,
                            TeamName = teamName,
                            Grades = studentGrades.OrderBy(g => g.LabId).ToList()
                        });
                    }
                }

                var response = new ClassGradesResponseModel
                {
                    ClassId = classId,
                    ClassName = classEntity.ClassName,
                    Labs = labInfoList,
                    Students = students.OrderBy(s => s.StudentName).ToList()
                };

                return new BaseResponse<ClassGradesResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassGradesResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
