using AutoMapper;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
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
        private readonly IMapper _mapper;

        public LabService(ILabRepository labRepository, IMapper mapper)
        {
            _labRepository = labRepository;
            _mapper = mapper;
        }

        public async Task AddLab(CreateLabRequestModel labModel)
        {
            var lab = _mapper.Map<Lab>(labModel);
            lab.LabStatus = labModel.LabStatus; // Nhận trạng thái từ request
            await _labRepository.AddLab(lab);
            Console.WriteLine($"LabId before insert: {lab.LabId}");
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
    }
} 