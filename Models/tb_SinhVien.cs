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
    
    public partial class tb_SinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_SinhVien()
        {
            this.tb_DangKyMon = new HashSet<tb_DangKyMon>();
            this.tb_DanhGiaRenLuyen = new HashSet<tb_DanhGiaRenLuyen>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_Nganh { get; set; }
        public Nullable<int> ID_TaiKhoan { get; set; }
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string MaLop { get; set; }
        public Nullable<int> Nam_HT { get; set; }
        public Nullable<int> HocKi_HT { get; set; }
        public Nullable<int> Cap_NNKC { get; set; }
        public Nullable<int> Cap_KyNang { get; set; }
        public Nullable<bool> Bang_NNKC { get; set; }
        public Nullable<bool> Bang_KyNang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DangKyMon> tb_DangKyMon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DanhGiaRenLuyen> tb_DanhGiaRenLuyen { get; set; }
        public virtual tb_Nganh tb_Nganh { get; set; }
        public virtual tb_TaiKhoanCap tb_TaiKhoanCap { get; set; }
    }
}
