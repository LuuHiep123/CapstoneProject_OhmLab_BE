using BusinessLayer.RequestModel.Slot;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Slot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ISlotService
    {
        Task<BaseResponse<SlotResponseModel>> CreateSlotAsync(CreateSlotRequestModel model);
        Task<BaseResponse<SlotResponseModel>> GetSlotByIdAsync(int id);
        Task<BaseResponse<List<SlotResponseModel>>> GetAllSlotsAsync();
        Task<BaseResponse<SlotResponseModel>> UpdateSlotAsync(int id, CreateSlotRequestModel model);
        Task<BaseResponse<bool>> DeleteSlotAsync(int id);
    }
} 