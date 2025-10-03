using AutoMapper;
using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.EquipmentType;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.KitAccessory;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.RegistrationSchedule;
using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.RequestModel.Slot;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.RequestModel.Team;
using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.RequestModel.TeamKit;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.Accessory;
using BusinessLayer.ResponseModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.EquipmentType;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.KitAccessory;
using BusinessLayer.ResponseModel.KitTemplate;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.ResponseModel.RegistrationSchedule;
using BusinessLayer.ResponseModel.Schedule;
using BusinessLayer.ResponseModel.ScheduleType;
using BusinessLayer.ResponseModel.Slot;
using BusinessLayer.ResponseModel.Subject;
using BusinessLayer.ResponseModel.Team;
using BusinessLayer.ResponseModel.TeamEquipment;
using BusinessLayer.ResponseModel.TeamKit;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using System;
using System.Linq;

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
            CreateMap<UpdateEquipmentRequestModel, Equipment>().ReverseMap();
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
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Team.Class.ClassName))
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Team.Class.ClassId))
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment.EquipmentName))
                .ForMember(dest => dest.EquipmentCode, opt => opt.MapFrom(src => src.Equipment.EquipmentCode))
                .ForMember(dest => dest.EquipmentNumberSerial, opt => opt.MapFrom(src => src.Equipment.EquipmentNumberSerial))
                .ReverseMap();
            CreateMap <GetAllTeamEquipmentRequestModel, TeamEquipmentAllResponseModel>().ReverseMap();
            CreateMap <CreateTeamEquipmentRequestModel, TeamEquipment>().ReverseMap();
            CreateMap <UpdateTeamEquipmentRequestModel, TeamEquipment>().ReverseMap();

            //TeamKit
            CreateMap<GetAllTeamKitRequestModel, TeamEquipment>().ReverseMap();
            CreateMap<TeamKit, TeamKitAllResponseModel>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Team.Class.ClassName))
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Team.Class.ClassId))
                .ForMember(dest => dest.KitName, opt => opt.MapFrom(src => src.Kit.KitName))
                .ForMember(dest => dest.KitDesription, opt => opt.MapFrom(src => src.Kit.KitDescription))
                .ForMember(dest => dest.KitImgUrl, opt => opt.MapFrom(src => src.Kit.KitUrlImg))
                .ReverseMap();
            CreateMap<GetAllTeamKitRequestModel, TeamKitAllResponseModel>().ReverseMap();
            CreateMap<CreateTeamKitRequestModel, TeamKit>().ReverseMap();
            CreateMap<UpdateTeamKitRequestModel, TeamKit>().ReverseMap();


            //KitTemplate
            CreateMap<CreateKitTemplateRequestModel, KitTemplate>().ReverseMap();
            CreateMap<CreateKitTemplateRequestModel, KitTemplateResponseModel>().ReverseMap();
            CreateMap<KitTemplateResponseModel, KitTemplate>().ReverseMap();
            CreateMap<UpdateKitTemplateRequestModel, KitTemplate>().ReverseMap();

            //Kit
            CreateMap<CreateKitRequestModel, Kit>().ReverseMap();
            CreateMap<UpdateKitRequestModel, Kit>().ReverseMap();
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
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Lecturer != null ? src.Lecturer.UserFullName : null))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => 
                    src.Subject != null && 
                    src.Subject.SemesterSubjects != null && 
                    src.Subject.SemesterSubjects.Any() && 
                    src.Subject.SemesterSubjects.First().Semester != null ? 
                    src.Subject.SemesterSubjects.First().Semester.SemesterName : null))
                .ForMember(dest => dest.SemesterStartDate, opt => opt.MapFrom(src => 
                    src.Subject != null && 
                    src.Subject.SemesterSubjects != null && 
                    src.Subject.SemesterSubjects.Any() && 
                    src.Subject.SemesterSubjects.First().Semester != null ? 
                    src.Subject.SemesterSubjects.First().Semester.SemesterStartDate : (DateTime?)null))
                .ForMember(dest => dest.SemesterEndDate, opt => opt.MapFrom(src => 
                    src.Subject != null && 
                    src.Subject.SemesterSubjects != null && 
                    src.Subject.SemesterSubjects.Any() && 
                    src.Subject.SemesterSubjects.First().Semester != null ? 
                    src.Subject.SemesterSubjects.First().Semester.SemesterEndDate : (DateTime?)null))
                .ForMember(dest => dest.ScheduleTypeName, opt => opt.MapFrom(src => src.ScheduleType != null ? src.ScheduleType.ScheduleTypeName : null))
                .ForMember(dest => dest.ScheduleTypeDow, opt => opt.MapFrom(src => src.ScheduleType != null ? src.ScheduleType.ScheduleTypeDow : null))
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.ScheduleType != null && src.ScheduleType.Slot != null ? src.ScheduleType.Slot.SlotName : null))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.ScheduleType != null && src.ScheduleType.Slot != null ? src.ScheduleType.Slot.SlotStartTime : null))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.ScheduleType != null && src.ScheduleType.Slot != null ? src.ScheduleType.Slot.SlotEndTime : null))
                .ForMember(dest => dest.ClassUsers, opt => opt.MapFrom(src => src.ClassUsers));

            //Lab
            CreateMap<Lab, LabResponseModel>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.SubjectCode,opt => opt.MapFrom(src => src.Subject != null ? src.Subject.SubjectCode : null))
                .ForMember(dest => dest.RequiredEquipments, opt => opt.MapFrom(src => src.LabEquipmentTypes))
                .ForMember(dest => dest.RequiredKits, opt => opt.MapFrom(src => src.LabKitTemplates));

            //Lab Equipment
            CreateMap<LabEquipmentType, LabEquipmentResponseModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LabEquipmentTypeStatus))
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType != null ? src.EquipmentType.EquipmentTypeName : null))
                .ForMember(dest => dest.EquipmentTypeCode, opt => opt.MapFrom(src => src.EquipmentType != null ? src.EquipmentType.EquipmentTypeCode : null))
                .ForMember(dest => dest.EquipmentTypeDescription, opt => opt.MapFrom(src => src.EquipmentType != null ? src.EquipmentType.EquipmentTypeDescription : null))
                .ForMember(dest => dest.EquipmentTypeQuantity, opt => opt.MapFrom(src => src.EquipmentType != null ? src.EquipmentType.EquipmentTypeQuantity : 0))
                .ForMember(dest => dest.EquipmentTypeUrlImg, opt => opt.MapFrom(src => src.EquipmentType != null ? src.EquipmentType.EquipmentTypeUrlImg : null));

            //Lab Kit
            CreateMap<LabKitTemplate, LabKitResponseModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LabKitTemplateStatus))
                .ForMember(dest => dest.KitTemplateName, opt => opt.MapFrom(src => src.KitTemplate != null ? src.KitTemplate.KitTemplateName : null))
                .ForMember(dest => dest.KitTemplateQuantity, opt => opt.MapFrom(src => src.KitTemplate != null ? src.KitTemplate.KitTemplateQuantity : 0))
                .ForMember(dest => dest.KitTemplateDescription, opt => opt.MapFrom(src => src.KitTemplate != null ? src.KitTemplate.KitTemplateDescription : null))
                .ForMember(dest => dest.KitTemplateUrlImg, opt => opt.MapFrom(src => src.KitTemplate != null ? src.KitTemplate.KitTemplateUrlImg : null))
                .ForMember(dest => dest.KitTemplateStatus, opt => opt.MapFrom(src => src.KitTemplate != null ? src.KitTemplate.KitTemplateStatus : null));
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
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.User != null ? src.User.UserRoleName : null))
                .ForMember(dest => dest.UserNumberCode, opt => opt.MapFrom(src => src.User != null ? src.User.UserNumberCode : null))
                .ForMember(dest => dest.ClassUserStatus, opt => opt.MapFrom(src => src.ClassUserStatus))
                // Thêm mapping cho thông tin môn học
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectId : (int?)null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectName : null))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectCode : null))
                .ForMember(dest => dest.SubjectDescription, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectDescription : null))
                .ForMember(dest => dest.SubjectStatus, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectStatus : null))
                // Thêm mapping cho thông tin kỳ học - lấy semester đầu tiên có status "Active"
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => 
                    src.Class != null && 
                    src.Class.Subject != null && 
                    src.Class.Subject.SemesterSubjects != null && 
                    src.Class.Subject.SemesterSubjects.Any() ? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null && ss.Semester.SemesterStatus.ToLower() == "active")
                        .Select(ss => ss.Semester.SemesterName)
                        .FirstOrDefault() ?? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null)
                        .Select(ss => ss.Semester.SemesterName)
                        .FirstOrDefault() : null))
                .ForMember(dest => dest.SemesterStartDate, opt => opt.MapFrom(src => 
                    src.Class != null && 
                    src.Class.Subject != null && 
                    src.Class.Subject.SemesterSubjects != null && 
                    src.Class.Subject.SemesterSubjects.Any() ? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null && ss.Semester.SemesterStatus.ToLower() == "active")
                        .Select(ss => (DateTime?)ss.Semester.SemesterStartDate)
                        .FirstOrDefault() ?? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null)
                        .Select(ss => (DateTime?)ss.Semester.SemesterStartDate)
                        .FirstOrDefault() : null))
                .ForMember(dest => dest.SemesterEndDate, opt => opt.MapFrom(src => 
                    src.Class != null && 
                    src.Class.Subject != null && 
                    src.Class.Subject.SemesterSubjects != null && 
                    src.Class.Subject.SemesterSubjects.Any() ? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null && ss.Semester.SemesterStatus.ToLower() == "active")
                        .Select(ss => (DateTime?)ss.Semester.SemesterEndDate)
                        .FirstOrDefault() ?? 
                    src.Class.Subject.SemesterSubjects
                        .Where(ss => ss.Semester != null)
                        .Select(ss => (DateTime?)ss.Semester.SemesterEndDate)
                        .FirstOrDefault() : null));
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
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectName : null))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Class != null && src.Class.Subject != null ? src.Class.Subject.SubjectId : 0))
                .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => src.Class != null && src.Class.Lecturer != null ? src.Class.Lecturer.UserFullName : null))
                .ForMember(dest => dest.LecturerId, opt => opt.MapFrom(src => src.Class != null && src.Class.Lecturer != null ? src.Class.Lecturer.UserId : Guid.Empty))
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotName : null))
                .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotId : 0))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotStartTime : null))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.Class != null && src.Class.ScheduleType != null && src.Class.ScheduleType.Slot != null ? src.Class.ScheduleType.Slot.SlotEndTime : null));

            //Team
            CreateMap<Team, TeamResponseModel>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassName : null))
                .ForMember(dest => dest.TeamUsers, opt => opt.MapFrom(src => src.TeamUsers));

            //TeamUser
            CreateMap<TeamUser, TeamUserResponseModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserFullName : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.UserEmail : null))
                .ForMember(dest => dest.UserNumberCode, opt => opt.MapFrom(src => src.User != null ? src.User.UserNumberCode : null));

            //Team Request Models
            CreateMap<CreateTeamRequestModel, Team>()
                .ForMember(dest => dest.TeamId, opt => opt.Ignore());
            CreateMap<UpdateTeamRequestModel, Team>()
                .ForMember(dest => dest.TeamId, opt => opt.Ignore());

            //Registration
            CreateMap<GetAllRegistrationScheduleRequestModel, RegistrationSchedule>().ReverseMap();
            CreateMap<RegistrationSchedule, RegistrationScheduleAllResponseModel>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.User.UserFullName))
                .ForMember(dest => dest.TeacherRollNumber, opt => opt.MapFrom(src => src.User.UserRollNumber))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.ClassName))
                .ForMember(dest => dest.LabName, opt => opt.MapFrom(src => src.Lab.LabName))
                .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot.SlotName))
                .ForMember(dest => dest.SlotStartTime, opt => opt.MapFrom(src => src.Slot.SlotStartTime))
                .ForMember(dest => dest.SlotEndTime, opt => opt.MapFrom(src => src.Slot.SlotEndTime))
                .ReverseMap();
            CreateMap<GetAllRegistrationScheduleRequestModel, RegistrationScheduleAllResponseModel>().ReverseMap();
            CreateMap<CreateRegistrationScheduleRequestModel, RegistrationSchedule>().ReverseMap();
            CreateMap<UpdateRegistrationScheduleRequestModel, RegistrationSchedule>().ReverseMap();


            //Accessory
            CreateMap<CreateAccessoryRequestModel, Accessory>().ReverseMap();
            CreateMap<UpdateAccessoryRequestModel, Accessory>().ReverseMap();
            CreateMap<AccessoryResponseModel, Accessory>().ReverseMap();

            //AccessoryKitTemplate
            CreateMap<CreateAccessoryKitTemplateRequestModel, AccessoryKitTemplate>().ReverseMap();
            CreateMap<UpdateAccessoryKitTemplateRequestModel, AccessoryKitTemplate>().ReverseMap();
            CreateMap<AccessoryKitTemplate, AccessoryKitTemplateResponseModel>()
                .ForMember(dest => dest.KitTemplateName, opt => opt.MapFrom(src => src.KitTemplate.KitTemplateName))
                .ForMember(dest => dest.AccessoryName, opt => opt.MapFrom(src => src.Accessory.AccessoryName))
                .ForMember(dest => dest.AccessoryValueCode, opt => opt.MapFrom(src => src.Accessory.AccessoryValueCode))
                .ReverseMap();

            //KitAccessory
            CreateMap<CreateKitAccessoryRequestModel, KitAccessory>().ReverseMap();
            CreateMap<UpdateKitAccessoryRequestModel, KitAccessory>().ReverseMap();
            CreateMap<KitAccessory, KitAccessoryResponseModel>()
                .ForMember(dest => dest.KitName, opt => opt.MapFrom(src => src.Kit.KitName))
                .ForMember(dest => dest.AccessoryName, opt => opt.MapFrom(src => src.Accessory.AccessoryName))
                .ForMember(dest => dest.AccessoryValueCode, opt => opt.MapFrom(src => src.Accessory.AccessoryValueCode))
                .ReverseMap();


        }
    }
}
