using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();

        public ActionResult Index()
        {
            if (Session["TK"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap", new {Area=""});
            }
            var TK = Session["TK"] as tb_TaiKhoan;
            var QT = db.tb_QuanTri.Where(s=>s.ID_TaiKhoan==TK.ID).FirstOrDefault();
            return View(QT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau(FormCollection fields)
        {
            if (Session["TK"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap", new { Area = "" });
            }
            var TK = Session["TK"] as tb_TaiKhoan;
            var TkAdmin = db.tb_TaiKhoan.Find(TK.ID);
            if (TkAdmin.MatKhau == fields["MatKhauCu"])
            {
                TkAdmin.MatKhau = fields["MatKhauMoi"];
                try
                {
                    db.Entry(TkAdmin).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["DoiMK"] = "Đổi mật khẩu thành công!";
                }
                catch
                {
                    
                }
                
            }
            else
            {
                Session["LoiDoiMK"] = "Mật khẩu cũ không đúng!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult DangXuat()
        {
            Session["TK"] = null;
            return RedirectToAction("DangNhap", "DangNhap", new { Area = "" });
        }

    }
}