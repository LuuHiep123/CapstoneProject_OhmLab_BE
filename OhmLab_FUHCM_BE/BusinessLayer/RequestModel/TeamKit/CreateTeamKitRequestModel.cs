using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamKit
{
    public class CreateTeamKitRequestModel
    {
        public int TeamId { get; set; }
        public string KitId { get; set; } = null!;
        public string TeamKitName { get; set; } = null!;
        public string? TeamKitDescription { get; set; }
    }
}
