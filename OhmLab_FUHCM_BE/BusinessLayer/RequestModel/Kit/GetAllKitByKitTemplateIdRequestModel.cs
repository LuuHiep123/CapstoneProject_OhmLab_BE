using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Kit
{
    public class GetAllKitByKitTemplateIdRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 1;
        public string kitTemplateId { get; set; }
        public string? keyWord { get; set; }
        public string? status { get; set; }
    }
}
