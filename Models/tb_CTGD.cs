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
    
    public partial class tb_CTGD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_CTGD()
        {
<<<<<<< HEAD
            this.tb_MonHoc = new HashSet<tb_MonHoc>();
=======
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
            this.tb_GiangVien = new HashSet<tb_GiangVien>();
        }
    
        public int ID { get; set; }
        public string MaCT { get; set; }
        public string TenCT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
<<<<<<< HEAD
        public virtual ICollection<tb_MonHoc> tb_MonHoc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
=======
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
        public virtual ICollection<tb_GiangVien> tb_GiangVien { get; set; }
    }
}