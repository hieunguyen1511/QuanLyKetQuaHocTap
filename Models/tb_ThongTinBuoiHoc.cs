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
    
    public partial class tb_ThongTinBuoiHoc
    {
        public int ID { get; set; }
        public Nullable<int> ID_KhoaHoc { get; set; }
        public string MaPH { get; set; }
        public Nullable<System.DateTime> NgayHoc { get; set; }
    
        public virtual tb_KhoaHoc tb_KhoaHoc { get; set; }
    }
}
