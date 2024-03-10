using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class SinhVienController : Controller
    {
        // GET: SinhVien
        public static tb_SinhVien sv;

        public ActionResult TrangChu()
        {
            if (Session["TK"] != null)
            {
                tb_TaiKhoan taiKhoan = (tb_TaiKhoan)Session["TK"];
                if (taiKhoan.LoaiTaiKhoan == General.intTaiKhoanSV)
                {
                    sv = taiKhoan.tb_SinhVien.First();
                }
            }
            return View(sv);
        }

        public ActionResult ThongTinCaNhan()
        {
            return View(sv);
        }
        public ActionResult HienThiThongTinCaNhan_Partial(int? id)
        {
            var sv = General.db.tb_SinhVien.Find(id);
            return PartialView(sv);
        }

        public ActionResult CapNhatThongTinCaNhan(int? id, FormCollection collection)
        {
            string email = collection["email"];
            string sdt = collection["sdt"];
            DateTime ngaySinh = DateTime.Parse(collection["ngaySinh"]);
            bool gioiTinh = collection["gioiTinh"] != General.strGioiTinhNu;
            tb_SinhVien sv = General.db.tb_SinhVien.Find(id);
            sv.Email = email;
            sv.SDT = sdt;
            sv.NgaySinh = ngaySinh;
            sv.GioiTinh = gioiTinh;
            General.db.Entry(sv).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            Session["SV"] = sv;
            SinhVienController.sv = sv;
            return RedirectToAction("ThongTinCaNhan");
        }
    }
}
