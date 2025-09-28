using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamEquipment
{
    public class GetAllTeamEquipmentByLecturerIdRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 1;
        public Guid LecturerId { get; set; }
        public string? keyWord { get; set; }
        public string? status { get; set; }
    }
}
