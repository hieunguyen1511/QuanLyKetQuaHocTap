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
    
    public partial class tb_CoVanHocTap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_CoVanHocTap()
        {
            this.tb_SinhVien = new HashSet<tb_SinhVien>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_GiangVien { get; set; }
        public string MaLop { get; set; }
        public Nullable<int> Nam_HT { get; set; }
        public Nullable<int> HocKi_HT { get; set; }
    
        public virtual tb_GiangVien tb_GiangVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_SinhVien> tb_SinhVien { get; set; }
    }
}