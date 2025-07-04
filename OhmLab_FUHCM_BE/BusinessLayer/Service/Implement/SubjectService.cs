using AutoMapper;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Subject;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task AddSubject(CreateSubjectRequestModel subjectModel)
        {
            var subject = _mapper.Map<Subject>(subjectModel);
            subject.SubjectStatus = "Active"; // Default status
            await _subjectRepository.AddSubject(subject);
        }

        public async Task DeleteSubject(int id)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject != null)
            {
                subject.SubjectStatus = "Inactive";
                await _subjectRepository.UpdateSubject(subject);
            }
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjects();
            var result = _mapper.Map<List<SubjectResponseModel>>(subjects);
            int pageNum = 1, pageSize = 10;
            var pagedSubjects = result.OrderBy(s => s.SubjectId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = result.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>
            {
                Code = 200,
                Success = true,
                Message = null,
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<SubjectResponseModel>
                {
                    PageData = pagedSubjects,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = pageNum,
                        Size = pageSize,
                        TotalItem = totalItems,
                        TotalPage = totalPages
                    },
                    SearchInfo = null
                }
            };
        }

        public async Task<SubjectResponseModel> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            return _mapper.Map<SubjectResponseModel>(subject);
        }

        public async Task UpdateSubject(int id, UpdateSubjectRequestModel subjectModel)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject != null)
            {
                _mapper.Map(subjectModel, subject);
                subject.SubjectId = id;
                await _subjectRepository.UpdateSubject(subject);
            }
        }
    }
} 