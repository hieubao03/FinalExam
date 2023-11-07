using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class GioHang
    {
        [Key]
        public int Id { get; set; }
        public int SanPhamId { get; set; }
        [ForeignKey("SanPhamId")]
        [ValidateNever]
        public SanPham SanPham { get; set; }
        public int Quantity { get; set; }
        public string ApplicattionUserId { get; set; }
        [ForeignKey("ApplicattionUserId")]
        [ValidateNever] 
        public ApplicationUser ApplicationUser { get; set; }

        public double TongGiaSP()
        {
            return (Quantity * SanPham.Gia);
        }
    }
}
