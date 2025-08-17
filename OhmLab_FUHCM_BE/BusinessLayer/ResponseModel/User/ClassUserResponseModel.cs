using System;

namespace BusinessLayer.ResponseModel.User
{
    public class ClassUserResponseModel
    {
        public int ClassUserId { get; set; }
        public int ClassId { get; set; }
        public Guid UserId { get; set; }
        public string? ClassName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }
        
        // Thêm thông tin môn học
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectDescription { get; set; }
        public string? SubjectStatus { get; set; }
        
        // Thêm thông tin kỳ học
        public string? SemesterName { get; set; }
        public DateTime? SemesterStartDate { get; set; }
        public DateTime? SemesterEndDate { get; set; }
        
        // Thêm trạng thái ClassUser
        public string? ClassUserStatus { get; set; }
    }
} 