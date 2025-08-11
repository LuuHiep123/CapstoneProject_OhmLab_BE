using System;

namespace BusinessLayer.RequestModel.Semester
{
    public class GetAllSemesterRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? keyWord { get; set; }
        public string? status { get; set; }
    }
} 