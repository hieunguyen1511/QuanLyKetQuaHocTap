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
    
    public partial class tb_TaiKhoanCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_TaiKhoanCap()
        {
            this.tb_GiangVien = new HashSet<tb_GiangVien>();
            this.tb_SinhVien = new HashSet<tb_SinhVien>();
        }
    
        public int ID { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
        public Nullable<int> ID_QuanTri_Cap { get; set; }
        public Nullable<System.DateTime> NgayCap { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_GiangVien> tb_GiangVien { get; set; }
        public virtual tb_QuanTri tb_QuanTri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_SinhVien> tb_SinhVien { get; set; }
    }
}
