using AutoMapper;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.KitTemplate;
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
    public class KitService : IKitService
    {
        private readonly IKitRepository _kitRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public KitService(IKitRepository kitRepository, IKitTemplateRepository kitTemplateRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _kitRepository = kitRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public static string GenerateQRCodeBase64(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            var pngQRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPng = pngQRCode.GetGraphic(20); // độ phân giải: pixel per module

            return Convert.ToBase64String(qrCodeAsPng);
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

        public async Task<BaseResponse<KitResponseModel>> CreateKit(CreateKitRequestModel model)
        {
            try
            {
                var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(model.KitTemplateId);
                if (kitTemplate == null)
                {
                    return new BaseResponse<KitResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not Found KitTemplateId!."

                    };
                }
                var checkKitName = await _kitRepository.GetKitByName(model.KitName);
                if(checkKitName != null)
                {
                    return new BaseResponse<KitResponseModel>()
                    {
                        Code = 401,
                        Success = false,
                        Message = "Duppicate Kit Name!."

                    };
                }
                string kidId = GenerateRandomString(5);
                var kit = _mapper.Map<Kit>(model);
                kit.KitId = kidId;
                kit.KitStatus = "Valid";
                kit.KitCreateDate = DateTime.Now;
                kit.KitUrlQr = GenerateQRCodeBase64(kidId);

                await _kitRepository.CreateKit(kit);
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create Kit Success!.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitResponseModel>> DeleteKit(string id)
        {
            try
            {
                var kit = await _kitRepository.GetKitById(id);
                if (kit == null)
                {
                    return new BaseResponse<KitResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Kit!"
                    };
                }
                kit.KitStatus = "Invalid";
                await _kitRepository.UpdateKit(kit);
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Delete Kit success!",
                    Data = null
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<KitResponseModel>> GetAllKit(GetAllKitRequestModel model)
        {
            try
            {
                var listKit= await _kitRepository.GetAllKit();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<Kit> listKitByName = listKit.Where(k => k.KitName.Contains(model.keyWord)).ToList();

                    listKit = listKitByName
                               .GroupBy(u => u.KitTemplateId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listKit = listKit.Where(k => k.KitStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<KitResponseModel>>(listKit);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(k => k.KitName) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<KitResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<KitResponseModel>()
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
                return new DynamicResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<DynamicResponse<KitResponseModel>> GetAllKitByKitTempalteId(GetAllKitByKitTemplateIdRequestModel model)
        {
            try
            {
                var listKit = await _kitRepository.GetAllKitByKitTemplateId(model.kitTemplateId);
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<Kit> listKitByName = listKit.Where(k => k.KitName.Contains(model.keyWord)).ToList();

                    listKit = listKitByName
                               .GroupBy(u => u.KitTemplateId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listKit = listKit.Where(k => k.KitStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<KitResponseModel>>(listKit);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(k => k.KitName) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<KitResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<KitResponseModel>()
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
                return new DynamicResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<KitResponseModel>> GetKitById(string id)
        {
            try
            {
                var kit = await _kitRepository.GetKitById(id);
                if (kit != null)
                {
                    var result = _mapper.Map<KitResponseModel>(kit);
                    return new BaseResponse<KitResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<KitResponseModel>()
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
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<KitResponseModel>> UpdateKit(string id, UpdateKitRequestModel model)
        {
            try
            {
                var kit = await _kitRepository.GetKitById(id);
                if (kit != null)
                {
                    var result = _mapper.Map(model, kit);
                    await _kitRepository.UpdateKit(result);
                    return new BaseResponse<KitResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<KitResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<KitResponseModel>()
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
                return new BaseResponse<KitResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
