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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
