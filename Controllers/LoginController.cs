using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public static tb_TaiKhoan taiKhoan;
        public ActionResult DangNhap()
        {
            Session["TK"] = null;
            return View();
        }

        public ActionResult KiemTraDangNhap(string userName, string password)
        {
            Session["ErrorWrongAccount"] = null;
            taiKhoan = General.db.tb_TaiKhoan.SingleOrDefault(n => n.TenDN == userName && n.MatKhau == password && n.LoaiTaiKhoan != General.intTaiKhoanQT);
            if (taiKhoan != null)
            {
                Session["TK"] = taiKhoan;
                if (taiKhoan.LoaiTaiKhoan == General.intTaiKhoanGV)
                {
                    return RedirectToAction("TrangChu", "GiangVien");
                }
                return RedirectToAction("TrangChu", "SinhVien");
            }
            else
            {
                Session["ErrorWrongAccount"] = "**Lỗi: Tài khoản không tồn tại";
            }
            return RedirectToAction("DangNhap");
        }

        public ActionResult DangXuat()
        {
            bool gv = taiKhoan.LoaiTaiKhoan == General.intTaiKhoanGV;
            Session["TK"] = null;
            if (gv)
            {
                return RedirectToAction("TrangChu", "GiangVien");
            }
            return RedirectToAction("TrangChu", "SinhVien");
        }
    }
}
