using AutoMapper;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            lab.LabStatus = "Active"; // Default status
            await _labRepository.AddLab(lab);
            Console.WriteLine($"LabId before insert: {lab.LabId}");
        }

        public async Task DeleteLab(int id)
        {
            var lab = await _labRepository.GetLabById(id);
            if (lab != null)
            {
                lab.LabStatus = "Inactive";
                await _labRepository.UpdateLab(lab);
            }
        }

        public async Task<LabResponseModel> GetLabById(int id)
        {
            var lab = await _labRepository.GetLabById(id);
            return _mapper.Map<LabResponseModel>(lab);
        }

        public async Task<List<LabResponseModel>> GetLabsBySubjectId(int subjectId)
        {
            var labs = await _labRepository.GetLabsBySubjectId(subjectId);
            return _mapper.Map<List<LabResponseModel>>(labs);
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

        public async Task<List<LabResponseModel>> GetAllLabs()
        {
            var labs = await _labRepository.GetAllLabs();
            return _mapper.Map<List<LabResponseModel>>(labs);
        }
    }
} 