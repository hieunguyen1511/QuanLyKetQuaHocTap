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
            int tongdiem = 0;
            foreach (var tc in tcdg)
            {
                var cttcdg = tc.tb_ChiTietTieuChiDGRL;
                foreach (var ct in cttcdg)
                {
                    int newValue;
                    int.TryParse(collection["diemCVHT-" + tc.ID + "-" + ct.ID], out newValue);
                    tb_ChiTietDGRL ctdgrl = General.db.tb_ChiTietDGRL.SingleOrDefault(n => n.ID_ChiTietTieuChiDGRL == ct.ID && n.ID_DanhGiaRenLuyen == id); 
                    if (newValue < ct.DiemToiThieu || newValue > ct.DiemToiDa)
                    {
                        tongdiem += ctdgrl.DiemCVHT.GetValueOrDefault();
                        continue;
                    }
                    tongdiem += newValue;
                    if (ctdgrl != null)
                    {
                        ctdgrl.DiemCVHT = newValue;
                        General.db.Entry(ctdgrl).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        ctdgrl = new tb_ChiTietDGRL();
                        ctdgrl.ID_DanhGiaRenLuyen = id;
                        ctdgrl.ID_ChiTietTieuChiDGRL = ct.ID;
                        ctdgrl.DiemCVHT = newValue;
                        ctdgrl.DiemTuDG = ctdgrl.DiemKhoa = 0;
                        General.db.tb_ChiTietDGRL.Add(ctdgrl);
                    }
                    General.db.SaveChanges();
                }
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
                    int newDiemGK = int.Parse(collection["diemGK-" + diem.ID]);
                    int newDiemCK = int.Parse(collection["diemCK-" + diem.ID]);
                    diemhp.DiemGK = newDiemGK;
                    diemhp.DiemCK = newDiemCK;
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

        public ActionResult XemCTGD(int? id)
        {
            return View(General.db.tb_CTGD.Find(id));
        }
    }
}
