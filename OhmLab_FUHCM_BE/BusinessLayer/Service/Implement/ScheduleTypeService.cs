using AutoMapper;
using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.ScheduleType;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Service.Implement
{
    public class ScheduleTypeService : IScheduleTypeService
    {
        private readonly IScheduleTypeRepository _scheduleTypeRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IClassRepository _classRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ScheduleTypeService> _logger;

        public ScheduleTypeService(
            IScheduleTypeRepository scheduleTypeRepository,
            ISlotRepository slotRepository,
            IClassRepository classRepository,
            IScheduleRepository scheduleRepository,
            IMapper mapper,
            ILogger<ScheduleTypeService> logger)
        {
            _scheduleTypeRepository = scheduleTypeRepository;
            _slotRepository = slotRepository;
            _classRepository = classRepository;
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ScheduleTypeResponseModel>> CreateScheduleTypeAsync(CreateScheduleTypeRequestModel model)
        {
            try
            {
                // Kiểm tra Slot tồn tại
                var slot = await _slotRepository.GetByIdAsync(model.SlotId);
                if (slot == null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy ca học!",
                        Data = null
                    };
                }

                // Kiểm tra tên ScheduleType đã tồn tại chưa
                var existingScheduleTypes = await _scheduleTypeRepository.GetAllAsync();
                var existingScheduleType = existingScheduleTypes.Any(st => 
                    st.ScheduleTypeName.ToLower() == model.ScheduleTypeName.ToLower() && st.SlotId == model.SlotId && st.ScheduleTypeDow == model.ScheduleTypeDow);
                
                if (existingScheduleType != null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Tên loại lịch học đã tồn tại!",
                        Data = null
                    };
                }

                var scheduleType = new ScheduleType
                {
                    SlotId = model.SlotId,
                    ScheduleTypeName = model.ScheduleTypeName,
                    ScheduleTypeDescription = model.ScheduleTypeDescription,
                    ScheduleTypeDow = model.ScheduleTypeDow,
                    ScheduleTypeStatus = model.ScheduleTypeStatus
                };

                var result = await _scheduleTypeRepository.CreateAsync(scheduleType);
                var response = await MapToScheduleTypeResponseModel(result);

                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo loại lịch học thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateScheduleTypeAsync: {Message}", ex.Message);
                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ScheduleTypeResponseModel>> GetScheduleTypeByIdAsync(int id)
        {
            try
            {
                var scheduleType = await _scheduleTypeRepository.GetByIdAsync(id);
                if (scheduleType == null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy loại lịch học!",
                        Data = null
                    };
                }

                var response = await MapToScheduleTypeResponseModel(scheduleType);

                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin loại lịch học thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetScheduleTypeByIdAsync: {Message}", ex.Message);
                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<ScheduleTypeResponseModel>>> GetAllScheduleTypesAsync()
        {
            try
            {
                var scheduleTypes = await _scheduleTypeRepository.GetAllAsync();
                var responses = new List<ScheduleTypeResponseModel>();

                foreach (var scheduleType in scheduleTypes)
                {
                    var response = await MapToScheduleTypeResponseModel(scheduleType);
                    responses.Add(response);
                }

                return new BaseResponse<List<ScheduleTypeResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách loại lịch học thành công!",
                    Data = responses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllScheduleTypesAsync: {Message}", ex.Message);
                return new BaseResponse<List<ScheduleTypeResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ScheduleTypeResponseModel>> UpdateScheduleTypeAsync(int id, UpdateScheduleTypeRequestModel model)
        {
            try
            {
                var scheduleType = await _scheduleTypeRepository.GetByIdAsync(id);
                if (scheduleType == null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy loại lịch học!",
                        Data = null
                    };
                }

                // Kiểm tra Slot tồn tại
                var slot = await _slotRepository.GetByIdAsync(model.SlotId);
                if (slot == null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy ca học!",
                        Data = null
                    };
                }

                // Kiểm tra tên ScheduleType đã tồn tại chưa (trừ chính nó)
                var existingScheduleTypes = await _scheduleTypeRepository.GetAllAsync();
                var existingScheduleType = existingScheduleTypes.FirstOrDefault(st => 
                    st.ScheduleTypeId != id && 
                    st.ScheduleTypeName.ToLower() == model.ScheduleTypeName.ToLower() && st.ScheduleTypeDow.ToLower() ==model.ScheduleTypeDow.ToLower() && st.SlotId == model.SlotId);
                
                if (existingScheduleType != null)
                {
                    return new BaseResponse<ScheduleTypeResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Tên loại lịch học đã tồn tại!",
                        Data = null
                    };
                }

                scheduleType.SlotId = model.SlotId;
                scheduleType.ScheduleTypeName = model.ScheduleTypeName;
                scheduleType.ScheduleTypeDescription = model.ScheduleTypeDescription;
                scheduleType.ScheduleTypeDow = model.ScheduleTypeDow;
                scheduleType.ScheduleTypeStatus = model.ScheduleTypeStatus;

                var result = await _scheduleTypeRepository.UpdateAsync(scheduleType);
                var response = await MapToScheduleTypeResponseModel(result);

                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật loại lịch học thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateScheduleTypeAsync: {Message}", ex.Message);
                return new BaseResponse<ScheduleTypeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteScheduleTypeAsync(int id)
        {
            try
            {
                var scheduleType = await _scheduleTypeRepository.GetByIdAsync(id);
                if (scheduleType == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy loại lịch học!",
                        Data = false
                    };
                }

                // Kiểm tra có lớp nào đang sử dụng ScheduleType này không
                var classes = await _classRepository.GetAllAsync();
                var usingClasses = classes.Where(c => c.ScheduleTypeId == id).ToList();
                
                if (usingClasses.Any())
                {
                    var classNames = string.Join(", ", usingClasses.Select(c => c.ClassName));
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = $"Không thể xóa loại lịch học này vì đang được sử dụng bởi các lớp: {classNames}",
                        Data = false
                    };
                }

                var result = await _scheduleTypeRepository.DeleteAsync(id);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa loại lịch học thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteScheduleTypeAsync: {Message}", ex.Message);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<List<ScheduleTypeResponseModel>>> GetAvailableScheduleTypesAsync()
        {
            try
            {
                var scheduleTypes = await _scheduleTypeRepository.GetAllAsync();
                var availableScheduleTypes = scheduleTypes.Where(st => st.ScheduleTypeStatus == "Active").ToList();
                var listClass = await _classRepository.GetAllAsync();

                var result = availableScheduleTypes.Where(abs => !listClass.Any(lc => lc.ScheduleTypeId == abs.ScheduleTypeId)).ToList();
                var responses = _mapper.Map<List<ScheduleTypeResponseModel>>(result);

                return new BaseResponse<List<ScheduleTypeResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách loại lịch học khả dụng thành công!",
                    Data = responses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAvailableScheduleTypesAsync: {Message}", ex.Message);
                return new BaseResponse<List<ScheduleTypeResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server!",
                    Data = null
                };
            }
        }

        private async Task<ScheduleTypeResponseModel> MapToScheduleTypeResponseModel(ScheduleType scheduleType)
        {
            var slot = await _slotRepository.GetByIdAsync(scheduleType.SlotId);
            var classes = await _classRepository.GetAllAsync();
            var classCount = classes.Count(c => c.ScheduleTypeId == scheduleType.ScheduleTypeId);
            
            var schedules = await _scheduleRepository.GetAllAsync();
            var scheduleCount = schedules.Count(s => s.Class.ScheduleTypeId == scheduleType.ScheduleTypeId);

            return new ScheduleTypeResponseModel
            {
                ScheduleTypeId = scheduleType.ScheduleTypeId,
                SlotId = scheduleType.SlotId,
                ScheduleTypeName = scheduleType.ScheduleTypeName,
                ScheduleTypeDescription = scheduleType.ScheduleTypeDescription,
                ScheduleTypeDow = scheduleType.ScheduleTypeDow,
                ScheduleTypeStatus = scheduleType.ScheduleTypeStatus,
                SlotName = slot?.SlotName,
                SlotStartTime = slot?.SlotStartTime,
                SlotEndTime = slot?.SlotEndTime,
                SlotDescription = slot?.SlotDescription,
                ClassCount = classCount,
                ScheduleCount = scheduleCount
            };
        }
    }
} 