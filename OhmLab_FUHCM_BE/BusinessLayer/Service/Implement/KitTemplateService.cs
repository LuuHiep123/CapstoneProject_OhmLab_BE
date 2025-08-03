using AutoMapper;
using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.KitTemplate;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
    public class KitTemplateService : IKitTemplateService
    {
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public KitTemplateService(IKitTemplateRepository kitTemplateRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _kitTemplateRepository = kitTemplateRepository;
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

        public async Task<BaseResponse<KitTemplateResponseModel>> CreateKitTemplate(CreateKitTemplateRequestModel model)
        {
            try
            {
                string kidTemplateId = GenerateRandomString(5);
                var kitTemplate = _mapper.Map<KitTemplate>(model);
                kitTemplate.KitTemplateId = kidTemplateId;
                kitTemplate.KitTemplateStatus = "Valid";
                kitTemplate.KitTemplateQuantity = 0;

                await _kitTemplateRepository.CreateKitTemplate(kitTemplate);
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create KitTemplate Success!.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitTemplateResponseModel>> DeleteKitTemplate(string id)
        {
            try
            {
                var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(id);
                if (kitTemplate == null)
                {
                    return new BaseResponse<KitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitTemplate!"
                    };
                }
                kitTemplate.KitTemplateStatus = "Invalid";
                await _kitTemplateRepository.UpdateKitTemplate(kitTemplate);
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete KitTemplate success!",
                    Data = null           
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<KitTemplateResponseModel>> GetAllKitTemplate(GetAllKitTemplateRequestModel model)
        {
            try
            {
                var listKitTemplate= await _kitTemplateRepository.GetAllKitTemplate();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<KitTemplate> listKitTemplateByName = listKitTemplate.Where(kt => kt.KitTemplateName.Contains(model.keyWord)).ToList();

                    listKitTemplate = listKitTemplateByName
                               .GroupBy(u => u.KitTemplateId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listKitTemplate = listKitTemplate.Where(kt => kt.KitTemplateStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<KitTemplateResponseModel>>(listKitTemplate);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.KitTemplateName) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<KitTemplateResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<KitTemplateResponseModel>()
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
                return new DynamicResponse<KitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<KitTemplateResponseModel>> GetKitTemplateById(string id)
        {
            try
            {
                var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(id);
                if (kitTemplate != null)
                {
                    var result = _mapper.Map<KitTemplateResponseModel>(kitTemplate);
                    return new BaseResponse<KitTemplateResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<KitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitTemplate!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitTemplateResponseModel>> UpdateKitTemplate(string id, UpdateKitTemplateRequestModel model)
        {
            try
            {
                var kitTemmplate = await _kitTemplateRepository.GetKitTemplateById(id);
                if (kitTemmplate != null)
                {
                    var result = _mapper.Map(model, kitTemmplate);
                    await _kitTemplateRepository.UpdateKitTemplate(result);
                    return new BaseResponse<KitTemplateResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<KitTemplateResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<KitTemplateResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found KitTemplate!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitTemplateResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
