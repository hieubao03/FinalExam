using _172100119_VuNguyenThanhDat.Data;
using _172100119_VuNguyenThanhDat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace _172100119_VuNguyenThanhDat.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            var tl = (from a in _db.TheLoai
                         from b in _db.CacTheLoai
                         where a.Id == b.TheLoaiId
                         select a).ToList();
            var dtl = tl.DistinctBy(x => x.Name).ToList();
            ViewBag.listtheloai = dtl;
            SanPhamViewModel sanpham = new SanPhamViewModel()
            {
                ListSanPham = _db.SanPham.Include("NhaCungCap").ToList(),
                ListCacTheLoai = _db.CacTheLoai.Include("TheLoai").ToList(),
            };
            return View(sanpham);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult BanhKem(int id)
        {
            var DsSanPham = (from a in _db.SanPham
                             from b in _db.CacTheLoai
                             where b.TheLoaiId == id
                             where a.Id == b.SanPhamId
                             select a).ToList();
            ViewBag.theloai = _db.TheLoai.Find(id).Name;
            ViewBag.DsSanPham = DsSanPham;
            return View();
        }

        //public IActionResult BanhQuy(int id)
        //{
        //    var DsSanPham = (from a in _db.SanPham
        //                     from b in _db.CacTheLoai
        //                     where b.TheLoaiId == id
        //                     where a.Id == b.SanPhamId
        //                     select a).ToList();
        //    ViewBag.theloai = _db.TheLoai.Find(id).Name;
        //    ViewBag.DsSanPham = DsSanPham;
        //    return View();
        //}

        [Authorize]
        public IActionResult GioHang()
        {
            // lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang
                            .Include("SanPham")
                            .Where(gh => gh.ApplicattionUserId == claim.Value)
                            .ToList(),
                HoaDon = new HoaDon()
            };
            return View(giohang);
        }
        [HttpGet]
        public IActionResult Detail(int sanphamId)
        {
            // Tao gio hang o trang Detail de xu ly chuc nang them vao gio
            GioHang giohang = new GioHang()
            {
                SanPhamId = sanphamId,
                Quantity = 1,   //Mac dinh so luong la 1
                SanPham = _db.SanPham.Include("NhaCungCap").FirstOrDefault(sp => sp.Id == sanphamId),
            };
            List<CacTheLoai> splq= (from a in _db.CacTheLoai
                                            from b in (from d in _db.CacTheLoai where d.SanPhamId == sanphamId select d)
                                            where a.TheLoaiId == b.TheLoaiId
                                            where a.SanPhamId != sanphamId
                                            select a).Include("SanPham").ToList();
            while(splq.Count() > 3)
            {
                Random r = new Random();
                int random = r.Next(0, splq.Count());
                splq.Remove(splq[random]);
            }
            IEnumerable<CacTheLoai> ctl = _db.CacTheLoai.Where(sp => sp.SanPhamId == sanphamId).Include("TheLoai").ToList();
            ViewBag.DsSanPham = splq;
            ViewBag.DsTheLoai = ctl;
            ViewBag.sltl = ctl.Count();
            return View(giohang);
        }
        [HttpPost]
        [Authorize] // phai dang nhap voi vao trang nay duoc
        public IActionResult Detail(GioHang giohang)
        {
            // lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            // dua thong tin tai khoan vao gio hang
            giohang.ApplicattionUserId = claim.Value;

            var giohangFormDb = _db.GioHang.FirstOrDefault(gh => gh.SanPhamId == giohang.SanPhamId);// kiem tra
            if (giohangFormDb == null) // neu chua co san pham nay thi them them vao database
            {
                // them gio hang trong database
                _db.GioHang.Add(giohang);
            }
            else                        // neu co san pham nay trong database thi dang so luong do len 
            {
                giohangFormDb.Quantity += giohang.Quantity;
            }
            _db.SaveChanges();

            return RedirectToAction("GioHang");
        }
        public IActionResult Giam(int giohangId)
        {
            var gh = _db.GioHang.Find(giohangId);
            gh.Quantity -= 1;
            if (gh.Quantity <= 0)
            {
                _db.GioHang.Remove(gh);
            }
            _db.SaveChanges();
            return RedirectToAction("GioHang");
        }
        public IActionResult Tang(int giohangId)
        {
            var gh = _db.GioHang.Find(giohangId);
            gh.Quantity += 1;
            _db.SaveChanges();
            return RedirectToAction("GioHang");
        }
        public IActionResult Xoa(int giohangId)
        {
            var gh = _db.GioHang.Find(giohangId);
            _db.GioHang.Remove(gh);
            _db.SaveChanges();
            return RedirectToAction("GioHang");
        }
        public IActionResult ThanhToan()
        {
            // lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            // tim gio hang cua tai khoan do
            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang
                            .Include("SanPham")
                            .Where(gh => gh.ApplicattionUserId == claim.Value)
                            .ToList(),
                HoaDon = new HoaDon()
            };
            // lay thong tin tai khoan
            giohang.HoaDon.ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);
            giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
            giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;
            giohang.HoaDon.PhoneNumber = giohang.HoaDon.ApplicationUser.PhoneNumber;
            return View(giohang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan(GioHangViewModel giohang)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.DsGioHang = _db.GioHang
                            .Include("SanPham")
                            .Where(gh => gh.ApplicattionUserId == claim.Value)
                            .ToList();
            giohang.HoaDon.ApplicationUserId = claim.Value;
            giohang.HoaDon.OrderDate = DateTime.Now;
            giohang.HoaDon.OrderStatus = "Đang cập nhật...";
            _db.HoaDon.Add(giohang.HoaDon);
            _db.SaveChanges();

            foreach (var item in giohang.DsGioHang)
            {
                ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon()
                {
                    SanPhamId = item.SanPhamId,
                    HoaDonId = giohang.HoaDon.Id,
                    ProductPrice = item.TongGiaSP(),
                    Quantity = item.Quantity
                };
                _db.ChiTietHoaDon.Add(chiTietHoaDon);
                _db.SaveChanges();
            }
            _db.GioHang.RemoveRange(giohang.DsGioHang);
            _db.SaveChanges();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public IActionResult UpdateUser()
        {
            // lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser user = _db.ApplicationUser.Find(claim.Value);
            return View(user);
        }
        [HttpPost]
        public IActionResult UpdateUser(ApplicationUser user)
        {
			// lay thong tin tai khoan
			ApplicationUser kh = _db.ApplicationUser.Find(user.Id);
			kh.Name = user.Name;
			kh.Address = user.Address;
			kh.PhoneNumber = user.PhoneNumber;
			_db.ApplicationUser.Update(kh);
			_db.SaveChanges();
			return View();
		}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}