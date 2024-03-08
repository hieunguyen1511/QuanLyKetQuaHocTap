using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class QuanLySinhVienController : Controller
    {
        // GET: Admin/QuanLySinhVien
        QuanLyKetQuaHocTapEntities db = new QuanLyKetQuaHocTapEntities();



        static List<tb_SinhVien> dsSinhVien;

        void loadDSSV()
        {
            dsSinhVien = db.tb_SinhVien.ToList();
        }

        public ActionResult Index()
        {
            if (dsSinhVien == null) loadDSSV();
            ViewBag.ChuyenNganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            return View(dsSinhVien);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiem(FormCollection fields)
        {
            bool? GioiTinh = bool.Parse(fields["GioiTinh"]);
            var ChuyenNganh = int.Parse(fields["ChuyenNganh"]);
            if (GioiTinh == null)
            {
                loadDSSV();
            }
            else
            {
                dsSinhVien = db.tb_SinhVien.Where(s => s.GioiTinh == GioiTinh && s.ID_Nganh == ChuyenNganh).ToList();
            }
            ViewBag.ChuyenNganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiem(FormCollection fields)
        {
            var key = fields["KEY"];
            if (key == null)
            {
                loadDSSV();
            }
            else
            {
                dsSinhVien = db.tb_SinhVien.Where(s => s.HoTen.Contains(key) || s.MaSV.Contains(key)).ToList();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSinhVien(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var SV = db.tb_SinhVien.Find(ID);
            if (SV != null)
            {
                db.tb_SinhVien.Remove(SV);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index", ViewBag.LoiXoaSV = "Không thể xóa sinh viên này");
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult SuaThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_SinhVien tb_SinhVien = db.tb_SinhVien.Find(id);
            if (tb_SinhVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh", tb_SinhVien.ID_Nganh);

            return View(tb_SinhVien);
        }


        tb_SinhVien SuaThongTinSV(int id, string hoten, DateTime ngaySinh, bool gioiTinh, string sdt, string email, string tonGiao, int chuyenNganh)
        {
            tb_SinhVien sv = db.tb_SinhVien.Find(id);
            try
            {
                sv.HoTen = hoten;
                sv.NgaySinh = ngaySinh;
                sv.GioiTinh = gioiTinh;
                sv.SDT = sdt;
                sv.Email = email;
                sv.TonGiao = tonGiao;
                sv.ID_Nganh = chuyenNganh;

            }
            catch
            {

            }
            return sv;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SuaThongTin(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var hoten = fields["HoTen"];
            var NgaySinh = DateTime.Parse(fields["NgaySinh"]);
            var GioiTinh = bool.Parse(fields["GioiTinh"]);
            var SDT = fields["SDT"];
            var Email = fields["Email"];
            var TonGiao = fields["TonGiao"];
            var ChuyenNganh = int.Parse(fields["ID_Nganh"]);

            tb_SinhVien sv = SuaThongTinSV(ID, hoten, NgaySinh, GioiTinh, SDT, Email, TonGiao, ChuyenNganh);
            try
            {
                db.Entry(sv).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {

            }
            loadDSSV();

            return RedirectToAction("Index");
        }

        public ActionResult Chitiet(int? id)
        {
            var sv = db.tb_SinhVien.Find(id);
            return View(sv);
        }


    }
}