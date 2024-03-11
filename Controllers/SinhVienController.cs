using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class SinhVienController : Controller
    {
        // GET: SinhVien
        public static tb_SinhVien sv;

        public ActionResult LopHocCoVan()
        {
            return View(sv.tb_CoVanHocTap);
        }

        public ActionResult ThoiKhoaBieu(DateTime? date, int? hocki, int? nam)
        {
            ViewBag.HocKi = hocki.GetValueOrDefault();
            ViewBag.Nam = nam.GetValueOrDefault();
            if (date == null)
            {
                date = DateTime.Now;
            }
            ViewBag.Date = date;
            return View();
        }

        public ActionResult TKB_Partial(int day, int month, int year, int hocki, int nam)
        {
            var ttdk = General.db.tb_ThongTinBuoiHoc.Where(n => n.tb_HocPhan.tb_DiemHocPhan.FirstOrDefault(s => s.ID_SinhVien == sv.ID) != null);

            var tmp = ttdk.Where(n => n.tb_HocPhan.tb_DiemHocPhan.FirstOrDefault(s => s.HocKi_DK == hocki && s.NamHoc_DK == nam) != null);

            if (tmp.Count() == 0)
            {
                hocki = ttdk.Max(n => n.tb_HocPhan.tb_DiemHocPhan.Max(s => s.HocKi_DK)).GetValueOrDefault();
                nam = ttdk.Max(n => n.tb_HocPhan.tb_DiemHocPhan.Max(s => s.NamHoc_DK)).GetValueOrDefault();
            }

            ttdk = ttdk.Where(n => n.tb_HocPhan.tb_DiemHocPhan.FirstOrDefault(s => s.NamHoc_DK == nam && s.HocKi_DK == hocki) != null);

            DateTime minDate = ttdk.Min(n => n.NgayHoc).GetValueOrDefault();
            DateTime maxDate = ttdk.Max(n => n.NgayHoc).GetValueOrDefault();
            while (minDate.DayOfWeek != DayOfWeek.Monday)
            {
                minDate = General.getPreviousDate(minDate);
            }
            while (maxDate.DayOfWeek != DayOfWeek.Sunday)
            {
                maxDate = General.getNextDate(maxDate);
            }
            DateTime first, last, date = new DateTime(year, month, day);
            if (date >= minDate && date <= maxDate)
            {
                first = new DateTime(date.Year, date.Month, date.Day);
                last = new DateTime(first.Year, first.Month, first.Day);
            }
            else
            {
                first = ttdk.Max(n => n.NgayHoc).GetValueOrDefault();
                last = new DateTime(first.Year, first.Month, first.Day);
            }

            while (first.DayOfWeek != DayOfWeek.Monday)
            {
                first = General.getPreviousDate(first);
            }
            while (last.DayOfWeek != DayOfWeek.Sunday)
            {
                last = General.getNextDate(last);
            }
            ViewBag.FirstDate = first;
            ViewBag.LastDate = last;
            ViewBag.MinDate = minDate;
            ViewBag.MaxDate = maxDate;
            ViewBag.HocKi = hocki;
            ViewBag.Nam = nam;
            
            return PartialView(ttdk.Where(n => n.tb_HocPhan.tb_DiemHocPhan.FirstOrDefault(s => s.NamHoc_DK == nam && s.HocKi_DK == hocki && n.NgayHoc >= first && n.NgayHoc <= last) != null));
        }
        public ActionResult BlockHocPhan_Partial(IEnumerable<tb_ThongTinBuoiHoc> tt, int tiet)
        {
            ViewBag.Tiet = tiet;
            return PartialView(tt);
        }

        public ActionResult DanhSachSV_Partial(int id, int type, string keyword)
        {
            IEnumerable<tb_SinhVien> dssv;
            if (type == 0)
            {
                if (keyword == null)
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_HocPhan == id) != null);
                }
                else
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_HocPhan == id) != null && (n.HoTen.Contains(keyword) || n.MaSV.Contains(keyword)));
                }
            }
            else
            {
                if (keyword == null)
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id);
                }
                else
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id && (n.HoTen.Contains(keyword) || n.MaSV.Contains(keyword)));
                }
            }
            return PartialView(dssv);
        }

    }
}
