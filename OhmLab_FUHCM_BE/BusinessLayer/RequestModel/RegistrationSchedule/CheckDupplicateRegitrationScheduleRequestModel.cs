using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.RegistrationSchedule
{
    public class CheckDupplicateRegitrationScheduleRequestModel
    {
        public DateTime RegistrationScheduleDate { get; set; }
        public int SlotId { get; set; }
    }
}
