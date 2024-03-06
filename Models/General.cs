using System;
using System.Linq;

namespace QuanLyKetQuaHocTap.Models
{
    public class General
    {
        public static db_QuanLyKetQuaHocTapEntities db = new db_QuanLyKetQuaHocTapEntities();
        public static bool gioiTinhNam = true;
        public static bool taiKhoanGV = false;
        public static string strGioiTinhKhongXD = "Không xác định", strGioiTinhNam = "Nam", strGioiTinhNu = "Nữ";
        public static string strTaiKhoanKhongXD = "Không xác định", strTaiKhoanGV = "Giảng viên", strTaiKhoanSV = "Sinh viên";
        public static int intTaiKhoanGV = 0, intTaiKhoanSV = 1, intTaiKhoanQT = 2;
        public static bool daHoanThanhDGRL = true;
        public static string chuaMoHP = "Chưa mở", dangHoatDongHP = "Đang hoạt động", dungHoatDongHP = "Dừng hoạt động";
        public static string XuatSac = "Xuất sắc", Gioi = "Giỏi", Kha = "Khá", TB = "Trung bình", Yeu = "Yếu", Kem = "Kém";
        public static int intLyThuyet = 0, intThucHanh = 1;
        public static bool batBuoc = true;
        public static bool coXetDiem = true;
        public static int intDat = 10, intRot = 0;
        public static string strDat = "DT", strRot = "CD";
        public static string getStrGioiTinh(bool? gioitinh)
        {
            if (gioitinh == null)
            {
                return strGioiTinhKhongXD;
            }
            if (gioitinh == gioiTinhNam)
            {
                return strGioiTinhNam;
            }
            return strGioiTinhNu;
        }
        public static string getStrTaiKhoan(bool? taikhoan)
        {
            if (taikhoan == null)
            {
                return strTaiKhoanKhongXD;
            }
            if (taikhoan == taiKhoanGV)
            {
                return strTaiKhoanGV;
            }
            return strTaiKhoanSV;
        }

        public static string getStrFormatDate(DateTime date)
        {
            return date.Year + "-" + (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString()) + "-" + (date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString());
        }
        public static string getStrFormatDate(DateTime date, char c)
        {
            return (date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString()) + c + (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString()) + c + date.Year;
        }
        public static string getStrFormatDate(DateTime? date)
        {
            DateTime dt = date.GetValueOrDefault();
            return (dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString()) + "/" + (dt.Month < 10 ? "0" + dt.Month.ToString() : dt.Month.ToString()) + "/" + dt.Year;
        }
        public static int getSoLuongSV(int id_co_van)
        {
            return db.tb_SinhVien.Where(n => n.ID_CoVan == id_co_van).Count();
        }

        public static double? getDiemTB(int? trongSoGK, double? diemGK, double? diemCK)
        {
            return diemGK * trongSoGK / 100 + diemCK * (100 - trongSoGK) / 100;
        }
        public static double? getDiemTBHe4(double? diemTB10)
        {
            return diemTB10 * 4 / 10;
        }
        public static string getXepLoaiDiemHP(double? diemtb)
        {
            if (diemtb >= 8.5)
            {
                return "A";
            }
            if (diemtb >= 7.8)
            {
                return "B+";
            }
            if (diemtb >= 7)
            {
                return "B";
            }
            if (diemtb >= 6.3)
            {
                return "C+";
            }
            if (diemtb >= 5.5)
            {
                return "C";
            }
            if (diemtb >= 4.8)
            {
                return "D+";
            }
            if (diemtb >= 4)
            {
                return "D";
            }
            //if (diemtb >= 3)
            //{
            //    return "F+";
            //}
            return "F";
        }


        public static double getPhanTramDGRLHKHT(int? id_gv)
        {
            var sv = db.tb_SinhVien.Where(n => n.tb_CoVanHocTap.ID_GiangVien == id_gv);
            var tongsv = sv.Count();
            var tongSVDaDG = sv.Where(n => n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == n.tb_CoVanHocTap.Nam_HT && s.HocKi == n.tb_CoVanHocTap.HocKi_HT) != null).Count();
            return Math.Round((double)tongSVDaDG * 100 / tongsv, 2);
        }
        public static double getPhanTramDGRL_GV(int? id_gv, string xl)
        {
            var sv = db.tb_DanhGiaRenLuyen.Where(n => n.tb_SinhVien.tb_CoVanHocTap.ID_GiangVien == id_gv);
            var tongsv = sv.Count();
            int minPoint, maxPoint;
            if (xl == XuatSac)
            {
                minPoint = 90;
                maxPoint = 100;
            }
            else if (xl == Gioi)
            {
                minPoint = 80;
                maxPoint = 89;
            }
            else if (xl == Kha)
            {
                minPoint = 65;
                maxPoint = 79;
            }
            else if (xl == TB)
            {
                minPoint = 50;
                maxPoint = 64;
            }
            else if (xl == Yeu)
            {
                minPoint = 35;
                maxPoint = 49;
            }
            else
            {
                minPoint = 0;
                maxPoint = 34;
            }
            var tongsvXL = sv.Where(n => n.DiemTongKhoaDG >= minPoint && n.DiemTongKhoaDG <= maxPoint).Count();
            return Math.Round((double)tongsvXL * 100 / tongsv, 2);
        }
        public static double getPhanTramSVDGRLLoaiGioiDenXuatSac(int id_sv)
        {
            var dsdg = db.tb_DanhGiaRenLuyen.Where(n => n.ID_SinhVien == id_sv);
            var tongdg = dsdg.Count();
            var soLuongDGGioiDenXuatSac = dsdg.Where(n => n.DiemTongKhoaDG >= 80).Count();
            return Math.Round((double)soLuongDGGioiDenXuatSac * 100 / tongdg, 2);
        }
        public static double getPhanTramSVDatHocBongHkHT(int id_gv)
        {
            var dssv = db.tb_SinhVien.Where(n => n.tb_CoVanHocTap.ID_GiangVien == id_gv);
            var tongsv = dssv.Count();
            var soLuongSvDatHocBong = 0;
            foreach (var sv in dssv)
            {
                if (getDiemTBHocKiSV(sv.ID, sv.tb_CoVanHocTap.Nam_HT, sv.tb_CoVanHocTap.HocKi_HT) >= 8 && sv.tb_DanhGiaRenLuyen.FirstOrDefault(n => n.Nam == sv.tb_CoVanHocTap.Nam_HT && n.HocKi == sv.tb_CoVanHocTap.HocKi_HT && n.DiemTongKhoaDG >= 80) != null)
                {
                    soLuongSvDatHocBong++;
                }
            }
            return Math.Round((double)soLuongSvDatHocBong * 100 / tongsv, 2);
        }
        public static double getPhanTramSVDatHocBong(tb_SinhVien sv)
        {
            var tong = sv.tb_DiemHocPhan.Count();
            var soLuongDatHocBong = 0;
            foreach (var d in sv.tb_DiemHocPhan)
            {
                if (getDiemTBHocKiSV(sv.ID, d.NamHoc_DK, d.HocKi_DK) >= 8 && sv.tb_DanhGiaRenLuyen.FirstOrDefault(n => n.Nam == d.NamHoc_DK && n.HocKi == d.HocKi_DK && n.DiemTongKhoaDG >= 80) != null)
                {
                    soLuongDatHocBong++;
                }
            }
            return Math.Round((double)soLuongDatHocBong * 100 / tong, 2);
        }


        public static double? getDiemTBHocKiSV(int id_sv, int? namHT, int? hocKiHT)
        {
            double? tb = 0;
            var diemHP = db.tb_DiemHocPhan.Where(n => n.ID_SinhVien == id_sv && n.NamHoc_DK == namHT && n.HocKi_DK == hocKiHT && n.tb_HocPhan.tb_MonHoc.XetDiem == coXetDiem);
            var tongTC = diemHP.Sum(n => n.tb_HocPhan.tb_MonHoc.SoTC);
            foreach (var diem in diemHP)
            {
                tb += getDiemTB(diem.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, diem.DiemGK, diem.DiemCK) * diem.tb_HocPhan.tb_MonHoc.SoTC;
            }
            return Math.Round((tb / tongTC).GetValueOrDefault(), 2);
        }
        public static double? getDiemTBTichLuySV(int id_sv, int? maxNam, int? maxHocKi)
        {
            double? tb = 0;
            var diemHP = db.tb_DiemHocPhan.Where(n => n.ID_SinhVien == id_sv && n.tb_HocPhan.tb_MonHoc.XetDiem == coXetDiem && ((n.NamHoc_DK == maxNam && n.HocKi_DK <= maxHocKi) || n.NamHoc_DK < maxNam));
            var tongTC = diemHP.Sum(n => n.tb_HocPhan.tb_MonHoc.SoTC);
            foreach (var diem in diemHP)
            {
                tb += getDiemTB(diem.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, diem.DiemGK, diem.DiemCK) * diem.tb_HocPhan.tb_MonHoc.SoTC;
            }
            return Math.Round((tb / tongTC).GetValueOrDefault(), 2);
        }
        public static int? getTongTCHocKiSV(int id_sv, int? namHT, int? hocKiHT)
        {
            var diemHP = db.tb_DiemHocPhan.Where(n => n.ID_SinhVien == id_sv && n.NamHoc_DK == namHT && n.HocKi_DK == hocKiHT && n.tb_HocPhan.tb_MonHoc.XetDiem == coXetDiem);
            var tongTC = diemHP.Sum(n => n.tb_HocPhan.tb_MonHoc.SoTC);
            foreach (var diem in diemHP)
            {
                double? dtb = getDiemTB(diem.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, diem.DiemGK, diem.DiemCK);
                if (dtb < 4)
                {
                    tongTC--;
                }
            }
            return tongTC;
        }
        public static int? getTongTCTichLuySV(int id_sv, int? maxNam, int? maxHocKi)
        {
            var diemHP = db.tb_DiemHocPhan.Where(n => n.ID_SinhVien == id_sv && n.tb_HocPhan.tb_MonHoc.XetDiem == coXetDiem && ((n.NamHoc_DK == maxNam && n.HocKi_DK <= maxHocKi) || n.NamHoc_DK < maxNam));
            var tongTC = diemHP.Sum(n => n.tb_HocPhan.tb_MonHoc.SoTC);
            foreach (var diem in diemHP)
            {
                double? dtb = getDiemTB(diem.tb_HocPhan.tb_MonHoc.TrongSoGiuaKi, diem.DiemGK, diem.DiemCK);
                if (dtb < 4)
                {
                    tongTC--;
                }
            }
            return tongTC;
        }

        public static double getPhanTramSVLoaiGioiDenXuatSacHKHT(int id_gv)
        {
            var dssv = db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.tb_HocPhan.ID_GiangVien == id_gv) != null);
            var tongsv = dssv.Count();
            var soLuongSvGioiDenXuatSac = 0;
            foreach (var sv in dssv)
            {
                if (getDiemTBHocKiSV(sv.ID, sv.tb_CoVanHocTap.Nam_HT, sv.tb_CoVanHocTap.HocKi_HT) >= 8)
                {
                    soLuongSvGioiDenXuatSac++;
                }
            }
            return Math.Round((double)soLuongSvGioiDenXuatSac * 100 / tongsv, 2);
        }
        public static double getPhanTramSVLoaiGioiDenXuatSac(tb_SinhVien sv)
        {
            int tong = sv.tb_DiemHocPhan.Count();
            int slsvXS = 0;
            foreach (var d in sv.tb_DiemHocPhan)
            {
                double? diemtb = getDiemTBHocKiSV(sv.ID, d.NamHoc_DK, d.HocKi_DK);
                if (diemtb >= 8)
                {
                    slsvXS++;
                }
            }
            return Math.Round((double)slsvXS / tong, 2);
        }

        public static double getPhanTramSinhVienXepLoai(int id_gv, string xl)
        {
            var dssv = db.tb_SinhVien.Where(n => n.tb_DiemHocPhan.FirstOrDefault(s => s.tb_HocPhan.ID_GiangVien == id_gv) != null);
            double minPoint, maxPoint;
            if (xl == XuatSac)
            {
                minPoint = 9;
                maxPoint = 10;
            }
            else if (xl == Gioi)
            {
                minPoint = 8;
                maxPoint = 8.9;
            }
            else if (xl == Kha)
            {
                minPoint = 7;
                maxPoint = 7.9;
            }
            else if (xl == TB)
            {
                minPoint = 5;
                maxPoint = 6.9;
            }
            else if (xl == Yeu)
            {
                minPoint = 4;
                maxPoint = 4.9;
            }
            else
            {
                minPoint = 0;
                maxPoint = 3.9;
            }
            var tong = 0;
            var soLuongSVXL = 0;
            foreach (var sv in dssv)
            {
                var dhp = sv.tb_DiemHocPhan.Where(n => n.tb_HocPhan.ID_GiangVien == id_gv);
                tong += dhp.Count();
                foreach (var d in dhp)
                {
                    double? diemtb = getDiemTBHocKiSV(sv.ID, d.NamHoc_DK, d.HocKi_DK);
                    if (diemtb >= minPoint && diemtb <= maxPoint)
                    {
                        soLuongSVXL++;
                    }
                }
            }
            return Math.Round((double)soLuongSVXL * 100 / tong, 2);
        }

        public static double getPhanTramDGRLLH_HKHT(int? id_coVan)
        {
            var sv = db.tb_SinhVien.Where(n => n.ID_CoVan == id_coVan);
            var tongsv = sv.Count();
            var tongSVDaDG = sv.Where(n => n.tb_DanhGiaRenLuyen.FirstOrDefault(s => s.Nam == n.tb_CoVanHocTap.Nam_HT && s.HocKi == n.tb_CoVanHocTap.HocKi_HT) != null).Count();
            return Math.Round((double)tongSVDaDG * 100 / tongsv, 2);
        }
        public static double getPhanTramDGRLLH_CoVan(int? id_coVan, string xl)
        {
            var sv = db.tb_DanhGiaRenLuyen.Where(n => n.tb_SinhVien.ID_CoVan == id_coVan);
            var tongsv = sv.Count();
            int minPoint, maxPoint;
            if (xl == XuatSac)
            {
                minPoint = 90;
                maxPoint = 100;
            }
            else if (xl == Gioi)
            {
                minPoint = 80;
                maxPoint = 89;
            }
            else if (xl == Kha)
            {
                minPoint = 65;
                maxPoint = 79;
            }
            else if (xl == TB)
            {
                minPoint = 50;
                maxPoint = 64;
            }
            else if (xl == Yeu)
            {
                minPoint = 35;
                maxPoint = 49;
            }
            else
            {
                minPoint = 0;
                maxPoint = 34;
            }
            var tongsvXL = sv.Where(n => n.DiemTongKhoaDG >= minPoint && n.DiemTongKhoaDG <= maxPoint).Count();
            return Math.Round((double)tongsvXL * 100 / tongsv, 2);
        }


        public static DateTime getPreviousDate(DateTime date)
        {
            int day = date.Day, month = date.Month, year = date.Year;
            if (day != 1)
            {
                day--;
            }
            else
            {
                if (month != 1)
                {
                    month--;
                }
                else
                {
                    month = 12;
                    year--;
                }
                switch (month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        day = 31;
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        day = 30;
                        break;
                    case 2:
                        if ((year % 100 == 0 && year % 400 == 0) || (year % 100 != 0 && year % 4 == 0))
                        {
                            day = 29;
                        }
                        else
                        {
                            day = 28;
                        }
                        break;
                }
            }
            return new DateTime(year, month, day);
        }

        public static DateTime getNextDate(DateTime date)
        {
            int day = date.Day, month = date.Month, year = date.Year;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day == 31)
                    {
                        day = 1;
                        if (month == 12)
                        {
                            month = 1;
                            year++;
                        }
                        else
                        {
                            month++;
                        }
                    }
                    else
                    {
                        day++;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day == 30)
                    {
                        day = 1;
                        month++;
                    }
                    else
                    {
                        day++;
                    }
                    break;
                case 2:
                    if ((year % 100 == 0 && year % 400 == 0) || (year % 100 != 0 && year % 4 == 0))
                    {
                        if (day == 29)
                        {
                            day = 1;
                            month++;
                        }
                        else
                        {
                            day++;
                        }
                    }
                    else
                    {
                        if (day == 28)
                        {
                            day = 1;
                            month++;
                        }
                        else
                        {
                            day++;
                        }
                    }
                    break;
            }
            return new DateTime(year, month, day);
        }

        public static string getTietHP(int id_mh)
        {
            var mh = db.tb_MonHoc.Find(id_mh);
            if (mh.LoaiMonHoc == intLyThuyet)
            {
                return "(" + mh.SoTC + "+0)";
            }
            return "(0+" + mh.SoTC + ")";
        }

        public static string getStrKhoaVien(tb_GiangVien gv)
        {
            string rs = "";
            foreach (var kv in gv.tb_KhoaVien)
            {
                rs += kv.TenKV + " (" + kv.MaKV + "), ";
            }
            return rs != "" ? rs.Substring(0, rs.Length - 2) : rs;
        }
        public static string getStrCTGD(tb_GiangVien gv)
        {
            string rs = "";
            foreach (var kv in gv.tb_CTGD)
            {
                rs += kv.TenCT + " (" + kv.MaCT + "), ";
            }
            return rs != "" ? rs.Substring(0, rs.Length - 2) : rs;
        }
        public static tb_DanhGiaRenLuyen getNewestDGRL(tb_SinhVien sv)
        {
            return sv.tb_DanhGiaRenLuyen.FirstOrDefault(n => n.Nam == sv.tb_CoVanHocTap.Nam_HT && n.HocKi == sv.tb_CoVanHocTap.HocKi_HT);
        }

        public static int getSoLuongMonCTGD(int id_ctgd)
        {
            return db.tb_MonHoc.Where(n => n.ID_CTGD == id_ctgd).Count();
        }
    }
}