using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamUser
{
    public class CreateTeamUserRequestModel
    {
        public int TeamId { get; set; } 
        public Guid UserId { get; set; }
    }
}
