using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.User
{
    public class UserResponseModel
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string UserRollNumber { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserNumberCode { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
