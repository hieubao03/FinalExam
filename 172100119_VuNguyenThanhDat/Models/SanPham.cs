using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _172100119_VuNguyenThanhDat.Models
{
	public class SanPham
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Bạn không được để trống Tên sản phẩm!")]
		[Display(Name = "Tên sản phẩm")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Bạn không được để trống Giá sản phẩm!")]
		[Display(Name = "Giá sản phẩm")] 
		public double Gia { get; set; }
		public string? Image { get; set; }
		[Required]
		public int NhaCungCapId { get; set; }
		[ForeignKey("NhaCungCapId")]
		[ValidateNever]
		public NhaCungCap NhaCungCap { get; set; }

		public void SavePath(string path)
		{
			Image = "ImageFiles/" + path;
		}
	}
}
