using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class ImportExcelRequestModel
    {
        [Required]
        public IFormFile ExcelFile { get; set; } = null!;
        
        [Required]
        public int ClassId { get; set; }
    }
}

