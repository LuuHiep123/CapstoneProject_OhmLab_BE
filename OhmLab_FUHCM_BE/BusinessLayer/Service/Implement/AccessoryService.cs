using AutoMapper;
using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.ResponseModel.Accessory;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Kit;
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
    public class AccessoryService : IAccessoryService
    {
        private readonly IAccessoryRepository _accessoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public AccessoryService(IAccessoryRepository accessoryRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _accessoryRepository = accessoryRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<BaseResponse<AccessoryResponseModel>> CreateAccessory(CreateAccessoryRequestModel model)
        {
            try
            {
                var accessory = _mapper.Map<Accessory>(model);
                accessory.AccessoryCreateDate = DateTime.Now;
                accessory.AccessoryStatus = "Valid";
                await _accessoryRepository.CreateAccessory(accessory);
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create Accessory Success!.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }


        public async Task<BaseResponse<AccessoryResponseModel>> DeleteAccessory(int id)
        {
            try
            {
                var accessory = await _accessoryRepository.GetAccessoryById(id);
                if (accessory == null)
                {
                    return new BaseResponse<AccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found accessory!"
                    };
                }
                accessory.AccessoryStatus = "Invalid";
                await _accessoryRepository.UpdateAccessory(accessory);
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete accessory success!",
                    Data = null
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<AccessoryResponseModel>> GetAccessoryById(int id)
        {
            try
            {
                var accessory = await _accessoryRepository.GetAccessoryById(id);
                if (accessory != null)
                {
                    var result = _mapper.Map<AccessoryResponseModel>(accessory);
                    return new BaseResponse<AccessoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<AccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found accessory!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<AccessoryResponseModel>> GetAllAccessory(GetAllAccessoryRequestModel model)
        {
            try
            {
                var listAccessory = await _accessoryRepository.GetAllAccessory();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<Accessory> listAccessoryByName = listAccessory.Where(a => a.AccessoryName.ToLower().Contains(model.keyWord.ToLower())).ToList();

                    List<Accessory> listAccessoryByValuCode = listAccessory.Where(a => a.AccessoryValueCode.ToLower().Contains(model.keyWord.ToLower())).ToList();

                    listAccessory = listAccessoryByName
                               .Concat(listAccessoryByValuCode)
                               .GroupBy(a => a.AccessoryId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listAccessory = listAccessory.Where(a => a.AccessoryStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<AccessoryResponseModel>>(listAccessory);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(a => a.AccessoryId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<AccessoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<AccessoryResponseModel>()
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
                return new DynamicResponse<AccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<AccessoryResponseModel>> UpdateAccessory(int id, UpdateAccessoryRequestModel model)
        {
            try
            {
                var accessory = await _accessoryRepository.GetAccessoryById(id);
                if (accessory != null)
                {
                    var result = _mapper.Map(model, accessory);
                    await _accessoryRepository.UpdateAccessory(result);
                    return new BaseResponse<AccessoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<AccessoryResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<AccessoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found accessory!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AccessoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
