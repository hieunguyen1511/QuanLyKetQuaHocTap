using OfficeOpenXml;
using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();

        class soLuongThongKe
        {
            public int slSVDky { get; set; }
            public int slSVQuaMon { get; set; }
            public int slSVRot { get; set; }
            public int slSVRot1Mon { get; set; }
            public int slSVRot2Mon { get; set; }
            public int slSVRot3MonTroLen { get; set; }
        }


        soLuongThongKe Load_ThongKe(int? id_nganh, int? hocKi, int? nam)
        {
            List<tb_SinhVien> svDaDky = null;
            if (id_nganh == null || hocKi == null || nam == null)
            {
                svDaDky = db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.HocKi_DK == 1 && s.NamHoc_DK == 1) != null).ToList();
            }
            else
            {
                svDaDky = db.tb_SinhVien.Where(n => n.ID_Nganh == id_nganh && n.tb_DiemHocPhan.FirstOrDefault(s => s.HocKi_DK == hocKi && s.NamHoc_DK == nam) != null).ToList();
            } 
            var svQuaMon = new List<tb_SinhVien>();

            foreach (var sv in svDaDky)
            {
                if (sv.tb_DiemHocPhan.FirstOrDefault(s => (s.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && ((float?)General.getDiemTB(s.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, s.DiemGK, s.DiemCK)) < 5) || (s.DiemCK == General.intRot)) == null)
                {
                    svQuaMon.Add(sv);
                }
            }
            var svRot = new List<tb_SinhVien>();

            foreach (var sv in svDaDky)
            {
                if (sv.tb_DiemHocPhan.FirstOrDefault(s => (s.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && ((float?)General.getDiemTB(s.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, s.DiemGK, s.DiemCK)) < 5) || (s.DiemCK == General.intRot)) != null)
                {
                    svRot.Add(sv);
                }
            }

            var svRot1Mon = svRot.Where(n => n.tb_DiemHocPhan.Where(s => (s.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && ((float?)General.getDiemTB(s.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, s.DiemGK, s.DiemCK)) < 5) || (s.DiemCK == General.intRot)).Count() == 1);
            var svRot2Mon = svRot.Where(n => n.tb_DiemHocPhan.Where(s => (s.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && ((float?)General.getDiemTB(s.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, s.DiemGK, s.DiemCK)) < 5) || (s.DiemCK == General.intRot)).Count() == 2);
            var svRot3MonTroLen = svRot.Where(n => n.tb_DiemHocPhan.Where(s => (s.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && General.getDiemTB(s.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, s.DiemGK, s.DiemCK) < 5) || (s.DiemCK == General.intRot)).Count() >= 3);
           
            var slSVDky = svDaDky.Count();
            var slSVQuaMon = svQuaMon.Count();
            var slSVRot = svRot.Count();
            var slSVRot1Mon = svRot1Mon.Count();
            var slSVRot2Mon = svRot2Mon.Count();
            var slSVRot3MonTroLen = svRot3MonTroLen.Count();
            Session["DSSV_ROTMON"] = svRot;

            //ViewBag.SVDDK = slSVDky;
            //ViewBag.SVQM = slSVQuaMon;
            //ViewBag.SVR = slSVRot;
            //ViewBag.SVR1 = slSVRot1Mon;
            //ViewBag.SVR2 = slSVRot2Mon;
            //ViewBag.SVR3M = slSVRot3MonTroLen;

            soLuongThongKe sl = new soLuongThongKe();
            sl.slSVDky = slSVDky;
            sl.slSVQuaMon = slSVQuaMon;
            sl.slSVRot = slSVRot;
            sl.slSVRot1Mon = slSVRot1Mon;
            sl.slSVRot2Mon = slSVRot2Mon;
            sl.slSVRot3MonTroLen = slSVRot3MonTroLen;


            return sl;

        }

        public ActionResult Index()
        {
            //Dem so luong sinh vien dang ky hoc phan

            ViewBag.Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");

            soLuongThongKe sl = Load_ThongKe(null, null, null);
            ViewBag.SVDDK = sl.slSVDky;
            ViewBag.SVQM = sl.slSVQuaMon;
            ViewBag.SVR = sl.slSVRot;
            ViewBag.SVR1 = sl.slSVRot1Mon;
            ViewBag.SVR2 = sl.slSVRot2Mon;
            ViewBag.SVR3M = sl.slSVRot3MonTroLen;

            ViewBag.Tile1 = (float)sl.slSVQuaMon / sl.slSVDky * 100;
            ViewBag.Tile2 = (float)sl.slSVRot / sl.slSVDky * 100;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(FormCollection fields)
        {

            ViewBag.Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");

            var id_nganh = int.Parse(fields["Nganh"]);
            var nam = int.Parse(fields["Nam"]);
            var hocKi = int.Parse(fields["HocKi"]);

            soLuongThongKe sl = Load_ThongKe(id_nganh, nam, hocKi);
            ViewBag.SVDDK = sl.slSVDky;
            ViewBag.SVQM = sl.slSVQuaMon;
            ViewBag.SVR = sl.slSVRot;
            ViewBag.SVR1 = sl.slSVRot1Mon;
            ViewBag.SVR2 = sl.slSVRot2Mon;
            ViewBag.SVR3M = sl.slSVRot3MonTroLen;

            ViewBag.Tile1 = (float)sl.slSVQuaMon / sl.slSVDky * 100;
            ViewBag.Tile2 = (float)sl.slSVRot / sl.slSVDky * 100;

            return View();
        }


        public void InFileExcel()
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách");

            string ds = "DANH SÁCH SINH VIÊN HỌC LẠI";
            ws.Cells["A1"].Value = ds;
            ws.Cells["A1"].Style.Font.Bold = true;
            ws.Cells["A1"].Style.Font.Size = 20;
            ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            // Gộp các ô A-Z
            ws.Cells["A1:F1"].Merge = true;

            ws.Cells["A2"].Value = "Ngày lập danh sách:";
            ws.Cells["B2"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A4"].Value = "STT";
            ws.Cells["B4"].Value = "Mã sinh viên";
            ws.Cells["C4"].Value = "Họ tên";
            ws.Cells["D4"].Value = "Chuyên ngành";
            ws.Cells["E4"].Value = "Số môn học rớt";
            ws.Cells["F4"].Value = "Ghi chú";

            ws.Cells["A4:F4"].Style.Font.Bold = true;
            ws.Cells["A4:F4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int row_start = 5;
            int stt = 1;
            List<tb_SinhVien> dsSV = Session["DSSV_ROTMON"] as List<tb_SinhVien>;
            foreach (var item in dsSV)
            {
                ws.Cells["A" + row_start].Value = stt;
                ws.Cells["B" + row_start].Value = item.MaSV;
                ws.Cells["C" + row_start].Value = item.HoTen;
                ws.Cells["D" + row_start].Value = item.tb_Nganh.TenNganh;
                ws.Cells["E" + row_start].Value = item.tb_DiemHocPhan.Where(n => n.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && General.getDiemTB(n.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, n.DiemGK, n.DiemCK) < 5 || n.DiemCK == General.intRot).Count();
                ws.Cells["F" + row_start].Value = item.tb_DiemHocPhan.Where(n => n.tb_HocPhan.tb_MonHoc.XetDiem == General.coXetDiem && General.getDiemTB(n.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, n.DiemGK, n.DiemCK) < 5 || n.DiemCK == General.intRot).Count() >= 3 ? "Học lực yếu" : "Học lực trung bình";
                row_start++;
                stt++;
            }
            string excelName = "DanhSachSinhVienHocLai.xlsx";
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