//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKetQuaHocTap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_DanhGiaRenLuyen
    {
        public int ID { get; set; }
        public Nullable<int> ID_SinhVien { get; set; }
        public Nullable<int> ID_GiangVien { get; set; }
        public Nullable<int> ID_QuanTri { get; set; }
        public string ChuoiDiem { get; set; }
        public Nullable<int> DiemTongSV { get; set; }
        public Nullable<int> DiemTongGV { get; set; }
        public Nullable<int> DiemTongKet { get; set; }
    
        public virtual tb_GiangVien tb_GiangVien { get; set; }
        public virtual tb_QuanTri tb_QuanTri { get; set; }
        public virtual tb_SinhVien tb_SinhVien { get; set; }
    }
}
