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

    }
}
