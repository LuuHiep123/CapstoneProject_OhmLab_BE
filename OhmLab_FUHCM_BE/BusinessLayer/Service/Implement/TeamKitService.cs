using AutoMapper;
using BusinessLayer.RequestModel.TeamKit;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.TeamEquipment;
using BusinessLayer.ResponseModel.TeamKit;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
    public class TeamKitService : ITeamKitService
    {
        private readonly ITeamKitRepository _teamKitRepository;
        private readonly IKitRepository _kitRepository;
        private readonly IMapper _mapper;

        public TeamKitService(ITeamKitRepository teamKitRepository, IKitRepository kitRepository, IMapper mapper)
        {
            _teamKitRepository = teamKitRepository;
            _kitRepository = kitRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamKitAllResponseModel>> CreateTeamKit(CreateTeamKitRequestModel model)
        {
            try
            {
                var kit = await _kitRepository.GetKitById(model.KitId);
                if (kit.KitStatus.Equals("InUse"))
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Kit in use!",
                        Data = null
                    };
                }
                else if (kit.KitStatus.Equals("Maintenance"))
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Kit in Maintenance!",
                        Data = null
                    };
                }
                else if (kit.KitStatus.Equals("Damaged"))
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Kit in Damaged!",
                        Data = null
                    };
                }
                else
                {
                    var teamKit = _mapper.Map<TeamKit>(model);
                    teamKit.TeamKitDateBorrow = DateTime.Now;
                    teamKit.TeamKitStatus = "AreBorrowing";
                    await _teamKitRepository.CreateTeamKit(teamKit);

                    kit.KitStatus = "InUse";
                    await _kitRepository.UpdateKit(kit);
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create TeamKit success!",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamKitAllResponseModel>> DeleteTeamKit(int teamKitId)
        {
            try
            {
                var teamKit = await _teamKitRepository.GetTeamKitById(teamKitId);
                if (teamKit == null)
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not fount TeamKit!",
                        Data = null
                    };
                }
                teamKit.TeamKitStatus = "Delete";
                await _teamKitRepository.UpdateTeamKit(teamKit);

                return new BaseResponse<TeamKitAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete TeamKit success!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamKitAllResponseModel>> FillBorrowDateForTeamKit(int teamKitId)
        {
            try
            {
                var teamKit = await _teamKitRepository.GetTeamKitById(teamKitId);
                if (teamKit == null)
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found",
                        Data = null
                    };
                }
                teamKit.TeamKitDateGiveBack = DateTime.Now;
                teamKit.TeamKitStatus = "Paid";
                await _teamKitRepository.UpdateTeamKit(teamKit);

                var kit = await _kitRepository.GetKitById(teamKit.KitId);
                if (!kit.KitStatus.Equals("InUse"))
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "The kit is not in use!",
                        Data = null
                    };
                }
                kit.KitStatus = "Valid";
                await _kitRepository.UpdateKit(kit);
                return new BaseResponse<TeamKitAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Give back kit success!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DynamicResponse<TeamKitAllResponseModel>> GetListTeamKit(GetAllTeamKitRequestModel model)
        {
            try
            {
                var listTeamKit = await _teamKitRepository.GetAllTeamKit();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<TeamKit> listTeamKitByName = listTeamKit.Where(TK => TK.TeamKitName.Contains(model.keyWord)).ToList();
                    listTeamKit = listTeamKitByName
                               .GroupBy(TK => TK.TeamKitId)
                               .Select(g => g.First())
                               .ToList();
                }
                var result = _mapper.Map<List<TeamKitAllResponseModel>>(listTeamKit);


                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.TeamKitId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<TeamKitAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<TeamKitAllResponseModel>()
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
                            role = null,
                            status = null,
                        },
                        PageData = pagedUsers.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<List<TeamKitAllResponseModel>>> GetListTeamKitByKitId(string kitId)
        {
            try
            {
                var listTeamKit = await _teamKitRepository.GetAllTeamKitByKitId(kitId);
                var result = _mapper.Map<List<TeamKitAllResponseModel>>(listTeamKit);

                return new BaseResponse<List<TeamKitAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list TeamKit by KitId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<List<TeamKitAllResponseModel>>> GetListTeamKitTeamId(int teamId)
        {
            try
            {
                var listTeamKit = await _teamKitRepository.GetAllTeamKitByTeamId(teamId);
                var result = _mapper.Map<List<TeamKitAllResponseModel>>(listTeamKit);

                return new BaseResponse<List<TeamKitAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list listTeamKit by TeamId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamKitAllResponseModel>> GetTeamKitById(int id)
        {
            try
            {
                var teamKit = await _teamKitRepository.GetTeamKitById(id);
                var result = _mapper.Map<TeamKitAllResponseModel>(teamKit);

                return new BaseResponse<TeamKitAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list TeamKit by EquipmentId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamKitAllResponseModel>> UpdateTeamKit(int teamKitId, UpdateTeamKitRequestModel model)
        {
            try
            {
                var teamKit = await _teamKitRepository.GetTeamKitById(teamKitId);
                if(teamKit == null)
                {
                    return new BaseResponse<TeamKitAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not fount TeamKit!",
                        Data = null
                    };
                }
                var result = _mapper.Map(model, teamKit);
                await _teamKitRepository.UpdateTeamKit(result);

                return new BaseResponse<TeamKitAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Update TeamKit success!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
