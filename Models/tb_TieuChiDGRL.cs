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
    
    public partial class tb_TieuChiDGRL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_TieuChiDGRL()
        {
            this.tb_ChiTietTieuChiDGRL = new HashSet<tb_ChiTietTieuChiDGRL>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_NoiDungDGRL { get; set; }
        public Nullable<int> STT { get; set; }
        public string NoiDung { get; set; }
        public Nullable<int> DiemToiDa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ChiTietTieuChiDGRL> tb_ChiTietTieuChiDGRL { get; set; }
        public virtual tb_NoiDungDGRL tb_NoiDungDGRL { get; set; }
    }
}
