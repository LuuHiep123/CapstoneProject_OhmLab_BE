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
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.ResponseModel.KitTemplate;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.RequestModel.EquipmentType;
using BusinessLayer.ResponseModel.EquipmentType;
using BusinessLayer.RequestModel.Slot;
using BusinessLayer.ResponseModel.Slot;
using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.ResponseModel.ScheduleType;
using BusinessLayer.ResponseModel.Schedule;

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
            CreateMap<Equipment, EquipmentResponseModel>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.EquipmentTypeName))
                .ReverseMap();

            // Assignment (Lịch thực hành, Báo cáo, Điểm)
            CreateMap<Schedule, ScheduleResponseModel>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectName : null))
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Class != null && src.Class.Lecturer != null ? src.Class.Lecturer.UserFullName : null))
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotName : null))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotStartTime : null))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotEndTime : null))
                .ForMember(dest => dest.ScheduleTypeName, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null ? src.Class.ScheduleType.ScheduleTypeName : null))
                .ForMember(dest => dest.ScheduleTypeDow, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null ? src.Class.ScheduleType.ScheduleTypeDow : null));

            //Report
            CreateMap<Report, BusinessLayer.ResponseModel.Report.ReportResponseModel>();
            CreateMap<Report, BusinessLayer.ResponseModel.Report.ReportDetailResponseModel>()
                .ForMember(dest => dest.ClassName, opt => opt.Ignore())
                .ForMember(dest => dest.SubjectName, opt => opt.Ignore())
                .ForMember(dest => dest.LecturerName, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduleDate, opt => opt.Ignore())
                .ForMember(dest => dest.SlotName, opt => opt.Ignore())
                .ForMember(dest => dest.SlotStartTime, opt => opt.Ignore())
                .ForMember(dest => dest.SlotEndTime, opt => opt.Ignore());

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


            //KitTemplate
            CreateMap<CreateKitTemplateRequestModel, KitTemplate>().ReverseMap();
            CreateMap<CreateKitTemplateRequestModel, KitTemplateResponseModel>().ReverseMap();
            CreateMap<KitTemplateResponseModel, KitTemplate>().ReverseMap();
            CreateMap<UpdateKitTemplateRequestModel, KitTemplate>().ReverseMap();

            //Kit
            CreateMap<CreateKitRequestModel, Kit>().ReverseMap();
            CreateMap<CreateKitRequestModel, KitResponseModel>().ReverseMap();
            CreateMap<Kit, KitResponseModel>()
                 .ForMember(dest => dest.KitTemplateName, opt => opt.MapFrom(src => src.KitTemplate.KitTemplateName))
                 .ReverseMap();         

            //EquipmentType
            CreateMap<CreateEquipmentTypeRequestModel, EquipmentType>().ReverseMap();
            CreateMap<CreateEquipmentTypeRequestModel, EquipmentTypeResponseModel>().ReverseMap();
            CreateMap<UpdateEquipmentTypeRequestModel, EquipmentType>().ReverseMap();
            CreateMap<EquipmentType, EquipmentTypeResponseModel>().ReverseMap();

            //Class
            CreateMap<Class, ClassResponseModel>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Lecturer != null ? src.Lecturer.UserFullName : null));
            CreateMap<ClassResponseModel, Class>();
            CreateMap<CreateClassRequestModel, Class>()
                .ForMember(dest => dest.ClassId, opt => opt.Ignore());
            //classuser
            // ClassUser -> ClassUserResponseModel
            CreateMap<ClassUser, ClassUserResponseModel>()
                .ForMember(dest => dest.ClassUserId, opt => opt.MapFrom(src => src.ClassUserId))
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserFullName : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.UserEmail : null))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.User != null ? src.User.UserRoleName : null));
            CreateMap<CreateSlotRequestModel, Slot>()
               .ForMember(dest => dest.SlotId, opt => opt.Ignore());
            CreateMap<Slot, SlotResponseModel>().ReverseMap();
            CreateMap<CreateSlotRequestModel, SlotResponseModel>();
            
            //ScheduleType
            CreateMap<CreateScheduleTypeRequestModel, ScheduleType>()
               .ForMember(dest => dest.ScheduleTypeId, opt => opt.Ignore());
            CreateMap<UpdateScheduleTypeRequestModel, ScheduleType>();
            CreateMap<ScheduleType, ScheduleTypeResponseModel>()
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot != null ? src.Slot.SlotName : null))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.Slot != null ? src.Slot.SlotStartTime : null))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.Slot != null ? src.Slot.SlotEndTime : null))
                .ForMember(dest => dest.SlotDescription, opt => opt.MapFrom(src => src.Slot != null ? src.Slot.SlotDescription : null));

            //Schedule
            CreateMap<Schedule, ScheduleResponseAllModel>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.ClassName))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.ClassName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Class.Subject.SubjectName))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Class.Subject.SubjectId))
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Class.Lecturer.UserFullName))
                .ForMember(dest => dest.LecturerId, opt => opt.MapFrom(src => src.Class.Lecturer.UserId))
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Class.ScheduleType.Slot.SlotName))
                .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.Class.ScheduleType.Slot.SlotId))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.Class.ScheduleType.Slot.SlotStartTime))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.Class.ScheduleType.Slot.SlotEndTime));




        }
    }
}
