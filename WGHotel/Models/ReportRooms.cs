//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ReportRooms
    {
        public int ID { get; set; }
        public int ReportID { get; set; }
        public string RoomName { get; set; }
        public int Amount { get; set; }
        public int RoomID { get; set; }
    
        public virtual Report Report { get; set; }
    }
}
