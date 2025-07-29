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
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.ResponseModel.TeamEquipment;
using BusinessLayer.RequestModel.Team;
using BusinessLayer.ResponseModel.Team;
using BusinessLayer.RequestModel.Slot;
using BusinessLayer.ResponseModel.Slot;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.Class;

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

            // Assignment (Lịch thực hành, Báo cáo, Điểm)
            CreateMap<Schedule, ScheduleResponseModel>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null));
                //.ForMember(dest => dest.WeeksName, opt => opt.MapFrom(src => src.Weeks != null ? src.Weeks.WeeksName : null));

            CreateMap<Report, ReportResponseModel>();

            CreateMap<Grade, GradeResponseModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade1));

            //TeamEquipment
            CreateMap<GetAllTeamEquipmentRequestModel, TeamEquipment>().ReverseMap();
            CreateMap<TeamEquipment, TeamEquipmentAllResponseModel>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment.EquipmentName))
                .ForMember(dest => dest.EquipmentCode, opt => opt.MapFrom(src => src.Equipment.EquipmentCode))
                .ForMember(dest => dest.EquipmentNumberSerial, opt => opt.MapFrom(src => src.Equipment.EquipmentNumberSerial))
                .ReverseMap();
            CreateMap <GetAllTeamEquipmentRequestModel, TeamEquipmentAllResponseModel>().ReverseMap();
            CreateMap <CreateTeamEquipmentRequestModel, TeamEquipment>().ReverseMap();

            //Team
            CreateMap<CreateTeamRequestModel, Team>()
                .ForMember(dest => dest.TeamId, opt => opt.Ignore());
            CreateMap<UpdateTeamRequestModel, Team>();
            CreateMap<Team, TeamResponseModel>();

            //Slot
            CreateMap<CreateSlotRequestModel, Slot>()
                .ForMember(dest => dest.SlotId, opt => opt.Ignore());
            CreateMap<Slot, SlotResponseModel>();

            //Class
            CreateMap<CreateClassRequestModel, Class>()
                .ForMember(dest => dest.ClassId, opt => opt.Ignore());
            CreateMap<Class, ClassResponseModel>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Lecturer != null ? src.Lecturer.UserFullName : null));

            //ClassUser
            CreateMap<ClassUser, ClassUserResponseModel>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserFullName : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.UserEmail : null))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.User != null ? src.User.UserRoleName : null));

            //TeamUser
            CreateMap<TeamUser, TeamUserResponseModel>();
        }
    }
}
