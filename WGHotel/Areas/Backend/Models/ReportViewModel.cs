using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WGHotel.Areas.Backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using WGHotel.Models;
    public partial class ReportViewModel
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
        public int HotelID { get; set; }

        public string HotelName { get; set; }
        public string Room { get; set; }
       // public int RoomID { get; set; }
        public int Creator { get; set; }
        public System.DateTime Modified { get; set; }

        public int Modify { get; set; }
        public System.DateTime Created { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString="{0:yyyy/MM/dd}", ApplyFormatInEditMode=true)]
        public System.DateTime CheckInDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]   
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> NumOfPeople { get; set; }
        public string Remark { get; set; }

        public int RoomID { get; set; }

        

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]   
        public Nullable<decimal> FoodCost { get; set; }
        public string Other { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]   
        public Nullable<decimal> OtherCost { get; set; }
        public string Food { get; set; }
        public string UserType { get; set; }

        public List<ReportRooms> RoomOfReport { get; set; }

        public List<int> Amount { get; set; }
        public List<int> RoomIds { get; set; }
        public List<int> Quantity { get; set; }
        public List<string> RoomName { get; set; }

        public List<int> ReportOfRoomIds { get; set; }

        public string UserName { get; set; }

        public void Create(){
            using (var _db = new WGHotelsEntities())
            {
                var Model = new Report();
                Model.Created = Created;
                Model.Creator = Creator;
                //Model.CountryID = CountryID;
                var country_id = int.Parse(Country);
                var country = _db.Country.Find(country_id);
                Model.CountryID = country.ID;
                Model.Country = country.Name;
                Model.CheckInDate = CheckInDate;
                Model.HotelID = HotelID;
                Model.NumOfPeople = NumOfPeople;
                Model.Price = Price;
                Model.RoomID = RoomID;
                Model.Modified = Modified;
                Model.Modify = Modify;
                Model.Room = Room;
                Model.Food = Food;
                Model.FoodCost = FoodCost;
                Model.Other = Other;
                Model.OtherCost = OtherCost;
                Model.UserType = UserType;

                var ReportRooms = new List<ReportRooms>();

                for (var i = 0; i < RoomIds.Count; i++)
                {
                    
                    ReportRooms.Add(new ReportRooms { Amount = Amount[i], Quantity = Quantity[i], RoomID = RoomIds[i], RoomName = RoomName[i],Deleted = false });
                }

                 Model.ReportRooms = ReportRooms;

                _db.Report.Add(Model);
                _db.SaveChanges();
            }
        }




        public void Edit()
        {
            using (var db = new WGHotelsEntities())
            {
                var Model = db.Report.Find(ID);
                Model.Modified = Modified;
                Model.Modify = Modify;
              
                var country_id = int.Parse(Country);
                var country = db.Country.Find(country_id);
                Model.CountryID = country.ID;
                Model.Country = country.Name;
                Model.CheckInDate = CheckInDate;
                Model.HotelID = HotelID;
                Model.NumOfPeople = NumOfPeople;
                Model.Price = Price;
                Model.RoomID = RoomID;
                Model.Room = Room;
                Model.Food = Food;
                Model.FoodCost = FoodCost;
                Model.Other = Other;
                Model.OtherCost = OtherCost;
                Model.UserType = UserType;

                //var ExistReprtRooms = db.ReportRooms.Where(o => o.ReportID == ID).ToList();
                var ExistReprtRooms = (from o in db.ReportRooms
                                       where o.ReportID == Model.ID && o.Deleted != true
                                       select o).ToList();
                var count = ExistReprtRooms.Count;
                var NewReportRooms = new List<ReportRooms>();
                var ExistExceptReportRooms = new List<ReportRooms>();
                for (var i = 0; i < RoomIds.Count; i++)
                {
                    if (!ExistReprtRooms.Any(o => o.ID == ReportOfRoomIds[i]))
                    {
                        NewReportRooms.Add(new ReportRooms { ReportID = Model.ID, Amount = Amount[i], Quantity = Quantity[i], RoomID = RoomIds[i], RoomName = RoomName[i], Deleted = false });
                    }
                    else
                    {
                        ExistExceptReportRooms.Add(new ReportRooms { ReportID = Model.ID, ID = ReportOfRoomIds[i], Amount = Amount[i], Quantity = Quantity[i], RoomID = RoomIds[i], RoomName = RoomName[i], Deleted = false });
                    }
                }

                var RemoveReportRooms = new List<ReportRooms>();
                foreach (var item in ExistReprtRooms)
                {
                    if (!ExistExceptReportRooms.Any(o => o.ID == item.ID))
                    {
                        item.Deleted = true;
                    }
                }

                //NewReportRooms 有  ExistReprtRooms 沒有 要新增
                //var NewExceptReportRooms = NewReportRooms;

                //ExistReprtRooms 有 NewReportRooms 沒有要 移除
                //var RemoveReportRooms = ExistExceptReportRooms.Except(ExistReprtRooms).ToList();

                //foreach (var item in RemoveReportRooms)
                //{
                //    var Rroom = db.ReportRooms.Find(item.ID);
                //    if (Rroom != null)
                //    {
                //        Rroom.Deleted = true;
                //    }
                //}

                //db.ReportRooms.AddRange(NewExceptReportRooms);
             
                //if (!string.IsNullOrEmpty(Room))
                //{
                //    var RoomString = Room;
                //    var RoomSpilt = RoomString.Split(',');
                //    foreach (var ro in RoomSpilt)
                //    {
                //        var room = ro.Split('^');
                //        var id = int.Parse(room[0]);
                //        var amt = int.Parse(room[1]);
                //        var name = room[2];
                //        ReportRoomsOf.Add(new ReportRooms { ID = ID, RoomID = id, Amount = amt, RoomName = name });
                       
                //        ReportRooms.Add(new ReportRooms { RoomID = id, Amount = amt, RoomName = name });
                     
                //    }

                //    var rrIDs = ReportRoomsOf.Select(o => o.RoomID).ToList();
                  
                //    var removemodel = db.ReportRooms.Where(o => o.ReportID == ID).ToList();
                //    db.ReportRooms.RemoveRange(removemodel);
                    //Model.ReportRooms = NewExceptReportRooms.ToList();
                //}
                db.ReportRooms.AddRange(NewReportRooms);
                db.Log.Add(new Log { TargetId = ID, Created = DateTime.Now, Creator = UserName, Remark = "ReportViewModel Edit", TargetType = "修改飯店回報資料" });
               
                db.SaveChanges();
            }
        }
    }

    public class ReportListModel
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string Hotel { get; set; }
        public string Rooms { get; set; }

        public string RoomName { get; set; }

        public int RoomQuantity { get; set; }
        public int RoomAmount { get; set; }
        public int Amount { get; set; }
        public DateTime CheckInDate { get; set; }
        public decimal? Price { get; set; }

        public int HotelID { get; set; }
        public string Food { get; set; }
        public decimal? FoodCost { get; set; }
        public string Other { get; set; }
        public decimal? OtherCost { get; set; }
        
    }

    public class ReportExcelModel
    {
        //public int ID { get; set; }
        public string 國籍 { get; set; }
        public string 城市 { get; set; }
        public string 地區 { get; set; }
        public string 飯店 { get; set; }
        public string 房型數量 { get; set; }

        public string 房型 { get; set; }

        public string 價格 { get; set; }

        public int 數量 { get; set; }

        //public int Amount { get; set; }
        public string 入住日期 { get; set; }
        public string 金額 { get; set; }

        public int 人數 { get; set; }

        public string 餐飲 { get; set; }
        public string 餐飲金額 { get; set; }

        public string 其他 { get; set; }
        public string 其他金額 { get; set; }
        //public int HotelID { get; set; }


    }

    public class ReportModel
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public int Nation { get; set; }

        public int City { get; set; }

        public string Keyword { get; set; }
    }
}