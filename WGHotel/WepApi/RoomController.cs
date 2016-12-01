using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WGHotel.Areas.Backend.Models;
using WGHotel.Models;
using Newtonsoft.Json;

namespace WGHotel.WepApi
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool Off { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Quantity { get; set; }
    }

    public class CalendarPost
    {
        public DateTime date { get; set; }
        public decimal price { get; set; }
        public bool off { get; set; }

        public int quantity { get; set; }

        public int roomid { get; set; }
    }
    public class RoomController : ApiController
    {
        private WGHotelsEntities _db = new WGHotelsEntities();
        public RoomController()
        {
            if (_db == null)
            {
                _db = new WGHotelsEntities();
            }
        }
        [HttpPost]
        [Route("GetRoomPrice/{id}")]
        public List<CalendarEvent> HotelImageUpload(int id)
        {
            var Room = new PRDate();
            var Begin = DateTime.Parse(Room.Begin);
            var End = DateTime.Parse(Room.End);
            decimal Price = 0;
            List<CalendarEvent> Events = new List<CalendarEvent>();
            
                Price =_db.RoomZH.Find(id).Sell.Value;               
            

            Events = (from room in _db.RoomPrice
                      where room.ROOMID == id
                      select new CalendarEvent
                      {
                          Start = room.Date,
                          //End = room.Date.AddDays(1),
                          Off = room.SaleOff,
                          Price = room.Price,
                          Quantity = room.Quantity
                      }).ToList();
           
            DateTime epoc = new DateTime(1970, 1, 1);
            List<CalendarEvent> events = new List<CalendarEvent>();
            if (Events == null || Events.Count <= 0)
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {
                    var beginDay = DateTime.Parse(date.ToShortDateString());
                    var endDay = DateTime.Parse(date.ToShortDateString());
                    events.Add(new CalendarEvent { 
                        Title = "Event" + id.ToString(),
                        Start = beginDay,
                        End = endDay,
                        Price = Price, 
                        Off = true,
                        Quantity = 0 
                    });
                }
            }
            else
            {
                for (var date = Begin; date < End; date = date.AddDays(1.0))
                {

                    var beginDay =DateTime.Parse(date.ToShortDateString());
                    var endDay = DateTime.Parse(date.ToShortDateString());
                    var Off = false;
                    decimal CurrentPrice = 0;
                    int Quantity = 0;
                    var Current = Events.Where(o => o.Start == beginDay).FirstOrDefault();
                    if (Current != null)
                    {
                        CurrentPrice = Current.Price;
                        Off = Current.Off;
                        Quantity = Current.Quantity;
                    }
                    else
                    {
                        CurrentPrice = Price;
                        Off = true;
                        Quantity = 0;
                    }
                    events.Add(new CalendarEvent {
                        Title = "Event" + id.ToString(),
                        Start = beginDay,
                        End = endDay,
                        Price = CurrentPrice,
                        Off = Off,
                        Quantity = Quantity
                    });
                }
            }
            return events;
        }

        [HttpPost]
        [Route("RoomSave")]
        public HttpResponseMessage RoomPriceSave(List<CalendarPost> data)
        {
           
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { message = "ok" }))
            };

            if (data.Count <= 0)
            {
               return response;
            }

            try
            {
                foreach (var item in data)
                {
                    if (_db.RoomPrice.Any(o => o.Date == item.date && o.ROOMID == item.roomid))
                    {
                        var obj = _db.RoomPrice.Where(o => o.Date == item.date && o.ROOMID == item.roomid).FirstOrDefault();
                        obj.ROOMID = item.roomid;
                        obj.Date = item.date;
                        obj.Price = item.price;
                        obj.Quantity = item.quantity;
                        obj.SaleOff = item.off;
                    }
                    else
                    {
                        _db.RoomPrice.Add(new RoomPrice
                        {
                            SaleOff = item.off,
                            Quantity = item.quantity,
                            Price = item.price,
                            Date = item.date,
                            ROOMID = item.roomid
                        });
                    }
                    _db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(JsonConvert.SerializeObject(new { message = ex.Message.ToString() }))
                };

                return response;
            }

           
            return response;
        }
    }
}
