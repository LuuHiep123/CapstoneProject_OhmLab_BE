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
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IClassRepository classRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
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
                // Kiểm tra Subject có đang được sử dụng bởi Class nào không
                var classes = await _classRepository.GetAllAsync();
                var usingClasses = classes.Where(c => c.SubjectId == id).ToList();
                
                if (usingClasses.Any())
                {
                    // Nếu có Class đang sử dụng, không cho phép xóa
                    throw new System.InvalidOperationException($"Không thể xóa môn học này vì đang được sử dụng bởi {usingClasses.Count} lớp học!");
                }

                subject.SubjectStatus = "Inactive";
                await _subjectRepository.UpdateSubject(subject);
            }
        }

        public async Task<SubjectResponseModel> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetSubjectById(id);
            if (subject == null)
                return null;

            return new SubjectResponseModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                SubjectCode = subject.SubjectCode,
                SubjectDescription = subject.SubjectDescription,
                SubjectStatus = subject.SubjectStatus
            };
        }

        public async Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjects();
            var subjectResponses = subjects.Select(s => new SubjectResponseModel
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                SubjectCode = s.SubjectCode,
                SubjectDescription = s.SubjectDescription,
                SubjectStatus = s.SubjectStatus
            }).ToList();

            return new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<SubjectResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách môn học thành công!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<SubjectResponseModel>
                {
                    PageData = subjectResponses,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = subjectResponses.Count,
                        TotalItem = subjectResponses.Count,
                        TotalPage = 1
                    },
                    SearchInfo = null
                }
            };
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