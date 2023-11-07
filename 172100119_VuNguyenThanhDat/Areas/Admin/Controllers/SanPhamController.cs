using _172100119_VuNguyenThanhDat.Data;
using _172100119_VuNguyenThanhDat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace _172100119_VuNguyenThanhDat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SanPhamController(ApplicationDbContext db)
        {
            _db = db;
        }
		public string ULImage(IFormFile FileImage)
		{
			var UiFileName = Guid.NewGuid().ToString() + "_" + FileImage.FileName;

			string ULPath = Path.Combine("wwwroot//ImageFiles", UiFileName);

			var stream = new FileStream(ULPath, FileMode.Create);

			FileImage.CopyToAsync(stream);

			stream.Close();

			return UiFileName;
		}
		public void RemoveImage(string path)
		{
			string FullPath = "wwwroot//" + path;

			System.IO.File.Delete(FullPath);
		}
		public IActionResult Index()
        {
			// Include lay them thong tin cua bang phai co khoa ngoai moi dung duoc
			// Nen phai lam 1 class trung gian
			SanPhamViewModel sanpham = new SanPhamViewModel()
			{
				ListSanPham = _db.SanPham.Include("NhaCungCap").ToList(),
				ListCacTheLoai = _db.CacTheLoai.Include("TheLoai").ToList(),
			};
            return View(sanpham);
        }
        [HttpGet]
		public IActionResult Upsert(int id)
		{
			SanPhamViewModel sanpham = new SanPhamViewModel() // tao sp lam trung gian de truyen vao view
			{
				SP = new SanPham(),
			};

			IEnumerable<SelectListItem> dsnhacungcap = _db.NhaCungCap // tao selectlist NhaCungCap
				.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString() 
				}
			);
            ViewBag.DsNhaCungCap = dsnhacungcap;

			if (id == 0) // truong hop tao moi san pham
			{
				IEnumerable<SelectListItem> dstheloai = _db.TheLoai // tao selectlist Theloai
				.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}
				);
				ViewBag.DsTheLoai = dstheloai;

				sanpham.SP.Image = "ImageFiles/UserDefault.png"; // Gan anh mac dinh khi tao moi san pham

				return View(sanpham);
			}
			else // truong hop chinh sua san pham
			{
				sanpham.SP = _db.SanPham.Find(id);
				sanpham.ListCacTheLoai = _db.CacTheLoai.Where(ctl => ctl.SanPhamId == id).Include("TheLoai").ToList(); 
				return View(sanpham);
			}
		}
		[HttpPost]
		public IActionResult Upsert(SanPhamViewModel sanpham, IFormFile FileImage)
		{
			ModelState.Remove(nameof(FileImage));
			if (ModelState.IsValid)
			{
				if (FileImage != null) // cap nhat anh
				{
					if (sanpham.SP.Image != "ImageFiles/UserDefault.png")
						RemoveImage(sanpham.SP.Image);
					sanpham.SP.SavePath(ULImage(FileImage));
				}
				if(sanpham.SP.Id == 0)
				{
					_db.SanPham.Add(sanpham.SP);
					_db.SaveChanges();

					// Tim id san pham vua them 
					var listsp = _db.SanPham.ToList();
					int sanphamid = listsp[listsp.Count-1].Id;

					CacTheLoai ctl = new CacTheLoai()
					{
						SanPhamId = sanphamid,
						TheLoaiId = sanpham.TheLoaiId
					};
					_db.CacTheLoai.Add(ctl);
				}
				else
				{
					_db.SanPham.Update(sanpham.SP);
				}
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpGet]
		public IActionResult AddTheLoai(int id)
		{
			var dsthemtl = (from tl in _db.TheLoai
						from ctl in _db.CacTheLoai
						where ctl.SanPhamId == id
						where tl.Id != ctl.TheLoaiId
						select tl).ToList();
			IEnumerable<SelectListItem> ds_themtl = dsthemtl
			.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}
			);
			IEnumerable<SelectListItem> ds_xoatl = _db.CacTheLoai.Where(sp => sp.SanPhamId == id).Include("TheLoai").ToList()
			.Select(x => new SelectListItem
			{
				Text = x.TheLoai.Name,
				Value = x.TheLoai.Id.ToString()
			}
			);
			ViewBag.DsThemTL = ds_themtl;
			ViewBag.DsXoaTL = ds_xoatl;
			SanPhamViewModel sanpham = new SanPhamViewModel()
			{
				 SP = _db.SanPham.Find(id),
				 ListCacTheLoai = _db.CacTheLoai.Where(sp => sp.SanPhamId == id).Include("TheLoai").ToList()
			};
			return View(sanpham);
		}
		[HttpPost]
		public IActionResult AddTheLoai(SanPhamViewModel sanpham)
		{
			CacTheLoai cactheloai = new CacTheLoai()
			{
				SanPhamId = sanpham.SP.Id,
				TheLoaiId = sanpham.TheLoaiId
			};
			var dsthemtl = (from ctl in _db.CacTheLoai
							where ctl.SanPhamId == sanpham.SP.Id
							where ctl.TheLoaiId == sanpham.TheLoaiId
							select ctl).ToList();
			if (dsthemtl.Count()==0)
			{
				_db.CacTheLoai.Add(cactheloai);
			}
			else
			{
				List<CacTheLoai> timtl = _db.CacTheLoai.Where(x => x.SanPhamId == cactheloai.SanPhamId && x.TheLoaiId == cactheloai.TheLoaiId).ToList();
				var tl = _db.CacTheLoai.Find(timtl[0].Id);
				_db.CacTheLoai.Remove(tl);
			}
			_db.SaveChanges();
			return RedirectToAction("AddTheLoai",new {id = sanpham.SP.Id });
		}
		[HttpGet]
		public IActionResult DeleteSanPham(int id)
		{
			SanPhamViewModel sanpham = new SanPhamViewModel() // tao sp lam trung gian de truyen vao view
			{
				SP = _db.SanPham.Include("NhaCungCap").FirstOrDefault(x=>x.Id==id),
				ListCacTheLoai = _db.CacTheLoai.Where(x=> x.SanPhamId==id).Include("TheLoai").ToList()
			};
			return View(sanpham);
		}
		[HttpPost]
		public IActionResult DeleteSanPham(SanPhamViewModel sanpham)
		{
			if (sanpham.SP.Image != "ImageFiles/UserDefault.png")
				RemoveImage(sanpham.SP.Image);
			_db.SanPham.Remove(sanpham.SP);
			sanpham.ListCacTheLoai = _db.CacTheLoai.Where(x => x.SanPhamId == sanpham.SP.Id).Include("TheLoai").ToList();
			_db.CacTheLoai.RemoveRange(sanpham.ListCacTheLoai);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult InfoSanPham(int id)
		{
			SanPhamViewModel sanpham = new SanPhamViewModel() // tao sp lam trung gian de truyen vao view
			{
				SP = _db.SanPham.Include("NhaCungCap").FirstOrDefault(x => x.Id == id),
				ListCacTheLoai = _db.CacTheLoai.Where(x => x.SanPhamId == id).Include("TheLoai").ToList()
			};
			return View(sanpham);
		}
		public IActionResult DonHang(string id)
		{
			IEnumerable<HoaDon> hd = _db.HoaDon.Include("ApplicationUser").ToList();
			if (id != null)
			{
				hd = _db.HoaDon.Where(user => user.ApplicationUserId == id).Include("ApplicationUser").ToList();
			}
			return View(hd);
		}
		public IActionResult ChiTietDonHang(int hoadonId)
		{
			IEnumerable<ChiTietHoaDon> cthd = _db.ChiTietHoaDon.Include("SanPham").Where(x => x.HoaDonId == hoadonId).ToList();
			var tl = (from a in cthd
					from b in _db.CacTheLoai
					from c in _db.TheLoai
					where a.SanPham.Id == b.SanPhamId
					where b.TheLoaiId == c.Id
					select new
					{
						SPId = a.SanPhamId,
						TL = c.Name
					}).ToList();
			ViewBag.CacTheLoai = _db.CacTheLoai.Include("TheLoai").ToList();
			return View(cthd);
		}
		public IActionResult KhachHang()
		{
			IEnumerable<ApplicationUser> Users  = _db.ApplicationUser.Where(role => role.Name != "Admin").ToList();
			return View(Users);
		}
		[HttpGet]
		public IActionResult UpdateUser(string id) // chua lam
		{
			ApplicationUser User = _db.ApplicationUser.Find(id);
			return View(User);
		}
		[HttpPost]
		public IActionResult UpdateUser(ApplicationUser user)
		{
			ApplicationUser kh = _db.ApplicationUser.Find(user.Id);
			kh.Name = user.Name;
			kh.Address = user.Address;
			kh.PhoneNumber = user.PhoneNumber;
			_db.ApplicationUser.Update(kh);
			_db.SaveChanges();
			return RedirectToAction("KhachHang");
		}
	}
}
