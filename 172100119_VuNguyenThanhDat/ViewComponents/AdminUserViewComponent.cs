using _172100119_VuNguyenThanhDat.Data;
using _172100119_VuNguyenThanhDat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _172100119_VuNguyenThanhDat.ViewComponents
{
    public class AdminUserViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public AdminUserViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if(identity.Claims.Count() != 0)// kiem tra Claims de tranh loi dong RoleName khong tim duoc claim
            {
                var RoleName = identity.FindFirst(ClaimTypes.Role).Value; // tim kiem role cua tai khoan hien tai
                ViewBag.RoleName = RoleName;
            }
            else
            {
                ViewBag.RoleName = null;
            }
            return View();
        }
    }
}
