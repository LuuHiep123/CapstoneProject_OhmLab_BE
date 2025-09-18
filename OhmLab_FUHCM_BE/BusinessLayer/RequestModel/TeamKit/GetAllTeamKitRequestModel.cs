using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamKit
{
    public class GetAllTeamKitRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 1;
        public string? keyWord { get; set; }
    }
}
