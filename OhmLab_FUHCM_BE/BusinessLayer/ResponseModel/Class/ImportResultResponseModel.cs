using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Class
{
    public class ImportResultResponseModel
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<ImportSuccessItem> SuccessItems { get; set; } = new List<ImportSuccessItem>();
        public List<ImportErrorItem> ErrorItems { get; set; } = new List<ImportErrorItem>();
    }
    
    public class ImportSuccessItem
    {
        public int RowNumber { get; set; }
        public string UserNumberCode { get; set; } = null!; // MSSV
        public string FullName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
    
    public class ImportErrorItem
    {
        public int RowNumber { get; set; }
        public string UserNumberCode { get; set; } = null!; // MSSV
        public string FullName { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;
    }
}

