using AutoMapper;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.ResponseModel.Subject;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.ResponseModel.Equipment;

namespace OhmLab_FUHCM_BE.AppStarts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<RegisterRequestModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<RegisterRequestModel, UserResponseModel>().ReverseMap();
            CreateMap<UpdateRequestModel, User>().ReverseMap();

            //Subject
            CreateMap<CreateSubjectRequestModel, Subject>()
                .ForMember(dest => dest.SubjectId, opt => opt.Ignore());
            CreateMap<UpdateSubjectRequestModel, Subject>();
            CreateMap<Subject, SubjectResponseModel>();

            //Lab
            CreateMap<CreateLabRequestModel, Lab>()
                .ForMember(dest => dest.LabId, opt => opt.Ignore());
            CreateMap<UpdateLabRequestModel, Lab>();
            CreateMap<Lab, LabResponseModel>();


            //Equipment
            CreateMap<CreateEquipmentRequestModel, Equipment>().ReverseMap();
            CreateMap<EquipmentResponseModel, Equipment>().ReverseMap();

        }
    }
}
