using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamKit
{
    public class UpdateTeamKitRequestModel
    {
        public int TeamId { get; set; }
        public string KitId { get; set; } = null!;
        public string TeamKitName { get; set; } = null!;
        public string? TeamKitDescription { get; set; }
        public DateTime TeamKitDateBorrow { get; set; }
        public DateTime? TeamKitDateGiveBack { get; set; }
        public string TeamKitStatus { get; set; } = null!;
    }
}
