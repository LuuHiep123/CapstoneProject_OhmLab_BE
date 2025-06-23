using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.User
{
    public class LoginResponseModel
    {
        public string token { get; set; }
        public UserResponseModel user { get; set; }
    }
}
