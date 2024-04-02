using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class QuanLyRenLuyenController : Controller
    {
        public static List<TieuChiDanhGia_temp> tieuChis = new List<TieuChiDanhGia_temp>();
        // GET: Admin/QuanLyRenLuyen
        public ActionResult Index(string keyword, int? id_nganh)
        {
            IEnumerable<tb_CoVanHocTap> cvht;
            if (string.IsNullOrEmpty(keyword))
            {
                cvht = General.db.tb_CoVanHocTap.ToList();
            }
            else
            {
                string upperKey = keyword.ToUpper();
                cvht = General.db.tb_CoVanHocTap.Where(n => n.MaLop.ToUpper().Contains(upperKey) || n.tb_GiangVien.MaGV.ToUpper().Contains(upperKey) || n.tb_GiangVien.HoTen.ToUpper().Contains(upperKey)).ToList();
                ViewBag.keyword = keyword;
            }

            ViewBag.Nganh = General.db.tb_Nganh.ToList();
            
            if (id_nganh != null)
            {
                ViewBag.SelectNganh = id_nganh;
                cvht = cvht.Where(n => n.ID_Nganh == id_nganh);
            }
            return View(cvht);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiemLH(FormCollection fields)
        {
            int? id_nganh = int.Parse(fields["Nganh"]);
            if (id_nganh == -1)
            {
                id_nganh = null;
            }
            return RedirectToAction("Index", new { id_nganh = id_nganh });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiemLH(FormCollection fields)
        {
            return RedirectToAction("Index", new { keyword = fields["KEY"] });
        }
        public ActionResult ChitietLH(int? id, bool? tt, string keyword)
        {
            var lh = General.db.tb_CoVanHocTap.Find(id);
            List<tb_SinhVien> dssv;
            if (tt == true)
            {
                dssv = lh.tb_SinhVien.Where(n => n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == lh.Nam_HT && s.HocKi == lh.HocKi_HT) != null && n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == lh.Nam_HT && s.HocKi == lh.HocKi_HT).DiemTongTuDG != null).ToList();
            }
            else if (tt == false)
            {
                dssv = lh.tb_SinhVien.Where(n => n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == lh.Nam_HT && s.HocKi == lh.HocKi_HT) == null || n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == lh.Nam_HT && s.HocKi == lh.HocKi_HT).DiemTongTuDG == null).ToList();
            }
            else
            {
                dssv = lh.tb_SinhVien.ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                string upperKeyword = keyword.ToUpper();
                dssv = dssv.Where(n => n.MaSV.ToUpper().Contains(upperKeyword) || n.HoTen.ToUpper().Contains(upperKeyword)).ToList();
                ViewBag.Keyword = keyword;
            }
            ViewBag.DSSV = dssv;
            ViewBag.TrangThai = tt;
            return View(lh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiemSV(FormCollection fields)
        {
            return RedirectToAction("ChitietLH", new { id = int.Parse(fields["ID"]), keyword = fields["KEY"] });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiemSV(FormCollection fields)
        {
            int id_cv = int.Parse(fields["ID"]);
            bool? tt = null;
            if (!string.IsNullOrEmpty(fields["TrangThai"]))
            {
                tt = bool.Parse(fields["TrangThai"]);
            }
            return RedirectToAction("ChitietLH", new { id = id_cv, tt = tt });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhGiaRenLuyen(FormCollection fields)
        {
            var dg = General.db.tb_DanhGiaRenLuyen.Find(int.Parse(fields["ID"]));
            var nddg = dg.tb_NoiDungDGRL;
            int? tongDiem = 0;
            var tcdg = nddg.tb_TieuChiDGRL;
            foreach (var tc in tcdg)
            {
                var cttcdg = tc.tb_ChiTietTieuChiDGRL;
                int? tongct = 0;
                foreach (var ct in cttcdg)
                {
                    tb_ChiTietDGRL chitiet = dg.tb_ChiTietDGRL.FirstOrDefault(n => n.ID_ChiTietTieuChiDGRL == ct.ID);
                    int newDiem = 0;
                    int.TryParse(fields["diemKhoa-" + tc.ID + "-" + ct.ID], out newDiem);
                    if (newDiem < ct.DiemToiThieu || newDiem > ct.DiemToiDa)
                    {
                        continue;
                    }
                    tongct += newDiem;
                    chitiet.DiemKhoa = newDiem;
                    
                    General.db.Entry(chitiet).State = System.Data.Entity.EntityState.Modified;
                }
                if (tongct > tc.DiemToiDa)
                {
                    tongct = tc.DiemToiDa;
                }
                tongDiem += tongct;
            }
            dg.DiemTongKhoaDG = tongDiem;
            General.db.Entry(dg).State = System.Data.Entity.EntityState.Modified;
            General.db.SaveChanges();
            return RedirectToAction("ChiTietLH", new { id = dg.tb_SinhVien.ID_CoVan });
        }

        public ActionResult QuanLyNoiDungRenLuyen()
        {
            var ndHT = General.db.tb_NoiDungDGRL.OrderByDescending(n => n.ID).FirstOrDefault();
            ViewBag.NoiDung = General.db.tb_NoiDungDGRL.ToList();
            return View(ndHT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiNoiDung(FormCollection fields)
        {
            var tcs = fields["tc"].Split('|');
            int length = int.Parse(fields["length"]);
            tb_NoiDungDGRL nd = new tb_NoiDungDGRL();
            nd.NgayTao = DateTime.Now;
            General.db.tb_NoiDungDGRL.Add(nd);
            General.db.SaveChanges();
            for (int i = 0; i < length; i++)
            {
                var tmp = tcs[i].Split(',');
                if (tmp.Length != 3)
                {
                    continue;
                }
                int stt_tc = i + 1;
                tb_TieuChiDGRL tieuChi = new tb_TieuChiDGRL();
                tieuChi.ID_NoiDungDGRL = nd.ID;
                tieuChi.STT = stt_tc;
                tieuChi.NoiDung = tmp[1];
                tieuChi.DiemToiDa = int.Parse(tmp[2]);
                General.db.tb_TieuChiDGRL.Add(tieuChi);
                General.db.SaveChanges();
                var cts = fields["ct-tc-" + stt_tc].Split('|');
                for (int j = 0; j < cts.Length; j++)
                {
                    var tmp2 = cts[j].Split(',');
                    if (tmp2.Length != 4)
                    {
                        continue;
                    }
                    int stt = j + 1;
                    tb_ChiTietTieuChiDGRL chitiet = new tb_ChiTietTieuChiDGRL();
                    chitiet.ID_TieuChiDGRL = tieuChi.ID;
                    chitiet.STT = stt;
                    chitiet.NoiDung = tmp2[1];
                    chitiet.DiemToiThieu = int.Parse(tmp2[2]);
                    chitiet.DiemToiDa = int.Parse(tmp2[3]);
                    General.db.tb_ChiTietTieuChiDGRL.Add(chitiet);
                }
                if (cts.Length != 0)
                {
                    General.db.SaveChanges();
                }
            }
            return RedirectToAction("QuanLyNoiDungRenLuyen");
        }


    }
}