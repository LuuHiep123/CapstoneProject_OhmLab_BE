using System;
using System.ComponentModel.DataAnnotations;
namespace BusinessLayer.RequestModel.HeadOfDepartment
{
    public class MonitorLabRequestModel
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? SubjectId { get; set; }

        public int? ClassId { get; set; }

        public Guid? LecturerId { get; set; }
    }
}
