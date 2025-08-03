using AutoMapper;
using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
namespace BusinessLayer.Service.Implement
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public EquipmentService(IEquipmentRepository equipmentRepository, IEquipmentTypeRepository equipmentTypeRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _equipmentRepository = equipmentRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
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

        public async Task<BaseResponse<EquipmentResponseModel>> AddQR(string id, string UrlQR)
        {
            try
            {
                var equipment = await _equipmentRepository.GetEquipmentById(id);
                if (equipment == null)
                {
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "not found equipment!",
                        Data = null,
                    };
                }
                else
                {
                    equipment.EquipmentQr = UrlQR;
                    await _equipmentRepository.UpdateEquipment(equipment);
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "add QR success!",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<EquipmentResponseModel>> CreateEquipment(CreateEquipmentRequestModel model)
        {
            try
            {
                    var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeByCode(model.EquipmentCode);
                string equipmentTypeId = GenerateRandomString(5);
                string equipmentId = GenerateRandomString(5);
                if (equipmentType == null)
                {
                    equipmentType = new DataLayer.Entities.EquipmentType()
                    {
                        EquipmentTypeId = equipmentTypeId,
                        EquipmentTypeName = model.EquipmentName,
                        EquipmentTypeCode = model.EquipmentCode,
                        EquipmentTypeDescription = model.EquipmentDescription,  
                        EquipmentTypeQuantity = 1,
                        EquipmentTypeUrlImg = model.EquipmentTypeUrlImg,
                        EquipmentTypeCreateDate = DateTime.Now,
                        EquipmentTypeStatus = "Available"

                    };
                    await _equipmentTypeRepository.CreateEquipmentType(equipmentType);

                    var equipment = _mapper.Map<Equipment>(model);
                    equipment.EquipmentTypeId = equipmentTypeId;
                    equipment.EquipmentId = equipmentId;
                    equipment.EquipmentStatus = "Available";
                    equipment.EquipmentQr = GenerateQRCodeBase64(equipmentId);
                    await _equipmentRepository.CreateEquipment(equipment);
                    return new BaseResponse<EquipmentResponseModel>(){
                        Code = 200,
                        Success = true,
                        Message = "Create equipment success!",
                        Data = _mapper.Map<EquipmentResponseModel>(equipment)

                    };
                }
                else
                {
                    equipmentType.EquipmentTypeQuantity += 1;
                    await _equipmentTypeRepository.UpdateEquipmentType(equipmentType);

                    var equipment = _mapper.Map<Equipment>(model);
                    equipment.EquipmentTypeId = equipmentTypeId;
                    equipment.EquipmentId = equipmentId;
                    equipment.EquipmentQr = GenerateQRCodeBase64(equipmentId);
                    equipment.EquipmentStatus = "Available";
                    await _equipmentRepository.CreateEquipment(equipment);
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create equipment success!",
                        Data = _mapper.Map<EquipmentResponseModel>(equipment)

                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<EquipmentResponseModel>> DeleteEquipment(string id)
        {
            try
            {
                var equipment = await _equipmentRepository.GetEquipmentById(id);
                if (equipment != null)
                {
                    equipment.EquipmentStatus = "Delete";
                    await _equipmentRepository.UpdateEquipment(equipment);
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete success!.",
                        Data = _mapper.Map<EquipmentResponseModel>(equipment)
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Equipment!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<EquipmentResponseModel>> GetAllEquipment(GetAllEquipmentRequestModel model)
        {
            try
            {
                var listEquipment = await _equipmentRepository.GetAllEquipment();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<Equipment> listEquipmentByName = listEquipment.Where(eq => eq.EquipmentName.Contains(model.keyWord)).ToList();

                    List<Equipment> listEquipmentBySeri = listEquipment.Where(eq => eq.EquipmentNumberSerial.Contains(model.keyWord)).ToList();

                    listEquipment = listEquipmentByName
                               .Concat(listEquipmentBySeri)
                               .GroupBy(eq => eq.EquipmentId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listEquipment = listEquipment.Where(eq => eq.EquipmentStatus.ToLower().Equals(model.status)).ToList();

                }
                var result = _mapper.Map<List<EquipmentResponseModel>>(listEquipment);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(eq => eq.EquipmentName) // Sắp xếp theo Name tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<EquipmentResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<EquipmentResponseModel>()
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
                return new DynamicResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<EquipmentResponseModel>> GetEquipmentById(string id)
        {
            try
            {
                var equipment = await _equipmentRepository.GetEquipmentById(id);
                if (equipment != null || !equipment.EquipmentStatus.Equals("Delete"))
                {
                    var result = _mapper.Map<EquipmentResponseModel>(equipment);
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Equipment!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<EquipmentResponseModel>> UpdateEquipment(string id, UpdateEquipmentRequestModel model)
        {
            try
            {
                var equipment = await _equipmentRepository.GetEquipmentById(id);
                if (equipment != null || !equipment.EquipmentStatus.Equals("Delete"))
                {
                    var result = _mapper.Map(model, equipment);
                    await _equipmentRepository.UpdateEquipment(result);
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<EquipmentResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<EquipmentResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Equipment!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
