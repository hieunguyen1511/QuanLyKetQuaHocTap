using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKetQuaHocTap.Models
{
    public class TieuChiDanhGia_temp
    {
        public int stt;
        public string noiDung;
        public int diemToiDa;
        public class ChiTietTieuChi
        {
            public int stt;
            public string noiDung;
            public int diemToiThieu;
            public int diemToiDa;
        }
        public List<ChiTietTieuChi> chitietTieuChis;

        public TieuChiDanhGia_temp(int stt, string noiDung, int diemToiDa)
        {
            this.stt = stt;
            this.noiDung = noiDung;
            this.diemToiDa = diemToiDa;
            chitietTieuChis = new List<ChiTietTieuChi>();
        }
    }
}