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
        private readonly IScheduleTypeRepository _scheduleTypeRepository; // ✅ THÊM MỚI
        private readonly ISlotRepository _slotRepository;            // ✅ THÊM MỚI để lấy slot info
        private readonly IMapper _mapper;

        public LabService(ILabRepository labRepository, 
                         ILabEquipmentTypeRepository labEquipmentTypeRepository,
                         ILabKitTemplateRepository labKitTemplateRepository,
                         IEquipmentTypeRepository equipmentTypeRepository,
                         IKitTemplateRepository kitTemplateRepository,
                         IClassRepository classRepository,            // ✅ THÊM MỚI
                         IScheduleRepository scheduleRepository,      // ✅ THÊM MỚI
                         IScheduleTypeRepository scheduleTypeRepository, // ✅ THÊM MỚI
                         ISlotRepository slotRepository,             // ✅ THÊM MỚI để lấy slot info
                         IMapper mapper)
        {
            _labRepository = labRepository;
            _labEquipmentTypeRepository = labEquipmentTypeRepository;
            _labKitTemplateRepository = labKitTemplateRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _classRepository = classRepository;                      // ✅ THÊM MỚI
            _scheduleRepository = scheduleRepository;                // ✅ THÊM MỚI
            _scheduleTypeRepository = scheduleTypeRepository;        // ✅ THÊM MỚI
            _slotRepository = slotRepository;                       // ✅ THÊM MỚI để lấy slot info
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

            

                // Tạo lab
                var lab = _mapper.Map<Lab>(labModel);
                lab.LabStatus = labModel.LabStatus ?? "Active";

             

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
                                LabEquipmentTypeStatus = "Active"  // ✅ LUÔN DÙNG "Active"
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
                                LabKitTemplateStatus = "Active"  
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
            
            
            
            // ✅ THÊM MỚI: Lấy required equipment
            var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
            labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
            
            // ✅ THÊM MỚI: Lấy required kits
            var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
            labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
            
            // ✅ THÊM MỚI: Lấy slot info thông qua Subject
            await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
            
            return labResponse;
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsBySubjectId(int subjectId)
        {
            var labs = await _labRepository.GetLabsBySubjectId(subjectId);
            var labResponses = new List<LabResponseModel>();

            foreach (var lab in labs)
            {
                var labResponse = _mapper.Map<LabResponseModel>(lab);
                
               
                
                // ✅ THÊM MỚI: Lấy required equipment
                var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                
                // ✅ THÊM MỚI: Lấy required kits
                var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                
                // ✅ THÊM MỚI: Lấy slot info thông qua Subject
                await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
                
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
                    
                   
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    // ✅ THÊM MỚI: Lấy slot info thông qua Subject
                    await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
                    
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
                    
                    
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    // ✅ THÊM MỚI: Lấy slot info thông qua Subject
                    await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
                    
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
                    
                 
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    // ✅ THÊM MỚI: Lấy slot info thông qua Subject
                    await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
                    
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
                  
                    
                    // ✅ THÊM MỚI: Lấy required equipment
                    var labEquipments = await _labEquipmentTypeRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredEquipments = _mapper.Map<List<LabEquipmentResponseModel>>(labEquipments);
                    
                    // ✅ THÊM MỚI: Lấy required kits
                    var labKits = await _labKitTemplateRepository.GetByLabIdAsync(lab.LabId);
                    labResponse.RequiredKits = _mapper.Map<List<LabKitResponseModel>>(labKits);
                    
                    // ✅ THÊM MỚI: Lấy slot info thông qua Subject
                    await PopulateSlotInfoForLab(labResponse, lab.SubjectId);
                    
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
                

                // Tạo lịch lab (sử dụng entity Schedule hiện tại)
                var schedule = new Schedule
                {
                    ClassId = classId,
                    ScheduleName = $"Lab: {lab.LabName}",
                    ScheduleDate = scheduledDate,
                    ScheduleDescription = $"Thực hành: {lab.LabRequest} | SlotId: {slotId}" // ✅ THÊM MỚI: Lưu slotId vào description
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

        // ✅ THÊM MỚI: Helper method để populate slot info cho lab response
        private async Task PopulateSlotInfoForLab(LabResponseModel labResponse, int subjectId)
        {
            try
            {
                // Tìm các lớp có môn học này để lấy slot info
                var classesWithSubject = await _classRepository.GetAllAsync();
                var classesForSubject = classesWithSubject.Where(c => c.SubjectId == subjectId).ToList();
                
                if (classesForSubject.Any())
                {
                    var firstClass = classesForSubject.First();
                    
                    // ✅ THÊM MỚI: Populate class info
                    labResponse.ClassId = firstClass.ClassId;
                    labResponse.ClassName = firstClass.ClassName;
                    
                    // ✅ THÊM MỚI: ƯU TIÊN lấy slot info từ Schedule đã tạo (nếu có)
                    var schedules = await _scheduleRepository.GetAllAsync();
                    var labSchedule = schedules.FirstOrDefault(s => s.ClassId == firstClass.ClassId && 
                                                                   s.ScheduleDescription != null && 
                                                                   s.ScheduleDescription.Contains("Thực hành"));
                    if (labSchedule != null)
                    {
                        labResponse.ScheduledDate = labSchedule.ScheduleDate;
                        
                        // ✅ THÊM MỚI: Parse slotId từ ScheduleDescription
                        if (labSchedule.ScheduleDescription.Contains("| SlotId:"))
                        {
                            var slotIdPart = labSchedule.ScheduleDescription.Split("| SlotId:")[1].Trim();
                            if (int.TryParse(slotIdPart, out int parsedSlotId))
                            {
                                // Lấy slot info từ slotId đã lưu
                                var slot = await _slotRepository.GetByIdAsync(parsedSlotId);
                                if (slot != null)
                                {
                                    labResponse.SlotId = slot.SlotId;
                                    labResponse.SlotName = slot.SlotName;
                                    labResponse.SlotStartTime = slot.SlotStartTime;
                                    labResponse.SlotEndTime = slot.SlotEndTime;
                                }
                            }
                        }
                    }
                    
                    // ✅ FALLBACK: Lấy slot info từ Class → ScheduleType → Slot (nếu không có Schedule)
                    if (firstClass.ScheduleTypeId != null)
                    {
                        var scheduleType = await _scheduleTypeRepository.GetByIdAsync(firstClass.ScheduleTypeId.Value);
                        if (scheduleType?.Slot != null)
                        {
                            // Chỉ set slot info nếu chưa có từ Schedule
                            if (labResponse.SlotId == null)
                            {
                                labResponse.SlotId = scheduleType.Slot.SlotId;
                                labResponse.SlotName = scheduleType.Slot.SlotName;
                                labResponse.SlotStartTime = scheduleType.Slot.SlotStartTime;
                                labResponse.SlotEndTime = scheduleType.Slot.SlotEndTime;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error nhưng không làm crash method chính
                Console.WriteLine($"Lỗi khi populate slot info: {ex.Message}");
            }
        }

        // ✅ THÊM MỚI: Method debug để kiểm tra tại sao slot info bị null
        public async Task<string> DebugSlotInfoForLab(int labId)
        {
            try
            {
                var debugInfo = new List<string>();
                debugInfo.Add($"🔍 Debug Slot Info cho LabId = {labId}");
                
                // 1. Kiểm tra Lab có tồn tại không
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    debugInfo.Add($"❌ Lab với ID {labId} không tồn tại!");
                    return string.Join("\n", debugInfo);
                }
                debugInfo.Add($"✅ Lab tồn tại: {lab.LabName}, SubjectId: {lab.SubjectId}");
                
                // 2. Kiểm tra các lớp có môn học này
                var allClasses = await _classRepository.GetAllAsync();
                var classesForSubject = allClasses.Where(c => c.SubjectId == lab.SubjectId).ToList();
                debugInfo.Add($"📊 Tìm thấy {classesForSubject.Count} lớp cho SubjectId = {lab.SubjectId}");
                
                foreach (var classEntity in classesForSubject)
                {
                    debugInfo.Add($"   - ClassId: {classEntity.ClassId}, ClassName: {classEntity.ClassName}, ScheduleTypeId: {classEntity.ScheduleTypeId}");
                    
                    if (classEntity.ScheduleTypeId != null)
                    {
                        var scheduleType = await _scheduleTypeRepository.GetByIdAsync(classEntity.ScheduleTypeId.Value);
                        if (scheduleType != null)
                        {
                            debugInfo.Add($"     📅 ScheduleType: {scheduleType.ScheduleTypeName}, SlotId: {scheduleType.SlotId}");
                            
                            if (scheduleType.Slot != null)
                            {
                                debugInfo.Add($"       🕐 Slot: {scheduleType.Slot.SlotName} ({scheduleType.Slot.SlotStartTime} - {scheduleType.Slot.SlotEndTime})");
                            }
                            else
                            {
                                debugInfo.Add($"       ❌ Slot không tồn tại!");
                            }
                        }
                        else
                        {
                            debugInfo.Add($"     ❌ ScheduleType không tồn tại!");
                        }
                    }
                    else
                    {
                        debugInfo.Add($"     ❌ Lớp không có ScheduleTypeId");
                    }
                    
                    // 3. Kiểm tra Schedule của lớp này
                    var schedules = await _scheduleRepository.GetAllAsync();
                    var classSchedules = schedules.Where(s => s.ClassId == classEntity.ClassId).ToList();
                    debugInfo.Add($"     📋 Tìm thấy {classSchedules.Count} schedule cho lớp này");
                    
                    foreach (var schedule in classSchedules)
                    {
                        debugInfo.Add($"       - ScheduleId: {schedule.ScheduleId}, Name: {schedule.ScheduleName}, Date: {schedule.ScheduleDate}");
                    }
                }
                
                return string.Join("\n", debugInfo);
            }
            catch (Exception ex)
            {
                return $"❌ Lỗi khi debug: {ex.Message}\nStack trace: {ex.StackTrace}";
            }
        }
    }
}