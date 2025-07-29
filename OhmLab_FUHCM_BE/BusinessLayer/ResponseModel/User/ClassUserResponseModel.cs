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
    }
} 