using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class CacTheLoai
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TheLoaiId { get; set; } // fk
        [ForeignKey("TheLoaiId")]
        [ValidateNever]
        public TheLoai TheLoai { get; set; }
        public int SanPhamId { get; set; } //fk
        [ForeignKey("SanPhamId")]
        [ValidateNever]
        public SanPham SanPham { get; set; }
    }
}
