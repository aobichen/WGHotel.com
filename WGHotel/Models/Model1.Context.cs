﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WGHotel.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WGHotelsEntities : DbContext
    {
        public WGHotelsEntities()
            : base("name=WGHotelsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<CityEN> CityEN { get; set; }
        public virtual DbSet<CityZH> CityZH { get; set; }
        public virtual DbSet<CodeFileEN> CodeFileEN { get; set; }
        public virtual DbSet<CodeFileZH> CodeFileZH { get; set; }
        public virtual DbSet<FacilityEN> FacilityEN { get; set; }
        public virtual DbSet<FacilityZH> FacilityZH { get; set; }
        public virtual DbSet<GameSiteEN> GameSiteEN { get; set; }
        public virtual DbSet<GameSiteZH> GameSiteZH { get; set; }
        public virtual DbSet<HotelEN> HotelEN { get; set; }
        public virtual DbSet<HotelZH> HotelZH { get; set; }
        public virtual DbSet<ImageStore> ImageStore { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportRooms> ReportRooms { get; set; }
        public virtual DbSet<RoomEN> RoomEN { get; set; }
        public virtual DbSet<RoomZH> RoomZH { get; set; }
        public virtual DbSet<SysSetting> SysSetting { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<RoomPrice> RoomPrice { get; set; }
        public virtual DbSet<VenueEN> VenueEN { get; set; }
        public virtual DbSet<VenueZH> VenueZH { get; set; }
    }
}
