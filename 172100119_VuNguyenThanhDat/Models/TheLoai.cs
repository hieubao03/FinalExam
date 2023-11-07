using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class TheLoai
    {
        [Key] 
        public int Id { get; set; }
        [Required(ErrorMessage ="Phải nhập tên thể loại!")]
        [Display(Name = "Thể loại")]
        public string Name { get; set; }
		[Required(ErrorMessage = "Không đúng định dạng!")]
		[Display(Name = "Ngày tạo")]
		public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Image { get; set; } 

        public void SavePath(string path)
        {
            Image = "ImageFiles/" + path;
        }
	}
}
