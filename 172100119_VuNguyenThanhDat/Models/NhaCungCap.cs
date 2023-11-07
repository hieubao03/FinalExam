using System.ComponentModel.DataAnnotations;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class NhaCungCap
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
