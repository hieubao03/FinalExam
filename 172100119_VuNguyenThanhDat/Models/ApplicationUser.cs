using Microsoft.AspNetCore.Identity;

namespace _172100119_VuNguyenThanhDat.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string? Address { get; set; }
    }
}
