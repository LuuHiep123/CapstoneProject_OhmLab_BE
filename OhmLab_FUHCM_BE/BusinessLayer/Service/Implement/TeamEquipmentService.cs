using AutoMapper;
using BusinessLayer.RequestModel.TeamEquipment;
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
    public class TeamEquipmentService : ITeamEquipmentService
    {
        private readonly ITeamEquipmentRepository _teamEquipmentRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;

        public TeamEquipmentService(ITeamEquipmentRepository teamEquipmentRepository, IEquipmentRepository equipmentRepository, IMapper mapper)
        {
            _teamEquipmentRepository = teamEquipmentRepository;
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TeamEquipmentAllResponseModel>> CreateTeamEquipment(CreateTeamEquipmentRequestModel model)
        {
            try
            {
                var equipment = await _equipmentRepository.GetEquipmentById(model.EquipmentId);
                if (equipment.EquipmentStatus.Equals("InUse"))
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Equipment in use!",
                        Data = null
                    };
                }
                else if (equipment.EquipmentStatus.Equals("Maintenance"))
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Equipment in Maintenance!",
                        Data = null
                    };
                } else if(equipment.EquipmentStatus.Equals("Damaged"))
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Equipment in Damaged!",
                        Data = null
                    };
                } else
                {
                    var teamEquipment = _mapper.Map<TeamEquipment>(model);      
                    teamEquipment.TeamEquipmentDateBorrow = DateTime.Now;
                    teamEquipment.TeamEquipmentStatus = "AreBorrowing";
                    await _teamEquipmentRepository.CreateTeamEquipment(teamEquipment);

                    equipment.EquipmentStatus = "InUse";
                    await _equipmentRepository.UpdateEquipment(equipment);
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create TeamEquipment success!",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamEquipmentAllResponseModel>> FillBorrowDateForTeamEquipment(int teamEquipmentId)
        {
            try
            {
                var teamEquipment = await _teamEquipmentRepository.GetTeamEquipmentById(teamEquipmentId);
                if(teamEquipment == null)
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found",
                        Data = null
                    };
                }
                teamEquipment.TeamEquipmentDateGiveBack = DateTime.Now;
                teamEquipment.TeamEquipmentStatus = "Paid";
                await _teamEquipmentRepository.UpdateTeamEquipment(teamEquipment);

                var equipment = await _equipmentRepository.GetEquipmentById(teamEquipment.EquipmentId);
                if (!equipment.EquipmentStatus.Equals("InUse"))
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "The device is not in use!",
                        Data = null
                    };
                }
                equipment.EquipmentStatus = "Available";
                await _equipmentRepository.UpdateEquipment(equipment);
                return new BaseResponse<TeamEquipmentAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Give back Equipment success!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DynamicResponse<TeamEquipmentAllResponseModel>> GetListTeamEquipment(GetAllTeamEquipmentRequestModel model)
        {
            try
            {
                var listTeamEquipment = await _teamEquipmentRepository.GetAllTeamEquipment();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<TeamEquipment> listTeamEquipmetByName = listTeamEquipment.Where(TE => TE.TeamEquipmentName.Contains(model.keyWord)).ToList();
                    listTeamEquipment = listTeamEquipmetByName
                               .GroupBy(TE => TE.TeamEquipmentId)
                               .Select(g => g.First())
                               .ToList();
                }
                var result = _mapper.Map<List<TeamEquipmentAllResponseModel>>(listTeamEquipment);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.TeamEquipmentId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<TeamEquipmentAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<TeamEquipmentAllResponseModel>()
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

        public async Task<BaseResponse<List<TeamEquipmentAllResponseModel>>> GetListTeamEquipmentByTeamId(int teamId)
        {
            try
            {
                var listTeamEquipment = await _teamEquipmentRepository.GetAllTeamEquipmentByTeamId(teamId);
                var result = _mapper.Map<List<TeamEquipmentAllResponseModel>>(listTeamEquipment);

                return new BaseResponse<List<TeamEquipmentAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list TeamEquipment by TeamId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamEquipmentAllResponseModel>> GetTeamEquipmentById(int id)
        {
            try
            {
                var teamEquipment = await _teamEquipmentRepository.GetTeamEquipmentById(id);
                var result = _mapper.Map<TeamEquipmentAllResponseModel>(teamEquipment);

                return new BaseResponse<TeamEquipmentAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list TeamEquipment by EquipmentId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<List<TeamEquipmentAllResponseModel>>> GetListTeamEquipmentByEquipmentId(string equipmentId)
        {
            try
            {
                var listTeamEquipment = await _teamEquipmentRepository.GetAllTeamEquipmentByEquipmentId(equipmentId);
                var result = _mapper.Map<List<TeamEquipmentAllResponseModel>>(listTeamEquipment);

                return new BaseResponse<List<TeamEquipmentAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "list TeamEquipment by EquipmentId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamEquipmentAllResponseModel>> UpdateTeamEquipment(int teamEquipmentId, UpdateTeamEquipmentRequestModel model)
        {
            try
            {
                var teamEquipment = await _teamEquipmentRepository.GetTeamEquipmentById(teamEquipmentId);
                if (teamEquipment == null)
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not fount TeamEquipment!",
                        Data = null
                    };
                }
                var result = _mapper.Map(model, teamEquipment);
                await _teamEquipmentRepository.UpdateTeamEquipment(result);

                return new BaseResponse<TeamEquipmentAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Update TeamEquipment success!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<TeamEquipmentAllResponseModel>> DeleteTeamEquipment(int teamEquipmentId)
        {
            try
            {
                var teamEquipment = await _teamEquipmentRepository.GetTeamEquipmentById(teamEquipmentId);
                if (teamEquipment == null)
                {
                    return new BaseResponse<TeamEquipmentAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not fount TeamEquipment!",
                        Data = null
                    };
                }
                teamEquipment.TeamEquipmentStatus = "Delete";
                await _teamEquipmentRepository.UpdateTeamEquipment(teamEquipment);

                return new BaseResponse<TeamEquipmentAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete TeamEquipment success!",
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
