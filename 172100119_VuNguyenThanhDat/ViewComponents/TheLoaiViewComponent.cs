using _172100119_VuNguyenThanhDat.Data;
using _172100119_VuNguyenThanhDat.Models;
using Microsoft.AspNetCore.Mvc;

namespace _172100119_VuNguyenThanhDat.ViewComponents
{
    public class TheLoaiViewComponent : ViewComponent //gan giong controller
    {
        private readonly ApplicationDbContext _db;

        public TheLoaiViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<TheLoai> theloai = _db.TheLoai.ToList();
            return View(theloai);
        }








    }
}
