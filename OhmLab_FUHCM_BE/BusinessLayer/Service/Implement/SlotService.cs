using AutoMapper;
using BusinessLayer.RequestModel.Slot;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Slot;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IMapper _mapper;

        public SlotService(ISlotRepository slotRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<SlotResponseModel>> CreateSlotAsync(CreateSlotRequestModel model)
        {
            try
            {
                var slot = new Slot
                {
                    SlotName = model.SlotName,
                    SlotStartTime = model.SlotStartTime,
                    SlotEndTime = model.SlotEndTime,
                    SlotDescription = model.SlotDescription,
                    SlotStatus = model.SlotStatus
                };

                var result = await _slotRepository.CreateAsync(slot);
                var response = _mapper.Map<SlotResponseModel>(result);

                return new BaseResponse<SlotResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo ca học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<SlotResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<SlotResponseModel>> GetSlotByIdAsync(int id)
        {
            try
            {
                var slot = await _slotRepository.GetByIdAsync(id);
                if (slot == null)
                {
                    return new BaseResponse<SlotResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy ca học!",
                        Data = null
                    };
                }

                var response = _mapper.Map<SlotResponseModel>(slot);

                return new BaseResponse<SlotResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin ca học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<SlotResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<SlotResponseModel>>> GetAllSlotsAsync()
        {
            try
            {
                var slots = await _slotRepository.GetAllAsync();
                var response = slots.Select(s => _mapper.Map<SlotResponseModel>(s)).ToList();

                return new BaseResponse<List<SlotResponseModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách ca học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<List<SlotResponseModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<SlotResponseModel>> UpdateSlotAsync(int id, CreateSlotRequestModel model)
        {
            try
            {
                var slot = await _slotRepository.GetByIdAsync(id);
                if (slot == null)
                {
                    return new BaseResponse<SlotResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy ca học!",
                        Data = null
                    };
                }

                slot.SlotName = model.SlotName;
                slot.SlotStartTime = model.SlotStartTime;
                slot.SlotEndTime = model.SlotEndTime;
                slot.SlotDescription = model.SlotDescription;
                slot.SlotStatus = model.SlotStatus;

                var result = await _slotRepository.UpdateAsync(slot);
                var response = _mapper.Map<SlotResponseModel>(result);

                return new BaseResponse<SlotResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật ca học thành công!",
                    Data = response
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResponse<SlotResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteSlotAsync(int id)
        {
            try
            {
                var result = await _slotRepository.DeleteAsync(id);
                if (!result)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy ca học!",
                        Data = false
                    };
                }

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa ca học thành công!",
                    Data = true
                };
            }
            catch (System.Exception ex)
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