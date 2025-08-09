using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class GetAllClassRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? keyWord { get; set; }
        public string? status { get; set; }
        public int? subjectId { get; set; }
        public Guid? lecturerId { get; set; }
    }
}
