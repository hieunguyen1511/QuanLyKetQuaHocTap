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
 
        public ActionResult ChuongTrinhDaoTao()
        {
            return View();
        }

        public ActionResult XemDiem()
        {
            var diemMH = sv.tb_DiemHocPhan;
            return View(diemMH);
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
            int tongdiem = 0;
            foreach (var tc in tcdg)
            {
                var cttcdg = tc.tb_ChiTietTieuChiDGRL;
                foreach (var ct in cttcdg)
                {
                    int newValue = 0;
                    int.TryParse(collection["diemTuDG-" + tc.ID + "-" + ct.ID], out newValue);
                    tb_ChiTietDGRL ctdgrl = General.db.tb_ChiTietDGRL.SingleOrDefault(n => n.ID_ChiTietTieuChiDGRL == ct.ID && n.ID_DanhGiaRenLuyen == id);
                    if (newValue < ct.DiemToiThieu || newValue > ct.DiemToiDa)
                    {
                        tongdiem += ctdgrl.DiemTuDG.GetValueOrDefault();
                        continue;
                    }
                    tongdiem += newValue;
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
                        ctdgrl.DiemCVHT = ctdgrl.DiemKhoa = 0;
                        ctdgrl.DiemTuDG = newValue;
                        General.db.tb_ChiTietDGRL.Add(ctdgrl);
                    }
                    General.db.SaveChanges();
                }
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
