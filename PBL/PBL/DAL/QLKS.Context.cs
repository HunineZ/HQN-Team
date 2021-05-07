﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PBL.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QLKS : DbContext
    {
        public QLKS()
            : base("name=QLKS")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BOOK> BOOKs { get; set; }
        public virtual DbSet<CHUCVU> CHUCVUs { get; set; }
        public virtual DbSet<DANGNHAP> DANGNHAPs { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<HOADON_DUNG_DICHVU> HOADON_DUNG_DICHVU { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<LOAIDICHVU> LOAIDICHVUs { get; set; }
        public virtual DbSet<LOAIPHONG> LOAIPHONGs { get; set; }
        public virtual DbSet<LOAIVATDUNG> LOAIVATDUNGs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<PHONG> PHONGs { get; set; }
        public virtual DbSet<TRANGTHAIBOOK> TRANGTHAIBOOKs { get; set; }
        public virtual DbSet<TRANGTHAIVATDUNG> TRANGTHAIVATDUNGs { get; set; }
        public virtual DbSet<VATDUNGPHONG> VATDUNGPHONGs { get; set; }
    
        [DbFunction("QLKS", "func_XemChiTietHoaDon_DichVu")]
        public virtual IQueryable<func_XemChiTietHoaDon_DichVu_Result> func_XemChiTietHoaDon_DichVu(string hoadonid)
        {
            var hoadonidParameter = hoadonid != null ?
                new ObjectParameter("hoadonid", hoadonid) :
                new ObjectParameter("hoadonid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<func_XemChiTietHoaDon_DichVu_Result>("[QLKS].[func_XemChiTietHoaDon_DichVu](@hoadonid)", hoadonidParameter);
        }
    
        [DbFunction("QLKS", "func_XemChiTietHoaDon_VatTu")]
        public virtual IQueryable<func_XemChiTietHoaDon_VatTu_Result> func_XemChiTietHoaDon_VatTu(string hoadonid)
        {
            var hoadonidParameter = hoadonid != null ?
                new ObjectParameter("hoadonid", hoadonid) :
                new ObjectParameter("hoadonid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<func_XemChiTietHoaDon_VatTu_Result>("[QLKS].[func_XemChiTietHoaDon_VatTu](@hoadonid)", hoadonidParameter);
        }
    
        public virtual int sp_Cal_HoaDon(string bookid)
        {
            var bookidParameter = bookid != null ?
                new ObjectParameter("bookid", bookid) :
                new ObjectParameter("bookid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_Cal_HoaDon", bookidParameter);
        }
    }
}
