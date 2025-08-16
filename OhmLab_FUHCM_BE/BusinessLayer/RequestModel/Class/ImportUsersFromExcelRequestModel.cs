using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class ImportUsersFromExcelRequestModel
    {
        [Required]
        public int ClassId { get; set; }
        
        [Required]
        public string FileName { get; set; } = null!;
        
        public string? Description { get; set; }
    }
    
    public class ExcelUserData
    {
        public string StudentId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}

