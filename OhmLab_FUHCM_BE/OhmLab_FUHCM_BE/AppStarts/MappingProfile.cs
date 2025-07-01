using AutoMapper;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Subject;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;

namespace OhmLab_FUHCM_BE.AppStarts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterRequestModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<RegisterRequestModel, UserResponseModel>().ReverseMap();
            CreateMap<UpdateRequestModel, User>().ReverseMap();

            CreateMap<CreateSubjectRequestModel, Subject>();
            CreateMap<UpdateSubjectRequestModel, Subject>();
            CreateMap<Subject, SubjectResponseModel>();

            CreateMap<CreateLabRequestModel, Lab>();
            CreateMap<UpdateLabRequestModel, Lab>();
            CreateMap<Lab, LabResponseModel>();
        }
    }
}
