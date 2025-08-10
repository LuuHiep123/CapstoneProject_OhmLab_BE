using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Slot
{
    public class GetAllSlotRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? keyWord { get; set; }
        public string? status { get; set; }
    }
}

