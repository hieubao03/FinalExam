using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class SanPhamViewModel
    {
		[ValidateNever]
		public IEnumerable<SanPham> ListSanPham { get; set; }
		[ValidateNever]
		public IEnumerable<CacTheLoai> ListCacTheLoai { get; set; }
        [ValidateNever]
        public IEnumerable<TheLoai> ListTheLoai { get; set; }
        public SanPham SP { get; set; }
        public int TheLoaiId { get; set; }
	}
}
