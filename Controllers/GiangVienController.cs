using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Controllers
{
    public class GiangVienController : Controller
    {
        // GET: GiangVien
        public static tb_GiangVien gv;

        public ActionResult TrangChu()
        {
<<<<<<< HEAD
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
            string cccd = collection["cccd"];
            tb_GiangVien gv = General.db.tb_GiangVien.Find(id);
            gv.Email = email;
            gv.SDT = sdt;
            gv.NgaySinh = ngaySinh;
            gv.GioiTinh = gioiTinh;
            gv.CCCD = cccd;
            General.db.Entry(gv).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            Session["GV"] = gv;
            GiangVienController.gv = gv;
            return RedirectToAction("ThongTinCaNhan");
        }

        public ActionResult DanhSachLopHoc()
        {
            return View();
        }

        public ActionResult DanhSachLopHoc_Partial(string keyword)
        {
            IEnumerable<tb_CoVanHocTap> cvht;
            if (keyword == null)
            {
                cvht = gv.tb_CoVanHocTap;
            }
            else
            {
                keyword = keyword.ToUpper();
                cvht = gv.tb_CoVanHocTap.Where(n => (n.tb_GiangVien.HoTen.ToUpper().Contains(keyword) || n.tb_GiangVien.MaGV.ToUpper().Contains(keyword) || n.MaLop.ToUpper().Contains(keyword) || n.tb_Nganh.MaNganh.ToUpper().Contains(keyword) || n.tb_Nganh.TenNganh.ToUpper().Contains(keyword)));
            }
            return PartialView(cvht);
        }

        public ActionResult XemLopHocCoVan(int? id)
        {
            var dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id);
            ViewBag.lh = General.db.tb_CoVanHocTap.Find(id);
            return View(dssv);
        }

        public ActionResult XemThongTinSV(int? id)
        {
            var sv = General.db.tb_SinhVien.Find(id);
            return View(sv);
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
                    int newValue;
                    int.TryParse(collection["diemCVHT-" + tc.ID + "-" + ct.ID], out newValue);
                    tb_ChiTietDGRL ctdgrl = General.db.tb_ChiTietDGRL.SingleOrDefault(n => n.ID_ChiTietTieuChiDGRL == ct.ID && n.ID_DanhGiaRenLuyen == id); 
                    if (newValue < ct.DiemToiThieu || newValue > ct.DiemToiDa)
                    {
                        continue;
                    }
                    tongct += newValue;
                    ctdgrl.DiemCVHT = newValue;
                    General.db.Entry(ctdgrl).State = System.Data.Entity.EntityState.Modified;
                    General.db.SaveChanges();
                }
		        if (tongct > tc.DiemToiDa)
                {
                    tongct = tc.DiemToiDa;
                }
                tongdiem += tongct;
            }
            dgrl.DiemTongCVDG = tongdiem;
            General.db.Entry(dgrl).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            gv = General.db.tb_GiangVien.Find(gv.ID);
            return RedirectToAction("XemThongTinSV", new { General.db.tb_DanhGiaRenLuyen.Find(id).tb_SinhVien.ID });
        }

        public ActionResult DanhGiaRenLuyen()
        {
            return View(gv.tb_CoVanHocTap);
        }

        public ActionResult DanhSachSV_Partial(int? id, int type, string keyword)
        {
            IEnumerable<tb_SinhVien> dssv;
            if (type == 0 || type == 2)
            {
                if (keyword == null)
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id);
                }
                else
                {
                    keyword = keyword.ToUpper();
                    dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id && (n.HoTen.ToUpper().Contains(keyword) || n.MaSV.ToUpper().Contains(keyword)));
                }
                ViewBag.lh = General.db.tb_CoVanHocTap.Find(id);
            }
            else
            {
                if (keyword == null)
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_HocPhan == id) != null);
                }
                else
                {
                    dssv = General.db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_HocPhan == id) != null && (n.HoTen.ToUpper().Contains(keyword) || n.MaSV.ToUpper().Contains(keyword)));
                }
                ViewBag.HP = General.db.tb_HocPhan.Find(id);
            }
            ViewBag.Type = type;
            return PartialView(dssv);
        }

        public ActionResult BangThongKeLopHoc()
        {
            var lh = gv.tb_CoVanHocTap;
            return View(lh);
        }

        public ActionResult DanhSachHocPhan()
        {
            var hp = gv.tb_HocPhan;
            return View(hp);
        }

        public ActionResult DanhSachHocPhan_Partial(string keyword)
        {
            IEnumerable<tb_HocPhan> dshp;
            if (keyword == null)
            {
                dshp = gv.tb_HocPhan;
            }
            else
            {
                keyword = keyword.ToUpper();
                dshp = gv.tb_HocPhan.Where(n => (n.MaHP.ToUpper().Contains(keyword) || n.tb_MonHoc.TenMH.ToUpper().Contains(keyword) || n.tb_MonHoc.MaMH.ToUpper().Contains(keyword) || n.tb_GiangVien.HoTen.ToUpper().Contains(keyword) || n.tb_GiangVien.MaGV.ToUpper().Contains(keyword)));
            }
            return PartialView(dshp);
        }

        public ActionResult XemHocPhan(int? id)
        {
            var hp = General.db.tb_HocPhan.Find(id);
            return View(hp);
        }

        public ActionResult CapNhatDiemHP(int? id, FormCollection collection)
        {
            var hp = General.db.tb_HocPhan.Find(id);
            var dhp = hp.tb_DiemHocPhan;
            foreach (var diem in dhp)
            {
                if (string.IsNullOrEmpty(collection["diemGK-" + diem.ID]))
                {
                    continue;
                }
                tb_DiemHocPhan diemhp = General.db.tb_DiemHocPhan.Find(diem.ID);
                if (hp.tb_MonHoc.XetDiem == General.coXetDiem)
                {
                    diemhp.DiemGK = int.Parse(collection["diemGK-" + diem.ID]);
                    if (string.IsNullOrEmpty(collection["diemCK-" + diem.ID]))
                    {
                        diemhp.DiemCK = null;
                    }
                    else
                    {
                        diemhp.DiemCK = int.Parse(collection["diemCK-" + diem.ID]);
                    }
                }
                else
                {
                    int newDiemTK = int.Parse(collection["diemTK-" + diem.ID]);
                    diemhp.DiemCK = newDiemTK;
                }
                General.db.Entry(diemhp).State = System.Data.Entity.EntityState.Modified;
                General.db.SaveChanges();
            }
            gv = General.db.tb_GiangVien.Find(gv.ID);
            return RedirectToAction("XemHocPhan", new { hp.ID });
        }
        public ActionResult BangThongKeHocPhan()
        {
            var hp = gv.tb_HocPhan;
            return View(hp);
        }

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

        public ActionResult XemCTGD(int? id)
        {
            return View(General.db.tb_CTGD.Find(id));
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
=======
            return View();
        }
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
    }
}