using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class GetAllClassRequestModel
    {
        public string? Status { get; set; } // "Active", "Inactive", "Deleted", hoặc null để lấy tất cả
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SearchTerm { get; set; }
    }
}

