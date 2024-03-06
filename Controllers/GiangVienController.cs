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

        public ActionResult HienThiThongTinCaNhan_Partial(int? id)
        {
            var gv = General.db.tb_GiangVien.Find(id);
            return PartialView(gv);
        }

        public ActionResult DanhSachLopHoc()
        {
            var lh = gv.tb_CoVanHocTap;
            return View(lh);
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

        public ActionResult DanhSachSV_Partial(int? id)
        {
            var dssv = General.db.tb_SinhVien.Where(n => n.ID_CoVan == id);
            ViewBag.lh = General.db.tb_CoVanHocTap.Find(id);
            return PartialView(dssv);
        }
        
        public ActionResult DanhSachHocPhan()
        {
            var hp = gv.tb_HocPhan;
            return View(hp);
        }

        public ActionResult XemHocPhan(int? id)
        {
            var hp = General.db.tb_HocPhan.Find(id);
            var dsdk = hp.tb_DiemHocPhan;
            ViewBag.HP = General.db.tb_HocPhan.Find(id);
            return View(dsdk);
        }

        public ActionResult CapNhatDiemHP(int? id, FormCollection collection)
        {
            var hp = General.db.tb_HocPhan.Find(id);
            var dhp = hp.tb_DiemHocPhan;
            foreach (var diem in dhp)
            {
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
    }
}
