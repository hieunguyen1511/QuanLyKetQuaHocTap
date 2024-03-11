using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class GiangVienController : Controller
    {
        // GET: GiangVien
        public static tb_GiangVien gv;

        public ActionResult TrangChu()
        {
            if (Session["TK"] != null)
            {
                tb_TaiKhoan taiKhoan = (tb_TaiKhoan)Session["TK"];
                if (taiKhoan.LoaiTaiKhoan == General.intTaiKhoanGV)
                {
                    gv = taiKhoan.tb_GiangVien.First();
                }
            }
            return View(gv);
        }

        public ActionResult ThongTinCaNhan()
        {
            return View(gv);
        }

        public ActionResult HienThiThongTinCaNhan_Partial(int? id)
        {
            var gv = General.db.tb_GiangVien.Find(id);
            return PartialView(gv);
        }

        public ActionResult CapNhatThongTinCaNhan(int? id, FormCollection collection)
        {
            string email = collection["email"];
            string sdt = collection["sdt"];
            DateTime ngaySinh = DateTime.Parse(collection["ngaySinh"]);
            bool gioiTinh = collection["gioiTinh"] != General.strGioiTinhNu;
            tb_GiangVien gv = General.db.tb_GiangVien.Find(id);
            gv.Email = email;
            gv.SDT = sdt;
            gv.NgaySinh = ngaySinh;
            gv.GioiTinh = gioiTinh;
            General.db.Entry(gv).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            Session["GV"] = gv;
            GiangVienController.gv = gv;
            return RedirectToAction("ThongTinCaNhan");
        }

        public ActionResult BangThongKeLopHoc()
        {
            var lh = gv.tb_CoVanHocTap;
            return View(lh);
        }

        public ActionResult BangThongKeHocPhan()
        {
            var hp = gv.tb_HocPhan;
            return View(hp);
        }
    }
}
