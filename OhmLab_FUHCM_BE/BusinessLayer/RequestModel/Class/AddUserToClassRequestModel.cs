using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class AddUserToClassRequestModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int ClassId { get; set; }
    }
} 