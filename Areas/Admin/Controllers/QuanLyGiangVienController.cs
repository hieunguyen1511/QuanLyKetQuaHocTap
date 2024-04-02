using OfficeOpenXml;
using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class QuanLyGiangVienController : Controller
    {
        db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();
        // GET: Admin/QuanLyGiangVien
        public ActionResult Index()
        {
            if (dsGiangVien == null) loadDSGV();
            ViewBag.TrinhDo = new SelectList( new List<string>{ "Thạc sĩ", "Tiến sĩ" });
            ViewBag.KhoaVien = new SelectList(db.tb_KhoaVien, "ID", "TenKV");
            return View(dsGiangVien);
        }
        private static List<tb_GiangVien> dsGiangVien;
        private void loadDSGV()
        {
            dsGiangVien = db.tb_GiangVien.ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiem(FormCollection fields)
        {
            bool? GioiTinh = bool.Parse(fields["GioiTinh"]);
            int? KhoaVien = null;
            if (fields["KhoaVien"] != null)
            {
                KhoaVien = int.Parse(fields["KhoaVien"]);
            }

            if (GioiTinh == null)
            {
                loadDSGV();
            }
            else
            {
                dsGiangVien = db.tb_GiangVien.Where(s => s.GioiTinh == GioiTinh &&  s.tb_KhoaVien.Where(t=>t.ID==KhoaVien).FirstOrDefault() != null).ToList();
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
                loadDSGV();
            }
            else
            {
                dsGiangVien = db.tb_GiangVien.Where(s => s.HoTen.Contains(key) || s.MaGV.Contains(key)).ToList();
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection fields)
        {
            var dsGiangVien = db.tb_GiangVien.ToList();
            return View(dsGiangVien);
        }

        public ActionResult ChiTiet(int? id)
        {
            var thongtinGiangVien = db.tb_GiangVien.Where(s => s.ID == id).FirstOrDefault();
            return View(thongtinGiangVien);
        }

        public ActionResult SuaThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_GiangVien GiangVien = db.tb_GiangVien.Find(id);
            if (GiangVien == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ID_Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh", tb_SinhVien.ID_Nganh);

            return View(GiangVien);
        }



        tb_GiangVien SuaThongTinGV(int id, string hoten, DateTime ngaySinh, bool gioiTinh, string sdt, string email, string CCCD, string TrinhDo)
        {
            tb_GiangVien GV = db.tb_GiangVien.Find(id);
            try
            {
                GV.HoTen = hoten;
                GV.NgaySinh = ngaySinh;
                GV.GioiTinh = gioiTinh;
                GV.SDT = sdt;
                GV.Email = email;
                GV.CCCD = CCCD;
                GV.TrinhDo = TrinhDo;

            }
            catch
            {

            }
            return GV;
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
            var CCCD = fields["CCCD"];
            var trinhDo = fields["TrinhDo"];

            tb_GiangVien GV = SuaThongTinGV(ID, hoten, NgaySinh, GioiTinh, SDT, Email, CCCD, trinhDo);
            try
            {
                db.Entry(GV).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {

            }

            return RedirectToAction("Index");
        }


        public ActionResult ThemGiangVien()
        {
            return View();
        }






        private int? Tao_TaiKhoan_GV(string MaGV)
        {
            tb_TaiKhoan taikhoan = new tb_TaiKhoan();
            taikhoan.TenDN = MaGV;
            taikhoan.MatKhau = MaGV;
            taikhoan.LoaiTaiKhoan = 0;
            taikhoan.NgayCap = DateTime.Now;

            try
            {
                db.tb_TaiKhoan.Add(taikhoan);
                db.SaveChanges();
                var id = db.tb_TaiKhoan.Where(s => s.TenDN == MaGV).SingleOrDefault();
                return id.ID;
            }
            catch
            {
                return null;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemGiangVien(FormCollection fields)
        {
            var MaGV = fields["MaGV"];
            var HoTen = fields["HoTenGV"];
            var GioiTinh = bool.Parse(fields["GioiTinh"]);
            var NgaySinh = DateTime.Parse(fields["NgaySinh"]);
            var SDT = fields["SDT"];
            var Email = fields["Email"];
            var CCCD = fields["CCCD"];
            var TrinhDo = fields["TrinhDo"];


            tb_GiangVien GiangVien = new tb_GiangVien();

            GiangVien.MaGV = MaGV;
            GiangVien.HoTen = HoTen;
            GiangVien.GioiTinh = GioiTinh;
            GiangVien.NgaySinh = NgaySinh;
            GiangVien.SDT = SDT;
            GiangVien.Email = Email;
            GiangVien.CCCD = CCCD;
            GiangVien.TrinhDo = TrinhDo;

            var ID_TaiKhoan = Tao_TaiKhoan_GV(MaGV);
            if (ID_TaiKhoan is null)
            {
                ViewBag.LoiTaiKhoanGV = "Không thể thêm tài khoản";
                return View();
            }
            else
            {
                GiangVien.ID_TaiKhoan = ID_TaiKhoan;

                try
                {
                    db.tb_GiangVien.Add(GiangVien);
                    db.SaveChanges();
                }
                catch
                {
                    Session["LoiThemGV"] = "Không thể thêm giảng viên";
                    var taiKhoan = db.tb_TaiKhoan.Find(ID_TaiKhoan);
                    db.tb_TaiKhoan.Remove(taiKhoan);
                    db.SaveChanges();
                    GiangVien = null;
                    return View();
                }
            }
            Session["ThemGV"] = "Thêm giảng viên thành công";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaGiangVien(FormCollection fields)
        {
            var id = int.Parse(fields["ID"]);
            var giangVien = db.tb_GiangVien.Find(id);
            if (giangVien != null)
            {
                try
                {
                    var taikhoan = db.tb_TaiKhoan.Find(giangVien.ID_TaiKhoan);
                    db.tb_TaiKhoan.Remove(taikhoan);
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index");
                }

                db.tb_GiangVien.Remove(giangVien);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    Session["LoiXoaGV"] = "Không thể xóa giảng viên";   
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }




        public void InFileExcel()
        {
            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách giảng viên");

            ws.Cells["A1"].Value = "Mã GV";
            ws.Cells["B1"].Value = "Họ tên";
            ws.Cells["C1"].Value = "Giới tính";
            ws.Cells["D1"].Value = "Ngày sinh";
            ws.Cells["E1"].Value = "Số điện thoại";
            ws.Cells["F1"].Value = "Email";
            ws.Cells["G1"].Value = "CCCD";
            ws.Cells["H1"].Value = "Trình độ";
            ws.Cells["I1"].Value = "Khoa viện";

            int row_start = 2;
            int stt = 1;
            if (dsGiangVien is null)
            {
                loadDSGV();
            }
            foreach (var item in dsGiangVien)
            {
                ws.Cells[string.Format("A{0}", row_start)].Value = item.MaGV;
                ws.Cells[string.Format("B{0}", row_start)].Value = item.HoTen;
                ws.Cells[string.Format("C{0}", row_start)].Value = item.GioiTinh == true ? "Nam" : "Nữ";
                ws.Cells[string.Format("D{0}", row_start)].Value = item.NgaySinh.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("E{0}", row_start)].Value = item.SDT;
                ws.Cells[string.Format("F{0}", row_start)].Value = item.Email;
                ws.Cells[string.Format("G{0}", row_start)].Value = item.CCCD;
                ws.Cells[string.Format("H{0}", row_start)].Value = item.TrinhDo;
                string KhoaVien = "";
                foreach (var kv in item.tb_KhoaVien)
                {
                    KhoaVien += kv.TenKV + ", ";
                }
                ws.Cells[string.Format("I{0}", row_start)].Value = KhoaVien;
                row_start++;
                stt++;
            }

            string excelName = "Danh sách giảng viên";
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





    }
}