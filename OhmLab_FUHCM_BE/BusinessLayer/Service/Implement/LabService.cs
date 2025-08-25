using AutoMapper;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security;
using BusinessLayer.ResponseModel.BaseResponse;
using System.Text.Json;
using static System.Console;

namespace BusinessLayer.Service.Implement
{
    public class LabService : ILabService
    {
        private readonly ILabRepository _labRepository;
        private readonly ILabEquipmentTypeRepository _labEquipmentTypeRepository;
        private readonly ILabKitTemplateRepository _labKitTemplateRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IClassRepository _classRepository;           // ✅ THÊM MỚI
        private readonly IScheduleRepository _scheduleRepository;     // ✅ THÊM MỚI
        private readonly IMapper _mapper;

        public LabService(ILabRepository labRepository, 
                         ILabEquipmentTypeRepository labEquipmentTypeRepository,
                         ILabKitTemplateRepository labKitTemplateRepository,
                         IEquipmentTypeRepository equipmentTypeRepository,
                         IKitTemplateRepository kitTemplateRepository,
                         IClassRepository classRepository,            // ✅ THÊM MỚI
                         IScheduleRepository scheduleRepository,      // ✅ THÊM MỚI
                         IMapper mapper)
        {
            _labRepository = labRepository;
            _labEquipmentTypeRepository = labEquipmentTypeRepository;
            _labKitTemplateRepository = labKitTemplateRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _classRepository = classRepository;                      // ✅ THÊM MỚI
            _scheduleRepository = scheduleRepository;                // ✅ THÊM MỚI
            _mapper = mapper;
        }

        public async Task AddLab(CreateLabRequestModel labModel, Guid currentUserId, string userRole)
        {
            try
            {
                // ✅ THÊM MỚI: Kiểm tra quyền - CHỈ Head of Department mới được tạo lab
                if (userRole != "HeadOfDepartment")
                {
                    throw new UnauthorizedAccessException("Chỉ Head of Department mới được tạo bài lab!");
                }

                // Validation
                if (labModel.SubjectId <= 0)
                {
                    throw new ArgumentException("Subject ID phải lớn hơn 0!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabName))
                {
                    throw new ArgumentException("Tên lab không được để trống!");
                }

                if (string.IsNullOrWhiteSpace(labModel.LabTarget))
                {
                    throw new ArgumentException("Mục tiêu lab không được để trống!");
                }

                if (labModel.RequiredSlots <= 0)
                {
                    throw new ArgumentException("Số buổi yêu cầu phải lớn hơn 0!");
                }

                if (string.IsNullOrWhiteSpace(labModel.AssignmentType))
                {
                    throw new ArgumentException("Loại bài tập không được để trống!");
                }

                if (labModel.AssignmentCount <= 0)
                {
                    throw new ArgumentException("Số lượng bài tập phải lớn hơn 0!");
                }

                // Tạo lab
                var lab = _mapper.Map<Lab>(labModel);
                lab.LabStatus = labModel.LabStatus ?? "Active";

                // ✅ Lưu 'quy định thực hành' theo yêu cầu (slots, assignments) vào LabRequest (JSON)
                var regulation = new
                {
                    requiredSlots = labModel.RequiredSlots,
                    assignmentType = labModel.AssignmentType,
                    assignmentCount = labModel.AssignmentCount,
                    gradingCriteria = labModel.GradingCriteria,
                    note = labModel.LabRequest
                };
                lab.LabRequest = JsonSerializer.Serialize(regulation);

                await _labRepository.AddLab(lab);

                // Thêm equipment nếu có
                if (labModel.RequiredEquipments != null && labModel.RequiredEquipments.Any())
                {
                    foreach (var equipment in labModel.RequiredEquipments)
                    {
                        // Guard: phần tử null hoặc thiếu id thì bỏ qua
                        if (equipment == null || string.IsNullOrWhiteSpace(equipment.EquipmentTypeId))
                        {
                            continue;
                        }

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
                                LabEquipmentTypeStatus = string.IsNullOrWhiteSpace(equipment.Status) ? "Active" : equipment.Status
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
                        // Guard: phần tử null hoặc thiếu id thì bỏ qua
                        if (kit == null || string.IsNullOrWhiteSpace(kit.KitTemplateId))
                        {
                            continue;
                        }

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
                                LabKitTemplateStatus = string.IsNullOrWhiteSpace(kit.Status) ? "Active" : kit.Status
                            };
                            await _labKitTemplateRepository.CreateAsync(labKit);
                        }
                    }
                }

                WriteLine($"LabId after insert: {lab.LabId}");
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
            if (lab == null) return null;
            
            var labResponse = _mapper.Map<LabResponseModel>(lab);
            
            // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
            labResponse.ParseLabRequest();
            
            // ✅ THÊM MỚI: Lấy required equipment
            var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
            labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
            
            // ✅ THÊM MỚI: Lấy required kits
            var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
            labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
            
            return labResponse;
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsBySubjectId(int subjectId)
        {
            var labs = await _labRepository.GetLabsBySubjectId(subjectId);
            var labResponses = new List<LabResponseModel>();

            foreach (var lab in labs)
            {
                var labResponse = _mapper.Map<LabResponseModel>(lab);
                
                // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
                labResponse.ParseLabRequest();
                
                // ✅ THÊM MỚI: Lấy required equipment
                var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                
                // ✅ THÊM MỚI: Lấy required kits
                var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                
                labResponses.Add(labResponse);
            }

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
                var labResponses = new List<LabResponseModel>();

                foreach (var lab in labs)
                {
                    var labResponse = _mapper.Map<LabResponseModel>(lab);
                    
                    // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
                    labResponse.ParseLabRequest();
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    labResponses.Add(labResponse);
                }

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
                var labResponses = new List<LabResponseModel>();

                foreach (var lab in labs)
                {
                    var labResponse = _mapper.Map<LabResponseModel>(lab);
                    
                    // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
                    labResponse.ParseLabRequest();
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    labResponses.Add(labResponse);
                }

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
            try
            {
                var labs = await _labRepository.GetAllLabs();
                var labResponses = new List<LabResponseModel>();

                foreach (var lab in labs)
                {
                    var labResponse = _mapper.Map<LabResponseModel>(lab);
                    
                    // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
                    labResponse.ParseLabRequest();
                    
                    // ✅ THÊM MỚI: Lấy TotalAssignments (tổng lượng bài lab)
                    labResponse.TotalAssignments = labs.Count;
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    labResponses.Add(labResponse);
                }

                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lab thành công!",
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
                    Message = $"Lỗi khi lấy danh sách lab: {ex.Message}",
                    Data = null
                };
            }
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

        // ✅ THÊM MỚI: Method để Lecturer xem lab cho lớp mình phụ trách
        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsForMyClasses(Guid lecturerId)
        {
            try
            {
                // Lấy các lớp mà lecturer này phụ trách
                var myClasses = await _classRepository.GetByLecturerIdAsync(lecturerId);
                var mySubjectIds = myClasses.Select(c => c.SubjectId).Distinct().ToList();
                
                // Lấy các lab thuộc các môn mà lecturer dạy
                var labs = new List<Lab>();
                foreach (var subjectId in mySubjectIds)
                {
                    var subjectLabs = await _labRepository.GetLabsBySubjectId(subjectId);
                    labs.AddRange(subjectLabs);
                }
                var labResponses = new List<LabResponseModel>();

                foreach (var lab in labs)
                {
                    var labResponse = _mapper.Map<LabResponseModel>(lab);
                    
                    // ✅ THÊM MỚI: Parse JSON từ LabRequest để hiển thị assignment info
                    labResponse.ParseLabRequest();
                    
                    // ✅ THÊM MỚI: Lấy TotalAssignments (tổng lượng bài lab)
                    labResponse.TotalAssignments = labs.Count;
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    labResponses.Add(labResponse);
                }

                return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lab cho lớp của bạn thành công!",
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
                    Message = $"Lỗi khi lấy danh sách lab: {ex.Message}",
                    Data = null
                };
            }
        }

        // ✅ THÊM MỚI: Method để Lecturer tạo lịch lab cho lớp
        public async Task<BaseResponse<bool>> CreateLabSchedule(int labId, int classId, DateTime scheduledDate, int slotId, Guid lecturerId)
        {
            try
            {
                // Kiểm tra lecturer có phụ trách lớp này không
                var classEntity = await _classRepository.GetByIdAsync(classId);
                if (classEntity?.LecturerId != lecturerId)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Bạn không phụ trách lớp này!",
                        Data = false
                    };
                }

                // Kiểm tra lab có tồn tại và thuộc môn học của lớp không
                var lab = await _labRepository.GetLabById(labId);
                if (lab?.SubjectId != classEntity.SubjectId)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Lab không thuộc môn học của lớp!",
                        Data = false
                    };
                }

                // Tạo lịch lab (sử dụng entity Schedule hiện tại)
                var schedule = new Schedule
                {
                    ClassId = classId,
                    ScheduleName = $"Lab: {lab.LabName}",
                    ScheduleDate = scheduledDate,
                    ScheduleDescription = $"Thực hành: {lab.LabRequest}"
                };

                await _scheduleRepository.CreateAsync(schedule);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo lịch lab thành công!",
                    Data = true
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
    }
} 