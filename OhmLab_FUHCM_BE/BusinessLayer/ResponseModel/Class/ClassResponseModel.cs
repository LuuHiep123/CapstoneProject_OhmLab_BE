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
        public List<ClassUserResponseModel>? ClassUsers { get; set; }
    }
} 