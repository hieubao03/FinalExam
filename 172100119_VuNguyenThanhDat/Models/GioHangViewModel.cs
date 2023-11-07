namespace _172100119_VuNguyenThanhDat.Models
{
    public class GioHangViewModel
    {
        // Luu tru thong tin cac san pham trong gio hang
        public IEnumerable<GioHang>  DsGioHang { get; set; }
        // Luu giu tong so tien gio hang
        public HoaDon HoaDon { get; set; }
    }
}
