using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Subject
{
    public class GetAllSubjectRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
} 