using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.TeamKit
{
    public class TeamKitAllResponseModel
    {
        public int TeamKitId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string KitId { get; set; } = null!;
        public string KitName { get; set; } = null!;
        public string KitDesription { get; set; } = null!;
        public string KitImgUrl { get; set; } = null!;
        public string TeamKitName { get; set; } = null!;
        public string? TeamKitDescription { get; set; }
        public DateTime TeamKitDateBorrow { get; set; }
        public DateTime? TeamKitDateGiveBack { get; set; }
        public string TeamKitStatus { get; set; } = null!;
    }
}
