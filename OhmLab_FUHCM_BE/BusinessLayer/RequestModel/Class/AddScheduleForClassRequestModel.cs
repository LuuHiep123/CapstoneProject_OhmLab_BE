using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Class
{
    public class AddScheduleForClassRequestModel
    {
        [Required]
        public int ClassId { get; set; }


        [Required]
        public int ScheduleTypeId { get; set; }
    }
}
