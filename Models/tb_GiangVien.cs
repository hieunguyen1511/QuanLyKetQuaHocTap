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
    
    public partial class tb_GiangVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_GiangVien()
        {
            this.tb_DanhGiaRenLuyen = new HashSet<tb_DanhGiaRenLuyen>();
            this.tb_KhoaHoc = new HashSet<tb_KhoaHoc>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_TaiKhoan { get; set; }
        public string MaGV { get; set; }
        public string HoTen { get; set; }
        public string MaLop_CoVan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DanhGiaRenLuyen> tb_DanhGiaRenLuyen { get; set; }
        public virtual tb_TaiKhoanCap tb_TaiKhoanCap { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_KhoaHoc> tb_KhoaHoc { get; set; }
    }
}
