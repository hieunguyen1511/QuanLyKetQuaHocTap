using QuanLyKetQuaHocTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKetQuaHocTap.Areas.Admin.Controllers
{
    public class QuanLyHocPhanController : Controller
    {
        // GET: Admin/QuanLyHocPhan
        public ActionResult Index(string keyword, string tt, int? id_gv, int? id_nganh)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
            IEnumerable<tb_HocPhan> dshp;
            if (string.IsNullOrEmpty(keyword))
            {
                dshp = General.db.tb_HocPhan.ToList();
            }
            else
            {
                string upperKey = keyword.ToUpper();
                dshp = General.db.tb_HocPhan.Where(n => n.MaHP.ToUpper().Contains(upperKey) || n.tb_MonHoc.MaMH.ToUpper().Contains(upperKey) || n.tb_MonHoc.TenMH.ToUpper().Contains(upperKey)).ToList();
                ViewBag.keyword = keyword;
            }
            if (!string.IsNullOrEmpty(tt))
            {
                dshp = General.db.tb_HocPhan.Where(n => n.TrangThai == tt);
                ViewBag.TrangThai = tt;
            }

            var dsgv = General.db.tb_GiangVien.ToList();
            var dsnganh = General.db.tb_Nganh.ToList();
            if (id_gv != null)
            {
                ViewBag.SelectGV = id_gv;
                dshp = dshp.Where(n => n.ID_GiangVien == id_gv);
            }
            if (id_nganh != null)
            {
                ViewBag.SelectNganh = id_nganh;
                dshp = dshp.Where(n => n.tb_MonHoc.ID_Nganh == id_nganh);
            }
            ViewBag.GiangVien = dsgv;
            ViewBag.Nganh = dsnganh;
            ViewBag.MonHoc = General.db.tb_MonHoc.ToList();
            return View(dshp);
        }

        public ActionResult LocGiangVien_Partial(int? id_mh, int? id_gv)
        {
            ViewBag.ID_GV = id_gv;
            IEnumerable<tb_GiangVien> dsgv = General.db.tb_GiangVien;
            if (id_mh != null)
            {
                dsgv = dsgv.Where(n => n.tb_MonHoc.FirstOrDefault(s => s.ID == id_mh) != null);
            }
            return PartialView(dsgv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTimKiem(FormCollection fields)
        {
            string trangThai = fields["TrangThai"];
            int? gv = int.Parse(fields["GV"]);
            if (gv == -1)
            {
                gv = null;
            }
            int? nganh = int.Parse(fields["Nganh"]);

            if (nganh == -1)
            {
                nganh = null;
            }

            return RedirectToAction("Index", new { tt = trangThai, id_gv = gv, id_nganh = nganh });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiem(FormCollection fields)
        {
            return RedirectToAction("Index", new { keyword = fields["KEY"] });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaHocPhan(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var hp = General.db.tb_HocPhan.Find(ID);
            if (hp != null)
            {
                General.db.tb_DiemHocPhan.RemoveRange(hp.tb_DiemHocPhan);
                General.db.tb_HocPhan.Remove(hp);
                try
                {
                    General.db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index", ViewBag.LoiXoaHP = "Không thể xóa học phần này");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult SuaThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_HocPhan hp = General.db.tb_HocPhan.Find(id);
            if (hp == null)
            {
                return HttpNotFound();
            }
            ViewBag.GiangVien = hp.tb_MonHoc.tb_GiangVien.ToList();
            return View(hp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaThongTin(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var hp = General.db.tb_HocPhan.Find(ID);
            if (hp != null)
            {
                hp.TrangThai = fields["TrangThai"];
                hp.SiSo_ToiDa = int.Parse(fields["SiSoToiDa"]);
                hp.NgayBD = DateTime.Parse(fields["ngayBD"]);
                hp.NgayKT = DateTime.Parse(fields["ngayKT"]);
                if (!string.IsNullOrEmpty(fields["GV"]))
                {
                    hp.ID_GiangVien = int.Parse(fields["GV"]);
                }
                General.db.Entry(hp).State = System.Data.Entity.EntityState.Modified;
                General.db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHocPhan(FormCollection fields)
        {
            tb_HocPhan hp = new tb_HocPhan();
            hp.MaHP = "HP" + General.getStrDecimal(General.getMaxIDHocPhan() + 1, 2);
            hp.ID_MonHoc = int.Parse(fields["AddMH"]);
            int? id_gv = int.Parse(fields["AddGV"]);
            if (id_gv != -1)
            {
                hp.ID_GiangVien = id_gv;
            }
            hp.NgayKhoiTao = DateTime.Now;
            hp.SiSo_HienTai = 0;
            hp.SiSo_ToiDa = int.Parse(fields["SiSoToiDa"]);
            hp.NgayBD = DateTime.Parse(fields["ngayBD"]);
            hp.NgayKT = DateTime.Parse(fields["ngayKT"]);
            General.db.tb_HocPhan.Add(hp);
            General.db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Chitiet(int? id)
        {
            var hp = General.db.tb_HocPhan.Find(id);
            ViewBag.DSDiem = hp.tb_DiemHocPhan.ToList();
            return View(hp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSinhVien(FormCollection fields)
        {
            var ID = int.Parse(fields["ID"]);
            var sv = General.db.tb_DiemHocPhan.Find(ID);
            if (sv != null)
            {
                int? id_hocPhan = sv.ID_HocPhan;
                General.db.tb_DiemHocPhan.Remove(sv);
                General.db.SaveChanges();
                return RedirectToAction("Chitiet", new { id = id_hocPhan });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSinhVien(FormCollection fields)
        {
            tb_HocPhan hp = General.db.tb_HocPhan.Find(int.Parse(fields["HP"]));

            var dssv = General.db.tb_SinhVien.Where(n => n.ID_Nganh == hp.tb_MonHoc.ID_Nganh && n.tb_DiemHocPhan.FirstOrDefault(s => s.ID_HocPhan == hp.ID) == null);

            foreach (var sv in dssv)
            {
                if (fields["SV-" + sv.ID] == "on")
                {
                    tb_DiemHocPhan diem = new tb_DiemHocPhan();
                    diem.DiemGK = diem.DiemCK = 0;
                    diem.HocKi_DK = sv.tb_CoVanHocTap.HocKi_HT;
                    diem.NamHoc_DK = sv.tb_CoVanHocTap.Nam_HT;
                    diem.ID_HocPhan = hp.ID;
                    diem.ID_SinhVien = sv.ID;
                    General.db.tb_DiemHocPhan.Add(diem);
                }
            }

            General.db.SaveChanges();
            return RedirectToAction("Chitiet", new { id = hp.ID });
        }
    }
}