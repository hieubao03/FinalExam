using _172100119_VuNguyenThanhDat.Data;
using _172100119_VuNguyenThanhDat.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace _172100119_VuNguyenThanhDat.Areas.Admin.Controllers
{
    [Area("Admin")]
	public class TheLoaiController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiController(ApplicationDbContext db)
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
            var DsTheLoai = _db.TheLoai.ToList();
            //var DsTheLoai = (from item in _db.TheLoai
            //                 where item.DateCreated.Day < 30 && item.DateCreated.Year < 2023
            //                 select item).ToList();
            ViewBag.TheLoai = DsTheLoai;
            return View();
        }

        [HttpGet]
        public IActionResult CreateTheLoai()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTheLoai(TheLoai theloai, IFormFile FileImage)
        {
            ModelState.Remove(nameof(FileImage));
            if (ModelState.IsValid)
            {
                if (FileImage != null)
                {
                    theloai.SavePath(ULImage(FileImage));
                }
                _db.TheLoai.Add(theloai);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditTheLoai(int id)
        {

            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }
        [HttpPost]
        public IActionResult EditTheLoai(TheLoai theloai, IFormFile FileImage)
        {
            if (FileImage != null)
            {
                if (theloai.Image != "ImageFiles/UserDefault.png")
                    RemoveImage(theloai.Image);
                theloai.SavePath(ULImage(FileImage));
            }
            _db.TheLoai.Update(theloai);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteTheLoai(int id)
        {
            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }
        [HttpPost]
        public IActionResult DeleteTheLoai(TheLoai theloai, IFormFile FileImage)
        {
            if (theloai.Image != "ImageFiles/UserDefault.png")
                RemoveImage(theloai.Image);
            _db.TheLoai.Remove(theloai);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult InfoTheLoai(int id)
        {
            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }


        


        //////////////////////////////
        public IActionResult DsNhaCungCap()
        {
            var DsNhaCungCap = _db.NhaCungCap.ToList();
            ViewBag.NhaCungCap = DsNhaCungCap;
            return View();
        }
		[HttpGet]
		public IActionResult CreateNhaCungCap()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CreateNhaCungCap(NhaCungCap ncc) 
		{
			if (ModelState.IsValid)
			{
				_db.NhaCungCap.Add(ncc);
				_db.SaveChanges();
				return RedirectToAction("DsNhaCungCap");
			}
			return View();
		}
		[HttpGet]
        public IActionResult EditNhaCungCap(int id) 
        {

            var ncc = _db.NhaCungCap.Find(id);
            return View(ncc);
        }
        [HttpPost]
        public IActionResult EditNhaCungCap(NhaCungCap ncc)
        {
            _db.NhaCungCap.Update(ncc);
            _db.SaveChanges();
            return RedirectToAction("DsNhaCungCap");
        }
        [HttpGet]
        public IActionResult DeleteNhaCungCap(int id)
        {
            var ncc = _db.NhaCungCap.Find(id);
            return View(ncc);
        }
        [HttpPost]
        public IActionResult DeleteNhaCungCap(NhaCungCap ncc)
        {
            _db.NhaCungCap.Remove(ncc);
            _db.SaveChanges();
            return RedirectToAction("DsNhaCungCap");
        }
    }
}
