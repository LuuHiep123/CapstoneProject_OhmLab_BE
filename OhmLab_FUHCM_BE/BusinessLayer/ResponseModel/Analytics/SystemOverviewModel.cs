using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Analytics
{
    /// <summary>
    /// Model thống kê tổng quan hệ thống - Tổng số User, Equipment và Report
    /// </summary>
    public class SystemOverviewModel
    {
        /// <summary>
        /// Tổng số người dùng trong hệ thống
        /// </summary>
        public int TongSoNguoiDung { get; set; }

        /// <summary>
        /// Tổng số thiết bị trong hệ thống
        /// </summary>
        public int TongSoThietBi { get; set; }

        /// <summary>
        /// Tổng số báo cáo trong hệ thống
        /// </summary>
        public int TongSoBaoCao { get; set; }

        /// <summary>
        /// Chi tiết thống kê người dùng theo vai trò
        /// </summary>
        public List<UserRoleStatsModel> ThongKeNguoiDungTheoVaiTro { get; set; } = new List<UserRoleStatsModel>();

        /// <summary>
        /// Chi tiết thống kê thiết bị theo loại
        /// </summary>
        public List<EquipmentTypeStatsModel> ThongKeThietBiTheoLoai { get; set; } = new List<EquipmentTypeStatsModel>();

        /// <summary>
        /// Chi tiết thống kê báo cáo theo trạng thái
        /// </summary>
        public List<ReportStatusStatsModel> ThongKeBaoCaoTheoTrangThai { get; set; } = new List<ReportStatusStatsModel>();

        /// <summary>
        /// Thời gian cập nhật thống kê
        /// </summary>
        public DateTime ThoiGianCapNhat { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Thống kê người dùng theo vai trò
    /// </summary>
    public class UserRoleStatsModel
    {
        /// <summary>
        /// Tên vai trò (Admin, Lecturer, Student, HeadOfDepartment)
        /// </summary>
        public string TenVaiTro { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng người dùng có vai trò này
        /// </summary>
        public int SoLuong { get; set; }

        /// <summary>
        /// Phần trăm so với tổng số người dùng
        /// </summary>
        public double PhanTram { get; set; }

        /// <summary>
        /// Mô tả vai trò bằng tiếng Việt
        /// </summary>
        public string MoTaVaiTro { get; set; } = string.Empty;
    }

    /// <summary>
    /// Thống kê thiết bị theo loại
    /// </summary>
    public class EquipmentTypeStatsModel
    {
        /// <summary>
        /// ID loại thiết bị
        /// </summary>
        public string LoaiThietBiId { get; set; } = string.Empty;

        /// <summary>
        /// Tên loại thiết bị
        /// </summary>
        public string TenLoaiThietBi { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng thiết bị thuộc loại này
        /// </summary>
        public int SoLuong { get; set; }

        /// <summary>
        /// Phần trăm so với tổng số thiết bị
        /// </summary>
        public double PhanTram { get; set; }

        /// <summary>
        /// Số lượng thiết bị đang hoạt động (ACTIVE)
        /// </summary>
        public int SoLuongHoatDong { get; set; }

        /// <summary>
        /// Số lượng thiết bị không hoạt động (INACTIVE)
        /// </summary>
        public int SoLuongKhongHoatDong { get; set; }
    }

    /// <summary>
    /// Thống kê báo cáo theo trạng thái
    /// </summary>
    public class ReportStatusStatsModel
    {
        /// <summary>
        /// Trạng thái báo cáo
        /// </summary>
        public string TrangThai { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng báo cáo có trạng thái này
        /// </summary>
        public int SoLuong { get; set; }

        /// <summary>
        /// Phần trăm so với tổng số báo cáo
        /// </summary>
        public double PhanTram { get; set; }

        /// <summary>
        /// Mô tả trạng thái bằng tiếng Việt
        /// </summary>
        public string MoTaTrangThai { get; set; } = string.Empty;
    }
}
