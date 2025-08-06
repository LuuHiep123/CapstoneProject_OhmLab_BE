using System;

namespace BusinessLayer.RequestModel.Report
{
    public class CreateReportRequestModel
    {
        public string ReportTitle { get; set; } = null!;
        public string? ReportDescription { get; set; }
        public string SelectedSlot { get; set; } = null!;
        public string SelectedClass { get; set; } = null!;
    }

    public class UpdateReportStatusRequestModel
    {
        public string ReportStatus { get; set; } = null!;
        public string? ResolutionNotes { get; set; }
    }
} 