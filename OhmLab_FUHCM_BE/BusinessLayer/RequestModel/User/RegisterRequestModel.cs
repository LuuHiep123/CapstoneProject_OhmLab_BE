using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.User
{
    public class RegisterRequestModel
    {
        public string UserFullName { get; set; } = null!;
        public string UserRollNumber { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserNumberCode { get; set; } = null!;
        public string User_RoleName { get; set; } = null!;
    }
}
