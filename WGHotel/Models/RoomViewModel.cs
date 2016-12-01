using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Models
{
    public class RoomViewList
    {
        public int ID { get; set; }

        public string LinkUrl {get;set;}
        public int HOTELID { get; set; }
        public string Name { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }
        public List<string> Facilities { get; set; }
        public bool HasBreakfast { get; set; }

        
        public Nullable<decimal> Sell { get; set; }

        public string Feature { get; set; }
        public string Notice { get; set; }
        public Nullable<int> Quantiy { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public Nullable<int> ParentId { get; set; }

        public List<ImageStore> Images { get; set; }
    }
}