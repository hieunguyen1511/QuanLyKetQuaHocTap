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
            string dantoc = collection["danToc"];
            string cccd = collection["cccd"];
            tb_SinhVien sv = General.db.tb_SinhVien.Find(id);
            sv.Email = email;
            sv.SDT = sdt;
            sv.NgaySinh = ngaySinh;
            sv.GioiTinh = gioiTinh;
            sv.DanToc = dantoc;
            sv.CCCD = cccd;
            General.db.Entry(sv).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            Session["SV"] = sv;
            SinhVienController.sv = sv;
            return RedirectToAction("ThongTinCaNhan");
        }

        public ActionResult LopHocCoVan()
        {
            return View(sv.tb_CoVanHocTap);
        }


        public ActionResult ChuongTrinhDaoTao()
        {
            return View();
        }

        public ActionResult XemDiem()
        {
            var diemMH = sv.tb_DiemHocPhan;
            return View(diemMH);
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

        public ActionResult DanhGiaRenLuyen()
        {
            return View(General.db.tb_DanhGiaRenLuyen.Where(n => n.ID_SinhVien == sv.ID));
        }
        public ActionResult HienThiDGRL_Partial(int? id, int loaiTK)
        {
            var dgrl = General.db.tb_DanhGiaRenLuyen.Find(id);
            ViewBag.LoaiTK = loaiTK;
            return PartialView(dgrl);
        }

        public ActionResult CapNhatDGRL(int? id, FormCollection collection)
        {
            var dgrl = General.db.tb_DanhGiaRenLuyen.Find(id);

            var tcdg = dgrl.tb_NoiDungDGRL.tb_TieuChiDGRL;
            int? tongdiem = 0;
            foreach (var tc in tcdg)
            {
                var cttcdg = tc.tb_ChiTietTieuChiDGRL;
		        int? tongct = 0;
                foreach (var ct in cttcdg)
                {
                    int newValue = 0;
                    int.TryParse(collection["diemTuDG-" + tc.ID + "-" + ct.ID], out newValue);
                    tb_ChiTietDGRL ctdgrl = General.db.tb_ChiTietDGRL.SingleOrDefault(n => n.ID_ChiTietTieuChiDGRL == ct.ID && n.ID_DanhGiaRenLuyen == id);
                    if (newValue < ct.DiemToiThieu || newValue > ct.DiemToiDa)
                    {
                        continue;
                    }
                    tongct += newValue;
                    if (ctdgrl != null)
                    {
                        ctdgrl.DiemTuDG = newValue;
                        General.db.Entry(ctdgrl).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        ctdgrl = new tb_ChiTietDGRL();
                        ctdgrl.ID_DanhGiaRenLuyen = id;
                        ctdgrl.ID_ChiTietTieuChiDGRL = ct.ID;
                        ctdgrl.DiemTuDG = newValue;
                        General.db.tb_ChiTietDGRL.Add(ctdgrl);
                    }
                    General.db.SaveChanges();
                }
		        if (tongct > tc.DiemToiDa)
                {
                    tongct = tc.DiemToiDa;
                }
                tongdiem += tongct;
            }
            dgrl.DiemTongTuDG = tongdiem;
            General.db.Entry(dgrl).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            sv = General.db.tb_SinhVien.Find(sv.ID);
            return RedirectToAction("DanhGiaRenLuyen");
        }


        public ActionResult HocPhanDangKy()
        {
            var hp = General.db.tb_HocPhan.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_SinhVien == sv.ID && s.NamHoc_DK == sv.tb_CoVanHocTap.Nam_HT && s.HocKi_DK == sv.tb_CoVanHocTap.HocKi_HT) != null);

            ViewBag.HPCH = General.db.tb_HocPhan.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_SinhVien == sv.ID && (s.NamHoc_DK != sv.tb_CoVanHocTap.Nam_HT || s.HocKi_DK != sv.tb_CoVanHocTap.HocKi_HT)) != null);

            return View(hp);
        }

        public ActionResult DanhSachHocPhan_Partial(int? nam, int? hocKi, string keyword)
        {
            if (keyword == null)
            {
                return PartialView(General.db.tb_HocPhan.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_SinhVien == sv.ID && s.NamHoc_DK == nam && s.HocKi_DK == hocKi) != null));
            }
            else
            {
                return PartialView(General.db.tb_HocPhan.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_SinhVien == sv.ID && s.NamHoc_DK == nam && s.HocKi_DK == hocKi) != null && (n.MaHP.Contains(keyword) || n.tb_MonHoc.TenMH.Contains(keyword) || n.tb_GiangVien.HoTen.Contains(keyword) || n.tb_GiangVien.MaGV.Contains(keyword) || n.tb_GiangVien.MaGV.Contains(keyword))));
            }
        }

        public ActionResult XemHocPhan(int? id)
        {
            return View(General.db.tb_HocPhan.Find(id));
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