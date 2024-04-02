<<<<<<< HEAD
﻿using OfficeOpenXml;
using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
=======
﻿using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class QuanLySinhVienController : Controller
    {
        // GET: Admin/QuanLySinhVien
<<<<<<< HEAD
        db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();
=======
        QuanLyKetQuaHocTapEntities db = new QuanLyKetQuaHocTapEntities();
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b



        static List<tb_SinhVien> dsSinhVien;

        void loadDSSV()
        {
            dsSinhVien = db.tb_SinhVien.ToList();
        }

<<<<<<< HEAD
        void getNull_DSSV()
        {
            dsSinhVien = null;
        }   

        public ActionResult Index()
        {
            
            if (dsSinhVien == null ) loadDSSV();
            var tmp = dsSinhVien;
            getNull_DSSV();
            ViewBag.ChuyenNganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            return View(tmp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiem(FormCollection fields)
        {
            bool? GioiTinh = bool.Parse(fields["GioiTinh"]);
            int? ChuyenNganh = null;
            if (fields["ChuyenNganh"] != null)
            {
                ChuyenNganh= int.Parse(fields["ChuyenNganh"]);
            }
            
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


        tb_SinhVien SuaThongTinSV(int id,string hoten, DateTime ngaySinh, bool gioiTinh, string sdt, string email, string danToc, int chuyenNganh)
        {
            tb_SinhVien sv = db.tb_SinhVien.Find(id);
            try
            {
                sv.HoTen = hoten;
                sv.NgaySinh = ngaySinh;
                sv.GioiTinh = gioiTinh;
                sv.SDT = sdt;
                sv.Email = email;
                sv.DanToc = danToc;
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
            var danToc = fields["DanToc"];
            var ChuyenNganh = int.Parse(fields["ID_Nganh"]);

            tb_SinhVien sv = SuaThongTinSV(ID, hoten, NgaySinh, GioiTinh, SDT, Email, danToc, ChuyenNganh);
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



        public ActionResult ThemSinhVien()
        {
            ViewBag.ID_Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            ViewBag.ID_CoVan = new SelectList(db.tb_CoVanHocTap, "ID", "tb_GiangVien.HoTen");
            return View();
        }

        
        private int? Tao_TaiKhoan_SV(string MaSV)
        {
            tb_TaiKhoan taikhoan = new tb_TaiKhoan();
            taikhoan.TenDN = MaSV;
            taikhoan.MatKhau = MaSV;
            taikhoan.LoaiTaiKhoan = 1;
            taikhoan.NgayCap = DateTime.Now;

            try
            {
                db.tb_TaiKhoan.Add(taikhoan);
                db.SaveChanges();
                var id = db.tb_TaiKhoan.Where(s => s.TenDN == MaSV).SingleOrDefault();
                return id.ID;
            }
            catch
            {
                return null;
            }

=======
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
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public ActionResult ThemSinhVien(FormCollection fields)
        {
            var MaSV = fields["MaSV"];
            var HoTen = fields["HoTenSV"];
            var GioiTinh = bool.Parse(fields["GioiTinh"]);
            var NgaySinh = DateTime.Parse(fields["NgaySinh"]);
            var SDT = fields["SDT"];
            var Email = fields["Email"];
            var DanToc = fields["DanToc"];
            var CCCD = fields["CCCD"];
            var NNKC = int.Parse(fields["NNKC"]);
            var KNXH = int.Parse(fields["KNXH"]);

            int? ID_Nganh = null;
            int? ID_CoVan = null;
            if (fields["ID_Nganh"] != null)
            {
                ID_Nganh = int.Parse(fields["ID_Nganh"]);
            }
            if (fields["ID_CoVan"] != null)
            {
                ID_CoVan = int.Parse(fields["ID_CoVan"]);
            }


            var checkMaSV = db.tb_SinhVien.Where(s => s.MaSV == MaSV).FirstOrDefault();
            var checkTaiKhoan = db.tb_TaiKhoan.Where(s => s.TenDN == MaSV).FirstOrDefault();
            if (checkMaSV != null) 
            {
                ViewBag.CheckMaSV = "Mã sinh viên đã tồn tại";
                return View();
            }
           
            if (checkTaiKhoan != null)
            {
                ViewBag.CheckTaiKhoan = "Tài khoản đã tồn tại";
                return View();
            }

            tb_SinhVien SinhVien = new tb_SinhVien();

            SinhVien.MaSV = MaSV;
            SinhVien.HoTen = HoTen;
            SinhVien.GioiTinh = GioiTinh;
            SinhVien.NgaySinh = NgaySinh;
            SinhVien.SDT = SDT;
            SinhVien.Email = Email;
            SinhVien.DanToc = DanToc;
            SinhVien.CCCD = CCCD;
            SinhVien.Cap_KyNang = KNXH;
            SinhVien.Cap_NNKC = NNKC;
            SinhVien.ID_Nganh = ID_Nganh;
            SinhVien.ID_CoVan = ID_CoVan;

            var ID_TaiKhoan = Tao_TaiKhoan_SV(MaSV);
            if (ID_TaiKhoan is null)
            {
                ViewBag.LoiTaiKhoanSV = "Không thể thêm tài khoản";
                return View();
            }
            else
            {
                SinhVien.ID_TaiKhoan = ID_TaiKhoan;

                try
                {
                    db.tb_SinhVien.Add(SinhVien);
=======
        public ActionResult XoaSinhVien(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var SV = db.tb_SinhVien.Find(ID);
            if (SV != null)
            {
                db.tb_SinhVien.Remove(SV);
                try
                {
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
                    db.SaveChanges();
                }
                catch
                {
<<<<<<< HEAD
                    Session["LoiThemSV"] = "Không thể thêm sinh viên";
                    var taiKhoan = db.tb_TaiKhoan.Find(ID_TaiKhoan);
                    db.tb_TaiKhoan.Remove(taiKhoan);
                    db.SaveChanges();
                    SinhVien = null;
                    return View();
                }
                
            }
            
            Session["ThemSV"] = "Thêm sinh viên thành công";
            return RedirectToAction("Index");
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        private void ThemDanhSach(HttpPostedFileBase file)
        {
            ExcelPackage excel = new ExcelPackage();
        }
        


        public ActionResult DanhSachSinhVien()
        {
            List<tb_SinhVien> danhSach = Session["DSSV"] as List<tb_SinhVien>;
            if (danhSach is null)
            {
                Session["DSSV"] = null;
                return RedirectToAction("ThemSinhVien");

            }
            return View();
        }


        public void InFileExcel()
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách sinh viên");

            ws.Cells["A1"].Value = "Mã sinh viên";
            ws.Cells["B1"].Value = "Họ tên";
            ws.Cells["C1"].Value = "Giới tính";
            ws.Cells["D1"].Value = "Ngày sinh";
            ws.Cells["E1"].Value = "Số điện thoại";
            ws.Cells["F1"].Value = "Email";
            ws.Cells["G1"].Value = "Dân tộc";
            ws.Cells["H1"].Value = "CCCD";
            ws.Cells["I1"].Value = "Chuyên Ngành";

            int row_start = 2;
            int stt = 1;
            if (dsSinhVien is null)
            {
                loadDSSV();
            }
            foreach (var item in dsSinhVien)
            {
                ws.Cells[string.Format("A{0}", row_start)].Value = item.MaSV;
                ws.Cells[string.Format("B{0}", row_start)].Value = item.HoTen;
                ws.Cells[string.Format("C{0}", row_start)].Value = item.GioiTinh == true ? "Nam" : "Nữ";
                ws.Cells[string.Format("D{0}", row_start)].Value = item.NgaySinh.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("E{0}", row_start)].Value = item.SDT;
                ws.Cells[string.Format("F{0}", row_start)].Value = item.Email;
                ws.Cells[string.Format("G{0}", row_start)].Value = item.DanToc;
                ws.Cells[string.Format("H{0}", row_start)].Value = item.CCCD;
                ws.Cells[string.Format("I{0}", row_start)].Value = item.tb_Nganh.TenNganh;
                row_start++;
                stt++;
            }

            string excelName = "Danh sách sinh viên";
            ws.Cells["A:AZ"].AutoFitColumns();

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                pck.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }




=======
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


>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
    }
}