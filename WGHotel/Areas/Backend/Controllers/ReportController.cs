using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using PagedList;
using WGHotel.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Reflection;


namespace WGHotel.Areas.Backend.Controllers

{
    [Authorize]
    public class ReportController : BaseController
    {
        // GET: Backend/Report
        public ActionResult Index(int Page=1)
        {
            var IsAdmin = (User.IsInRole("Admin") || User.IsInRole("SystemAdmin"))?true:false;
            var model = (from report in _db.Report
                         where IsAdmin || report.Creator == CurrentUser.Id
                         select new ReportViewModel
                             {
                                 CheckInDate = report.CheckInDate,
                                 Price = report.Price==null?0:report.Price,
                                 Country = report.Country,
                                 ID = report.ID,
                                 NumOfPeople = report.NumOfPeople,
                                Room = report.Room,                                
                                HotelID = report.HotelID
                             }).OrderByDescending(o=>o.CheckInDate).ToList();

            foreach (var m in model)
            {
                var hotel = _db.HotelZH.Where(o => o.ID == m.HotelID).FirstOrDefault();
                m.HotelName = hotel == null ? string.Empty : hotel.Name;
                //var room = _dbzh.Room.Where(o => o.ID == m.HotelID).FirstOrDefault();
            }

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;

            var PageModel = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);
        }

       

        // GET: Backend/Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (id.HasValue)
            {
                var result = new ReportViewModel();
                var model = _db.Report.Where(o => o.ID == id).Select(o =>
                     new ReportViewModel
                     {
                         CheckInDate = o.CheckInDate,
                         Price = o.Price,
                         OtherCost = o.OtherCost,
                         Other = o.Other,
                         FoodCost = o.FoodCost,
                         Food = o.Food,
                         Country = o.Country,
                         CountryID = o.CountryID,
                         HotelID = o.HotelID,
                         ID = o.ID,
                         NumOfPeople = o.NumOfPeople,
                         Remark = o.Remark,
                         Room = o.Room,
                         RoomID = o.RoomID,
                         UserType = o.UserType,
                     }).FirstOrDefault();
                    
                var Hotel = _db.HotelZH.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel==null ? new List<RoomZH>():Hotel.RoomZH;
                if (User.IsInRole("Admin") || User.IsInRole("System"))
                {
                    model.HotelID = model.HotelID;
                    var Room = _db.RoomZH.Where(o => o.HOTELID == model.HotelID).ToList() ;
                    Rooms = Room;
                    var IDs = Room.Select(o => o.ID).ToList();
                   
                    model.RoomOfReport = _db.ReportRooms.Where(o => o.ReportID == id).ToList();
                
                }else {
                    model.HotelID = Hotel.ID;
                    model.RoomOfReport = _db.ReportRooms.Where(o => o.ReportID == id).ToList();
                }
               

                //model.RoomOfReport = _basedb.ReportRooms.Where(o => o.ID == model.ID).ToList();
                
                //var Rooms = Hotel == null ? new List<Room>() : Hotel.Room;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name",model.RoomID);
                var Country = _db.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name",model.CountryID);
                ViewBag.UserType = model.UserType;
                return View(model);
            }
            else
            {
                var Hotel = _db.HotelZH.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<RoomZH>() : Hotel.RoomZH;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _db.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name");
                

                var model = new ReportViewModel();
                model.CheckInDate = DateTime.Now;
                model.HotelID = Hotel.ID;
                return View(model);
            }

        }

        // POST: Backend/Report/Edit/5
        [HttpPost]
        public ActionResult Edit(ReportViewModel model)
        {
            if (string.IsNullOrEmpty(model.Room)||model.CheckInDate == DateTime.MinValue || model.Price==null )
            {
                TempData["Message"] = "回報資訊不完整";
                if (model.ID != 0)
                {
                    return RedirectToAction("Edit", new { id = model.ID });
                }
                else
                {

                    return RedirectToAction("Edit");
                }
            }
           

            //model.HotelID = _dbzh.Room.Find(model.HotelID).Hotel.ID;
            var IsAdmin = (User.IsInRole("Admin") || User.IsInRole("System"))? true:false;
            var IsMyHotel = _db.HotelZH.Any(o => o.ID == model.HotelID && o.UserId == CurrentUser.Id);
            if (!IsAdmin && !IsMyHotel)      
            {
                return View();
            }
            var Now = DateTime.Now;
            var UserId = CurrentUser.Id;
            model.Created = Now;
            model.Creator = UserId;
            model.Modified = Now;
            model.Modify = UserId;
            //model.CheckInDate = Now;
        
           
            //model.Room = _dbzh.Room.Find(model.RoomID).Name;
            try
            {
                if (model.ID <= 0)
                {
                    model.Create();
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Edit();
                    return RedirectToAction("Edit", new { id = model.ID });
                }

                
            }
            catch
            {
                var Hotel = _db.HotelZH.Where(o => o.UserId == CurrentUser.Id).FirstOrDefault();
                var Rooms = Hotel == null ? new List<RoomZH>() : Hotel.RoomZH;
                ViewBag.RoomId = new SelectList(Rooms, "ID", "Name");
                var Country = _db.Country.ToList();
                ViewBag.Country = new SelectList(Country, "ID", "Name");
                ModelState.AddModelError("","編輯未完成，請檢查資料");
                return View();
            }
        }

        // GET: Backend/Report/Delete/5
        public ActionResult Export(ReportModel search = null)
        {
            var key = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.key = key;
            var Country = _db.Country.ToList().OrderBy(o=>o.Name);
            ViewBag.Nation = new SelectList(Country, "ID", "Name");
            var ReportModel = new ReportModel();
            if(search==null || (search.Nation==0 &&
                search.Begin == DateTime.MinValue && search.End == DateTime.MinValue))
            {
                var Now = DateTime.Now;
                ReportModel.Begin = Now;
                ReportModel.End = Now.AddDays(1);
                return View(ReportModel);
            }

            search.Begin = DateTime.Parse(search.Begin.ToShortDateString() + " 00:00:00");
            search.End = DateTime.Parse(search.End.ToShortDateString() + " 23:59:59");
            var result = (from r in _db.Report
                          where (search.Nation == 0 ||
                          r.CountryID == search.Nation) &&
                          (r.CheckInDate >= search.Begin && r.CheckInDate <= search.End)
                          select new ReportListModel
             {
                 //Amount = r.Amount,
                 ID = r.ID,
                 CheckInDate = r.CheckInDate,
                 Country = r.Country,
                 HotelID = r.HotelID,
                 Rooms = r.Room,
                 Price = r.Price,
                 Amount = r.NumOfPeople.Value,
                 Food = r.Food,
                 FoodCost = r.FoodCost,
                 Other = r.Other,
                 OtherCost = r.OtherCost

             }).ToList();

            var model = new List<ReportListModel>();
            var dtExcel = new List<ReportExcelModel>();
            foreach (var m in result)
            {
                //var list = m.Rooms.Split(',').Select(int.Parse).ToList();
                var room_list =_db.ReportRooms.Where(o=> o.ReportID == m.ID).ToList();
                var room = new List<string>();
                foreach(var item in room_list){
                    room.Add(string.Format("{0}/{1}",item.RoomName,item.Amount));
                }
                
                var hotel = _db.HotelZH.Where(o=>o.ID == m.HotelID).FirstOrDefault();
                var Name = hotel != null ? hotel.Name : string.Empty;
                model.Add(new ReportListModel
                {
                    Rooms = string.Join(",", room),
                    Price = m.Price,
                    Hotel = Name,
                    CheckInDate = m.CheckInDate,
                    Country = m.Country,
                    Amount = m.Amount,
                    Food = m.Food,
                    FoodCost = m.FoodCost,
                    Other = m.Other,
                    OtherCost = m.OtherCost
                });

                dtExcel.Add(new ReportExcelModel
                {
                    國籍 = m.Country,
                    飯店 = Name,
                    房型數量 = string.Join(",", room),
                    人數 = m.Amount,
                    金額 = m.Price.Value.ToString(),
                    入住日期 = m.CheckInDate.ToShortDateString(),
                    餐飲 = m.Food,
                    餐飲金額 = m.FoodCost == null ? string.Empty : m.FoodCost.Value.ToString(),
                    其他 = m.Other,
                    其他金額 = m.OtherCost == null ? string.Empty : m.OtherCost.Value.ToString()
                });
            }
            ViewBag.ReportList = model;

            var dt = ConvertListToDataTable(dtExcel);
            Session[key] = dt;
            ReportModel.Begin = search.Begin;
            ReportModel.End = search.End;
            
            return View(ReportModel);
        }





        private DataTable ConvertListToDataTable<T>(List<T> items)
        {
            // New table.
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public ActionResult ExportData(string key)
        {
            if (Session[key] != null)
            {
                //var list = _basedb.ReportRooms.ToList();
                //DataTable dt = ConvertListToDataTable(list);
                DataTable dt = (DataTable)Session[key];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    var FileName = Guid.NewGuid().GetHashCode().ToString("x");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= " + FileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                
            }
            return RedirectToAction("Export", "Report");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }  


        

    }
}
