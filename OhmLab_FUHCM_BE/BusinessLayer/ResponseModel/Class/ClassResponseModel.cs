using System;
using System.Collections.Generic;
using BusinessLayer.ResponseModel.User;

namespace BusinessLayer.ResponseModel.Class
{
    public class ClassResponseModel
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public Guid? LecturerId { get; set; }
        public int? ScheduleTypeId { get; set; }
        public string ClassName { get; set; }
        public string? ClassDescription { get; set; }
        public string ClassStatus { get; set; }
        public string? SubjectName { get; set; }
        public string? LecturerName { get; set; }
        
        // Thêm các trường mới
        public string? SemesterName { get; set; }
        public DateTime? SemesterStartDate { get; set; }
        public DateTime? SemesterEndDate { get; set; }
        public string? ScheduleTypeName { get; set; }
        public string? ScheduleTypeDow { get; set; }
        public string? SlotName { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        
        public List<ClassUserResponseModel>? ClassUsers { get; set; }
    }
} 