using AutoMapper;
using BusinessLayer.RequestModel.KitAccessory;
using BusinessLayer.ResponseModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.KitAccessory;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
    public class KitAccessoryService : IKitAccessoryService
    {
        private readonly IKitRepository _kitRepository;
        private readonly IKitAccessoryRepository _kitAccessoryRepository;
        private readonly IAccessoryRepository _accessoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public KitAccessoryService(IAccessoryRepository accessoryRepository, IKitAccessoryRepository kitAccessoryRepository, IKitRepository kitRepository,  IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _accessoryRepository = accessoryRepository;
            _kitRepository = kitRepository;
            _kitAccessoryRepository = kitAccessoryRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<BaseResponse<KitAccessoryResponseModel>> CreateKitAccessory(CreateKitAccessoryRequestModel model)
        {
            try
            {
                var kit = await _kitRepository.GetKitById(model.KitId);
                if (kit == null)
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found kit!.",
                        Data = null
                    };
                }

                var accessory = await _accessoryRepository.GetAccessoryById(model.AccessoryId);
                if (accessory == null)
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found accessory!.",
                        Data = null
                    };
                }

                if (model.AccessoryQuantity <= 0)
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 402,
                        Success = false,
                        Message = "Quantity must not be less than or equal to 0!.",
                        Data = null
                    };
                }
                var kitAccessory = _mapper.Map<KitAccessory>(model);
                kitAccessory.KitAccessoryStatus = "Valid";
                await _kitAccessoryRepository.CreateKitAccessory(kitAccessory);
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create KitAccessory Success!.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitAccessoryResponseModel>> DeleteKitAccessory(int id)
        {
            try
            {
                var kitAccessory = await _kitAccessoryRepository.GetKitAccessoryById(id);
                if (kitAccessory == null)
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found kitAccessory!"
                    };
                }
                kitAccessory.KitAccessoryStatus = "Invalid";
                await _kitAccessoryRepository.UpdateKitAccessory(kitAccessory);
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete kitAccessory success!",
                    Data = null
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<KitAccessoryResponseModel>> GetAllKitAccessory(GetALlKitAccessoryRequestModel model)
        {
            try
            {
                var listKitAccessory = await _kitAccessoryRepository.GetAllKitAccessory();
                if (!string.IsNullOrEmpty(model.status))
                {
                    listKitAccessory = listKitAccessory.Where(ak => ak.KitAccessoryStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<KitAccessoryResponseModel>>(listKitAccessory);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(ak => ak.KitAccessoryId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<KitAccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<KitAccessoryResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedUsers.PageNumber,
                            Size = pagedUsers.PageSize,
                            Sort = "Ascending",
                            Order = "Name",
                            TotalPage = pagedUsers.PageCount,
                            TotalItem = pagedUsers.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.keyWord,
                            role = null,
                            status = model.status,
                        },
                        PageData = pagedUsers.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<KitAccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<List<KitAccessoryResponseModel>>> GetAllKitAccessoryByKitId(string KitId)
        {
            try
            {
                var listKitAccessory = await _kitAccessoryRepository.GetAllKitAccessory();
                var result = listKitAccessory.Where(ka => ka.KitId.Equals(KitId)).ToList();
                return new BaseResponse<List<KitAccessoryResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,
                    Data = _mapper.Map<List<KitAccessoryResponseModel>>(result)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<KitAccessoryResponseModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitAccessoryResponseModel>> GetKitAccessoryById(int id)
        {
            try
            {
                var kitAccessory = await _kitAccessoryRepository.GetKitAccessoryById(id);
                if (kitAccessory != null)
                {
                    var result = _mapper.Map<KitAccessoryResponseModel>(kitAccessory);
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitAccessory!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitAccessoryResponseModel>> UpdateKitAccessory(int id, UpdateKitAccessoryRequestModel model)
        {
            try
            {
                var kitAccessory = await _kitAccessoryRepository.GetKitAccessoryById(id);
                if (kitAccessory != null)
                {
                    var result = _mapper.Map(model, kitAccessory);
                    await _kitAccessoryRepository.UpdateKitAccessory(result);
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<KitAccessoryResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<KitAccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitAccessory!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitAccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
