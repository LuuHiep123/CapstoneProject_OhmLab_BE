using AutoMapper;
using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.Accessory;
using BusinessLayer.ResponseModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
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
    public class AccessoryKitTemplateService : IAccessoryKitTemplateService
    {

        private readonly IAccessoryKitTemplateRepository _accessoryKitTemplateRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IAccessoryRepository _accessoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public AccessoryKitTemplateService(IAccessoryRepository accessoryRepository, IKitTemplateRepository kitTemplateRepository, IAccessoryKitTemplateRepository accessoryKitTemplateRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _accessoryKitTemplateRepository = accessoryKitTemplateRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _accessoryRepository = accessoryRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<BaseResponse<AccessoryKitTemplateResponseModel>> CreateAccessoryKitTemplate(CreateAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(model.KitTemplateId);
                if (kitTemplate == null)
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitTemplate!.",
                        Data = null
                    };
                }

                var accessory = await _accessoryRepository.GetAccessoryById(model.AccessoryId);
                if (accessory == null)
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found accessory!.",
                        Data = null
                    };
                }

                if(model.AccessoryQuantity <= 0)
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 402,
                        Success = false,
                        Message = "Quantity must not be less than or equal to 0!.",
                        Data = null
                    };
                }
                var accessoryKitTemplate = _mapper.Map<AccessoryKitTemplate>(model);
                accessoryKitTemplate.AccessoryKitTemplateStatus = "Valid";
                await _accessoryKitTemplateRepository.CreateAccessoryKitTemplate(accessoryKitTemplate);
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create AssoryKitTemplate Success!.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<AccessoryKitTemplateResponseModel>> DeleteAccessoryKitTemplate(int id)
        {
            try
            {
                var accessoryKitTemplate = await _accessoryKitTemplateRepository.GetAccessoryKitTemplateById(id);
                if (accessoryKitTemplate == null)
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found AccessoryKitTemplate!"
                    };
                }
                accessoryKitTemplate.AccessoryKitTemplateStatus = "Invalid";
                await _accessoryKitTemplateRepository.UpdateAccessoryKitTemplate(accessoryKitTemplate);
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete AccessoryKitTemplate success!",
                    Data = null
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<AccessoryKitTemplateResponseModel>> GetAccessoryKitTemplateById(int id)
        {
            try
            {
                var accessoryKitTemplate = await _accessoryKitTemplateRepository.GetAccessoryKitTemplateById(id);
                if (accessoryKitTemplate != null)
                {
                    var result = _mapper.Map<AccessoryKitTemplateResponseModel>(accessoryKitTemplate);
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found AccessoryKitTemplate!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<AccessoryKitTemplateResponseModel>> GetAllAccessoryKitTemplate(GetAllAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var listAccessoryKitTemplate = await _accessoryKitTemplateRepository.GetAllAccessoryKitTemplate();
                if (!string.IsNullOrEmpty(model.status))
                {
                    listAccessoryKitTemplate = listAccessoryKitTemplate.Where(ak=> ak.AccessoryKitTemplateStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<AccessoryKitTemplateResponseModel>>(listAccessoryKitTemplate);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(ak => ak.AccessoryKitTemplateId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<AccessoryKitTemplateResponseModel>()
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
                return new DynamicResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<List<AccessoryKitTemplateResponseModel>>> GetAllAccessoryKitTemplateByKitTemplateId(string kitTemplateId)
        {
            try
            {
                var accessoryKitTemplate = await _accessoryKitTemplateRepository.GetAllAccessoryKitTemplate();
                var result = accessoryKitTemplate.Where(ak => ak.KitTemplateId.Equals(kitTemplateId)).ToList();
                return new BaseResponse<List<AccessoryKitTemplateResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,
                    Data = _mapper.Map<List<AccessoryKitTemplateResponseModel>>(result)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AccessoryKitTemplateResponseModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<AccessoryKitTemplateResponseModel>> UpdateAccessoryKitTemplate(int id, UpdateAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var accessoryKitTemplate = await _accessoryKitTemplateRepository.GetAccessoryKitTemplateById(id);
                if (accessoryKitTemplate != null)
                {
                    var result = _mapper.Map(model, accessoryKitTemplate);
                    await _accessoryKitTemplateRepository.UpdateAccessoryKitTemplate(result);
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<AccessoryKitTemplateResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<AccessoryKitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found AccessoryKitTemplate!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryKitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
