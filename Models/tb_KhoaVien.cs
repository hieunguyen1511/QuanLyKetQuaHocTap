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
    
    public partial class tb_KhoaVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_KhoaVien()
        {
            this.tb_Nganh = new HashSet<tb_Nganh>();
            this.tb_GiangVien = new HashSet<tb_GiangVien>();
        }
    
        public int ID { get; set; }
        public string MaKV { get; set; }
        public string TenKV { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_Nganh> tb_Nganh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_GiangVien> tb_GiangVien { get; set; }
    }
}
