using AutoMapper;
using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.EquipmentType;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.EquipmentType;
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
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public EquipmentTypeService(IEquipmentTypeRepository equipmentTypeRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        public async Task<BaseResponse<EquipmentTypeResponseModel>> CreateEquipmentType(CreateEquipmentTypeRequestModel model)
        {
            try
            {
                var equipmentType = _mapper.Map<EquipmentType>(model);
                equipmentType.EquipmentTypeQuantity = 0;
                equipmentType.EquipmentTypeStatus = "Available";
                equipmentType.EquipmentTypeCreateDate = DateTime.Now;
                equipmentType.EquipmentTypeId = GenerateRandomString(5);

                await _equipmentTypeRepository.CreateEquipmentType(equipmentType);

                return new BaseResponse<EquipmentTypeResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create equipmentType success!",
                    Data = null

                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentTypeResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"
                };
            }
        }

        public async Task<BaseResponse<EquipmentTypeResponseModel>> DeleteEquipmentType(string id)
        {
            try
            {
                var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeById(id);
                if (equipmentType != null)
                {
                    equipmentType.EquipmentTypeStatus = "Delete";
                    await _equipmentTypeRepository.UpdateEquipmentType(equipmentType);
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete success!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EquipmentType!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentTypeResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<EquipmentTypeResponseModel>> GetAllEquipmentType(GetAllEquipmentTypeRequestModel model)
        {
            try
            {
                var listEquipmentType = await _equipmentTypeRepository.GetAllEquipmentType();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<EquipmentType> listEquipmentTypeByName = listEquipmentType.Where(eqt => eqt.EquipmentTypeName.Contains(model.keyWord)).ToList();

                    listEquipmentType = listEquipmentTypeByName
                               .GroupBy(eqt=> eqt.EquipmentTypeId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listEquipmentType = listEquipmentType.Where(eq => eq.EquipmentTypeStatus.ToLower().Equals(model.status)).ToList();

                }
                var result = _mapper.Map<List<EquipmentTypeResponseModel>>(listEquipmentType);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(eqt => eqt.EquipmentTypeName) // Sắp xếp theo Name tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<EquipmentTypeResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<EquipmentTypeResponseModel>()
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
                return new DynamicResponse<EquipmentTypeResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<EquipmentTypeResponseModel>> GetEquipmentTypeById(string id)
        {
            try
            {
                var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeById(id);
                if (equipmentType != null || !equipmentType.EquipmentTypeStatus.Equals("Delete"))
                {
                    var result = _mapper.Map<EquipmentTypeResponseModel>(equipmentType);
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EquipmentType!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentTypeResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<EquipmentTypeResponseModel>> UpdateEquipmentType(string id, UpdateEquipmentTypeRequestModel model)
        {
            try
            {
                if(model.EquipmentTypeQuantity < 0)
                {
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Do not enter negative numbers!",
                        Data = null
                    };
                }
                var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeById(id);
                if (equipmentType != null || !equipmentType.EquipmentTypeStatus.Equals("Delete"))
                {
                    var result = _mapper.Map(model, equipmentType);
                    await _equipmentTypeRepository.UpdateEquipmentType(result);
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentTypeResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EquipmentType!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentTypeResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
