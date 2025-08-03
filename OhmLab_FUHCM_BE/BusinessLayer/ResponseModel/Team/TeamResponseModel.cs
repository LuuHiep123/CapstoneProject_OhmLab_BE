using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Team
{
    public class TeamResponseModel
    {
        public int TeamId { get; set; }
        public int ClassId { get; set; }
        public string TeamName { get; set; }
        public string? TeamDescription { get; set; }
        public string TeamStatus { get; set; }
        public string? ClassName { get; set; }
        public List<TeamUserResponseModel>? TeamUsers { get; set; }
    }

    public class TeamUserResponseModel
    {
        public int TeamUserId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string TeamUserStatus { get; set; }
    }
} 