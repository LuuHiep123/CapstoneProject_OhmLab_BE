using AutoMapper;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Lab;

namespace BusinessLayer.Service.Implement
{
    public class LabService : ILabService
    {
        private readonly ILabRepository _labRepository;
        private readonly ILabEquipmentTypeRepository _labEquipmentTypeRepository;
        private readonly ILabKitTemplateRepository _labKitTemplateRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IMapper _mapper;

        public LabService(ILabRepository labRepository, 
                         ILabEquipmentTypeRepository labEquipmentTypeRepository,
                         ILabKitTemplateRepository labKitTemplateRepository,
                         IEquipmentTypeRepository equipmentTypeRepository,
                         IKitTemplateRepository kitTemplateRepository,
                         IMapper mapper)
        {
            _labRepository = labRepository;
            _labEquipmentTypeRepository = labEquipmentTypeRepository;
            _labKitTemplateRepository = labKitTemplateRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _mapper = mapper;
        }

        public async Task AddLab(CreateLabRequestModel labModel)
        {
            try
            {
                // Validation
                if (labModel.SubjectId <= 0)
                {
                    throw new ArgumentException("Subject ID phải lớn hơn 0!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabName))
                {
                    throw new ArgumentException("Tên lab không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabRequest))
                {
                    throw new ArgumentException("Yêu cầu lab không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabTarget))
                {
                    throw new ArgumentException("Mục tiêu lab không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabStatus))
                {
                    throw new ArgumentException("Trạng thái lab không được để trống!");
                }

                // Tạo lab
                var lab = _mapper.Map<Lab>(labModel);
                lab.LabStatus = labModel.LabStatus;
                await _labRepository.AddLab(lab);

                // Thêm equipment nếu có
                if (labModel.RequiredEquipments != null && labModel.RequiredEquipments.Any())
                {
                    foreach (var equipment in labModel.RequiredEquipments)
                    {
                        // Kiểm tra equipment type tồn tại
                        var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeById(equipment.EquipmentTypeId);
                        if (equipmentType == null)
                        {
                            throw new ArgumentException($"Không tìm thấy loại thiết bị với ID: {equipment.EquipmentTypeId}");
                        }

                        // Kiểm tra đã tồn tại chưa
                        var exists = await _labEquipmentTypeRepository.ExistsAsync(lab.LabId, equipment.EquipmentTypeId);
                        if (!exists)
                        {
                            var labEquipment = new LabEquipmentType
                            {
                                LabId = lab.LabId,
                                EquipmentTypeId = equipment.EquipmentTypeId,
                                
                            };
                            await _labEquipmentTypeRepository.CreateAsync(labEquipment);
                        }
                    }
                }

                // Thêm kit nếu có
                if (labModel.RequiredKits != null && labModel.RequiredKits.Any())
                {
                    foreach (var kit in labModel.RequiredKits)
                    {
                        // Kiểm tra kit template tồn tại
                        var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(kit.KitTemplateId);
                        if (kitTemplate == null)
                        {
                            throw new ArgumentException($"Không tìm thấy kit template với ID: {kit.KitTemplateId}");
                        }

                        // Kiểm tra đã tồn tại chưa
                        var exists = await _labKitTemplateRepository.ExistsAsync(lab.LabId, kit.KitTemplateId);
                        if (!exists)
                        {
                            var labKit = new LabKitTemplate
                            {
                                LabId = lab.LabId,
                                KitTemplateId = kit.KitTemplateId,
                               
                            };
                            await _labKitTemplateRepository.CreateAsync(labKit);
                        }
                    }
                }

                Console.WriteLine($"LabId after insert: {lab.LabId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo lab: {ex.Message}");
            }
        }

        public async Task DeleteLab(int id)
        {
            var lab = await _labRepository.GetLabById(id);
            if (lab != null)
            {
                // Lab có thể được xóa trực tiếp vì Report chỉ dùng để báo cáo sự cố
                lab.LabStatus = "Inactive";
                await _labRepository.UpdateLab(lab);
            }
        }

        public async Task<LabResponseModel> GetLabById(int id)
        {
            var lab = await _labRepository.GetLabById(id);
            return _mapper.Map<LabResponseModel>(lab);
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsBySubjectId(int subjectId)
        {
            var labs = await _labRepository.GetLabsBySubjectId(subjectId);
            var labResponses = labs.Select(l => _mapper.Map<LabResponseModel>(l)).ToList();

            return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách bài lab theo môn học thành công!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<LabResponseModel>
                {
                    PageData = labResponses,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = labResponses.Count,
                        TotalItem = labResponses.Count,
                        TotalPage = 1
                    },
                    SearchInfo = null
                }
            };
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsByLecturerId(string lecturerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lecturerId))
                {
                    return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Lecturer ID không được để trống!",
                        Data = null
                    };
                }

                var labs = await _labRepository.GetLabsByLecturerId(lecturerId);
                var labResponses = labs.Select(l => _mapper.Map<LabResponseModel>(l)).ToList();

                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách bài lab theo giảng viên thành công!",
                    Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<LabResponseModel>
                    {
                        PageData = labResponses,
                        PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                        {
                            Page = 1,
                            Size = labResponses.Count,
                            TotalItem = labResponses.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsByClassId(int classId)
        {
            try
            {
                if (classId <= 0)
                {
                    return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Class ID phải lớn hơn 0!",
                        Data = null
                    };
                }

                var labs = await _labRepository.GetLabsByClassId(classId);
                var labResponses = labs.Select(l => _mapper.Map<LabResponseModel>(l)).ToList();

                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách bài lab theo lớp học thành công!",
                    Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<LabResponseModel>
                    {
                        PageData = labResponses,
                        PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                        {
                            Page = 1,
                            Size = labResponses.Count,
                            TotalItem = labResponses.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task UpdateLab(int id, UpdateLabRequestModel labModel)
        {
            var lab = await _labRepository.GetLabById(id);
            if (lab != null)
            {
                _mapper.Map(labModel, lab);
                lab.LabId = id;
                await _labRepository.UpdateLab(lab);
            }
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetAllLabs()
        {
            var labs = await _labRepository.GetAllLabs();
            var labResponses = labs.Select(l => _mapper.Map<LabResponseModel>(l)).ToList();

            return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách tất cả bài lab thành công!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<LabResponseModel>
                {
                    PageData = labResponses,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = labResponses.Count,
                        TotalItem = labResponses.Count,
                        TotalPage = 1
                    },
                    SearchInfo = null
                }
            };
        }

        // Lab Equipment Management
        public async Task<BaseResponse<LabEquipmentResponseModel>> AddEquipmentToLab(int labId, AddLabEquipmentRequestModel model)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<LabEquipmentResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Kiểm tra equipment type tồn tại
                var equipmentType = await _equipmentTypeRepository.GetEquipmentTypeById(model.EquipmentTypeId);
                if (equipmentType == null)
                {
                    return new BaseResponse<LabEquipmentResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy loại thiết bị!",
                        Data = null
                    };
                }

                // Kiểm tra đã tồn tại chưa
                var exists = await _labEquipmentTypeRepository.ExistsAsync(labId, model.EquipmentTypeId);
                if (exists)
                {
                    return new BaseResponse<LabEquipmentResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Thiết bị này đã được thêm vào lab!",
                        Data = null
                    };
                }

                // Tạo lab equipment
                var labEquipment = new LabEquipmentType
                {
                    LabId = labId,
                    EquipmentTypeId = model.EquipmentTypeId,
                    LabEquipmentTypeStatus = model.Status
                };

                var result = await _labEquipmentTypeRepository.CreateAsync(labEquipment);
                
                // Lấy lại lab equipment với đầy đủ thông tin
                var labEquipmentWithDetails = await _labEquipmentTypeRepository.GetByIdAsync(result.LabEquipmentTypeId);
                var labEquipmentResponse = _mapper.Map<LabEquipmentResponseModel>(labEquipmentWithDetails);

                return new BaseResponse<LabEquipmentResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm thiết bị vào lab thành công!",
                    Data = labEquipmentResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LabEquipmentResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> RemoveEquipmentFromLab(int labId, string equipmentTypeId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = false
                    };
                }

                // Tìm lab equipment
                var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(labId);
                var labEquipment = labEquipments.FirstOrDefault(le => le.EquipmentTypeId == equipmentTypeId);

                if (labEquipment == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thiết bị trong lab!",
                        Data = false
                    };
                }

                // Xóa lab equipment
                var result = await _labEquipmentTypeRepository.DeleteAsync(labEquipment.LabEquipmentTypeId);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa thiết bị khỏi lab thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<List<LabEquipmentResponseModel>>> GetLabEquipments(int labId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<List<LabEquipmentResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(labId);
                var response = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);

                return new BaseResponse<List<LabEquipmentResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách thiết bị của lab thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<LabEquipmentResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        // Lab Kit Management
        public async Task<BaseResponse<LabKitResponseModel>> AddKitToLab(int labId, AddLabKitRequestModel model)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<LabKitResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                // Kiểm tra kit template tồn tại
                var kitTemplate = await _kitTemplateRepository.GetKitTemplateById(model.KitTemplateId);
                if (kitTemplate == null)
                {
                    return new BaseResponse<LabKitResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy kit template!",
                        Data = null
                    };
                }

                // Kiểm tra đã tồn tại chưa
                var exists = await _labKitTemplateRepository.ExistsAsync(labId, model.KitTemplateId);
                if (exists)
                {
                    return new BaseResponse<LabKitResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Kit này đã được thêm vào lab!",
                        Data = null
                    };
                }

                // Tạo lab kit
                var labKit = new LabKitTemplate
                {
                    LabId = labId,
                    KitTemplateId = model.KitTemplateId,
                    LabKitTemplateStatus = model.Status
                };

                var result = await _labKitTemplateRepository.CreateAsync(labKit);
                
                // Lấy lại lab kit với đầy đủ thông tin
                var labKitWithDetails = await _labKitTemplateRepository.GetByIdAsync(result.LabKitTemplateId);
                var labKitResponse = _mapper.Map<LabKitResponseModel>(labKitWithDetails);

                return new BaseResponse<LabKitResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm kit vào lab thành công!",
                    Data = labKitResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LabKitResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> RemoveKitFromLab(int labId, string kitTemplateId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = false
                    };
                }

                // Tìm lab kit
                var labKits = await _labKitTemplateRepository.GetByLabIdAsync(labId);
                var labKit = labKits.FirstOrDefault(lk => lk.KitTemplateId == kitTemplateId);

                if (labKit == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy kit trong lab!",
                        Data = false
                    };
                }

                // Xóa lab kit
                var result = await _labKitTemplateRepository.DeleteAsync(labKit.LabKitTemplateId);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa kit khỏi lab thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<List<LabKitResponseModel>>> GetLabKits(int labId)
        {
            try
            {
                // Kiểm tra lab tồn tại
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<List<LabKitResponseModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lab!",
                        Data = null
                    };
                }

                var labKits = await _labKitTemplateRepository.GetByLabIdAsync(labId);
                var response = _mapper.Map<List<LabKitResponseModel>>(labKits);

                return new BaseResponse<List<LabKitResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách kit của lab thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<LabKitResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }
    }
} 