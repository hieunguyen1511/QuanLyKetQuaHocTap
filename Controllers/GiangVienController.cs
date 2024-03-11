using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuanLyKetQuaHocTap.Models;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class GiangVienController : Controller
    {
        // GET: GiangVien
        public static tb_GiangVien gv;

        public ActionResult LichGiangDay(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            ViewBag.Date = date;
            return View();
        }

        public ActionResult Lich_Partial(int day, int month, int year)
        {
            DateTime firstDate = new DateTime(year, month, day);
            while (firstDate.DayOfWeek != DayOfWeek.Monday)
            {
                firstDate = General.getPreviousDate(firstDate);
            }
            DateTime lastDate = new DateTime(year, month, day);
            while (lastDate.DayOfWeek != DayOfWeek.Sunday)
            {
                lastDate = General.getNextDate(lastDate);
            }
            ViewBag.FirstDate = firstDate;
            ViewBag.LastDate = lastDate;
            var dsbh = General.db.tb_ThongTinBuoiHoc.Where(n => n.NgayHoc >= firstDate && n.NgayHoc <= lastDate).OrderBy(n => n.NgayHoc);
            return PartialView(dsbh);
        }
        
        public ActionResult BlockHocPhan_Partial(IEnumerable<tb_ThongTinBuoiHoc> tt, int tiet)
        {
            ViewBag.Tiet = tiet;
            return PartialView(tt);
        }

        public ActionResult ChuongTrinhCuaBan()
        {
            return View(gv.tb_CTGD);
        }
        public ActionResult ChuongTrinhGV_Partial(int type, string keyword)
        {
            IEnumerable<tb_CTGD> ctgd;
            if (type == 0)
            {
                if (keyword != null)
                {
                    keyword = keyword.ToLower();
                    ctgd = gv.tb_CTGD.Where(n => (n.MaCT.ToLower().Contains(keyword) || n.TenCT.ToLower().Contains(keyword)));
                }
                else
                {
                    ctgd = gv.tb_CTGD;
                }
            }
            else
            {
                if (keyword != null)
                {
                    keyword = keyword.ToLower();
                    ctgd = General.db.tb_CTGD.Where(n => (n.MaCT.ToLower().Contains(keyword) || n.TenCT.ToLower().Contains(keyword)));
                }
                else
                {
                    ctgd = General.db.tb_CTGD;
                }
            }
            ViewBag.Type = type;
            return PartialView(ctgd);
        }

        public ActionResult ChuongTrinhGiangDay()
        {
            return View(gv.tb_CTGD);
        }
	public ActionResult DanhSachMonHoc_Partial(int? id, string keyword)
        {
            if (keyword == null)
            {
                return PartialView(General.db.tb_CTGD.Find(id).tb_MonHoc);
            }
            keyword = keyword.ToUpper();
            return PartialView(General.db.tb_CTGD.Find(id).tb_MonHoc.Where(n => (n.TenMH.ToUpper().Contains(keyword) || n.MaMH.ToUpper().Contains(keyword))));
        }
    }
}
