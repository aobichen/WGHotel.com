using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Models
{
    public class HotelViewModel
    {
        public int ID { get; set; }

        public decimal? Sell { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string City { get; set; }
        public string Game { get; set; }

        public string LinkUrl { get; set; }
        public string Tel { get; set; }
        public int Image { get; set; }
    }

    public class HotelDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string City { get; set; }
        public string Game { get; set; }

        public List<string> Facilities { get; set; }

        public string Feature { get; set; }
        public string Address { get; set; }

        public string LinkUrl { get; set; }
        public string Tel { get; set; }
        public List<ImageStore> Images { get; set; }

        public List<RoomViewList> Rooms { get; set; }

        public int ParentId { get; set; }
       
    }
}