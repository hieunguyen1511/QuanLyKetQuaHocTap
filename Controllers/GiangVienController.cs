using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyKetQuaHocTap.Models;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class GiangVienController : Controller
    {
        public static db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();
        public static tb_GiangVien gv;
        // GET: GiangVien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult KiemTraDangNhap(string userName, string password)
        {
            Session["ErrorWrongAccount"] = null;
            var tk = db.tb_TaiKhoan.SingleOrDefault(n => n.TenDN == userName && n.MatKhau == password && n.LoaiTaiKhoan == General.intTaiKhoanGV);
            if (tk != null)
            {
                gv = db.tb_GiangVien.SingleOrDefault(n => n.ID_TaiKhoan == tk.ID);
                Session["GV"] = gv;
                return RedirectToAction("TrangChu", "GiangVien");
            }
            else
            {
                Session["ErrorWrongAccount"] = "**Lỗi: Tài khoản không tồn tại";
            }
            return RedirectToAction("DangNhap", "GiangVien");
        }
    }
}
