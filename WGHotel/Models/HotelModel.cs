using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;

namespace WGHotel.Models
{
    public class HotelModel
    {
        WGHotelsEntities _db { get; set; }
        public HotelModel()
        {
            _db = _db ?? new WGHotelsEntities();
        }
        public List<HotelViewModel> MinPirceOfCalendar(List<HotelViewModel> model, DateTime CheckInDate)
        {
            CheckInDate = CheckInDate.Date;
            var result = new List<HotelViewModel>();
            foreach (var item in model)
            {
                var Hotel = _db.HotelEN.Find(item.ID);
                
               
                var Room = _db.RoomEN.Where(o => o.HOTELID == item.ID).Select(o => o.ID).ToList();
                #region *** 搜尋後的價格 ***
                if (Room != null && Room.Count > 0)
                {

                    var HasRoomPrice = _db.RoomPrice.Where(
                        o => Room.Contains(o.ROOMID) && o.SaleOff == true && (
                         DateTime.Compare(o.Date, CheckInDate) == 0)
                        ).OrderBy(o => o.Price).FirstOrDefault();
                    if (HasRoomPrice == null)
                    {
                        result.Add(new HotelViewModel
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Game = item.Game,
                            Sell = Hotel.RoomEN.Min(o => o.Sell),
                            Tel = item.Tel,
                            LinkUrl = item.LinkUrl
                        });
                    }
                    else
                    {
                        if (HasRoomPrice.SaleOff == true)
                        {
                            result.Add(new HotelViewModel
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Game = item.Game,
                                Sell = HasRoomPrice.Price,
                                Tel = item.Tel,
                                LinkUrl = item.LinkUrl
                            });
                        }
                    }
                }
                #endregion

                
            }
            return result;
        }
    }
}