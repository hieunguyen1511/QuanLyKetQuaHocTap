﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QuanLyKetQuaHocTapEntities : DbContext
    {
        public QuanLyKetQuaHocTapEntities()
            : base("name=QuanLyKetQuaHocTapEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_DangKyMon> tb_DangKyMon { get; set; }
        public virtual DbSet<tb_DanhGiaRenLuyen> tb_DanhGiaRenLuyen { get; set; }
        public virtual DbSet<tb_GiangVien> tb_GiangVien { get; set; }
        public virtual DbSet<tb_KhoaHoc> tb_KhoaHoc { get; set; }
        public virtual DbSet<tb_MonHoc> tb_MonHoc { get; set; }
        public virtual DbSet<tb_Nganh> tb_Nganh { get; set; }
        public virtual DbSet<tb_QuanTri> tb_QuanTri { get; set; }
        public virtual DbSet<tb_SinhVien> tb_SinhVien { get; set; }
        public virtual DbSet<tb_TaiKhoanCap> tb_TaiKhoanCap { get; set; }
        public virtual DbSet<tb_ThongTinBuoiHoc> tb_ThongTinBuoiHoc { get; set; }
    }
}
