using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Models;
using AutoMapped;

namespace WGHotel.Controllers
{
    public class HomeController : BaseController
    {//

        public class SearchModel
        {
            public string word { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }

            public string Game { get; set; }
        }

        private List<HotelViewModel> RenderImages(List<HotelViewModel> model)
        {
            if (CurrentLanguage.Equals("us"))
            {
                foreach (var m in model)
                {

                    var image = _db.ImageStore.Where(o => o.ReferIdUS == m.ID).FirstOrDefault();

                    m.Image = image == null ? 0 : image.ID;
                }
            }
            else
            {
                foreach (var m in model)
                {
                    var image = _db.ImageStore.Where(o => o.ReferIdZH == m.ID).FirstOrDefault();
                    m.Image = image == null ? 0 : image.ID;
                }
            }
            return model;
        }

        public ActionResult Index(SearchModel search=null)
        {
            ViewBag.Banners = _db.Banner.Where(o => o.Enabled == true).Select(o => o.Path).ToList();
            var model = new List<HotelViewModel>();
            ViewBag.GameSite = new GameSiteModel().SelectList();

            var CheckInDate = search.Begin <= DateTime.MinValue ? DateTime.Now : search.Begin;
            var CheckOutDate = search.End <= DateTime.MinValue ? DateTime.Now.AddDays(1) : search.End;

            ViewBag.CheckInDate = CheckInDate.ToShortDateString();
            ViewBag.CheckOutDate = CheckOutDate.ToShortDateString();
            #region

            

            if (string.IsNullOrEmpty(search.word) &&
                string.IsNullOrEmpty(search.Game)&&
                search.Begin == DateTime.MinValue &&
                search.End == DateTime.MinValue)
            {

                ViewBag.IsSearch = true;
                if (CurrentLanguage.Equals("us"))
                {
                    #region ** KEELUNG **
                    var Keelung = (from h in _db.HotelEN
                                   where h.City == 1 && h.RoomEN.Count > 0
                                   select new HotelViewModel
                                   {
                                       ID = h.ID,
                                       Name = h.Name,
                                       Game = h.Game,
                                       Sell = h.RoomEN.Min(o => o.Sell),
                                       Tel = h.Tel,
                                       LinkUrl = h.LinkUrl
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Keelung = new HotelModel().MinPirceOfCalendar(Keelung, CheckInDate);
                    ViewBag.Keelung = RenderImages(Keelung);
                    #endregion

                    #region ** TAIPEI **
                    var Taipei = (from h in _db.HotelEN
                                  where h.City == 2 && h.RoomEN.Count > 0
                                  select new HotelViewModel
                                  {
                                      ID = h.ID,
                                      Name = h.Name,
                                     
                                      Game = h.Game,
                                      Sell = h.RoomEN.Min(o => o.Sell),
                                      Tel = h.Tel,
                                      LinkUrl = h.LinkUrl
                                  }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Taipei = new HotelModel().MinPirceOfCalendar(Taipei, CheckInDate);
                    ViewBag.Taipei = RenderImages(Taipei);

                    #endregion

                    #region ** NEWTAIPEI **
                    var NewTaipei = (from h in _db.HotelEN
                                     where h.City == 3 && h.RoomEN.Count > 0
                                     select new HotelViewModel
                                     {
                                         ID = h.ID,
                                         Name = h.Name,
                                         //City = h.City,
                                         Game = h.Game,
                                         Sell = h.RoomEN.Min(o => o.Sell),
                                         Tel = h.Tel,
                                         LinkUrl = h.LinkUrl
                                     }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    NewTaipei = new HotelModel().MinPirceOfCalendar(NewTaipei, CheckInDate);
                    ViewBag.NewTaipei = RenderImages(NewTaipei);
                    #endregion

                    #region ** TAOYUAN **
                    var Taoyuan = (from h in _db.HotelEN
                                   where h.City == 4 && h.RoomEN.Count > 0
                                   select new HotelViewModel
                                   {
                                       ID = h.ID,
                                       Name = h.Name,
                                       //City = h.City,
                                       Game = h.Game,
                                       Sell = h.RoomEN.Min(o => o.Sell),
                                       Tel = h.Tel,
                                       LinkUrl = h.LinkUrl
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Taoyuan = new HotelModel().MinPirceOfCalendar(Taoyuan, CheckInDate);
                    ViewBag.Taoyuan = RenderImages(Taoyuan);
                    #endregion

                    #region ** HSINCHUCITY **
                    var HsinchuCity = (from h in _db.HotelEN
                                       where h.City == 5 && h.RoomEN.Count > 0
                                       select new HotelViewModel
                                       {
                                           ID = h.ID,
                                           Name = h.Name,
                                           //City = h.City,
                                           Game = h.Game,
                                           Sell = h.RoomEN.Min(o => o.Sell),
                                           Tel = h.Tel,
                                           LinkUrl = h.LinkUrl
                                       }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    HsinchuCity = new HotelModel().MinPirceOfCalendar(HsinchuCity, CheckInDate);
                    ViewBag.HsinchuCity = RenderImages(HsinchuCity);
                    #endregion

                    #region ** HSINCHUCOUNTRY **
                    var HsinchuCountry = (from h in _db.HotelEN
                                          where h.City == 6 && h.RoomEN.Count > 0
                                          select new HotelViewModel
                                          {
                                              ID = h.ID,
                                              Name = h.Name,
                                              //City = h.City,
                                              Game = h.Game,
                                              Sell = h.RoomEN.Min(o => o.Sell),
                                              Tel = h.Tel,
                                              LinkUrl = h.LinkUrl
                                          }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    HsinchuCountry = new HotelModel().MinPirceOfCalendar(HsinchuCountry, CheckInDate);
                    ViewBag.HsinchuCountry = RenderImages(HsinchuCountry);
                    #endregion
                }
                else
                {
                    #region ** 基隆市 **
                    var Keelung = (from h in _db.HotelZH
                                   where h.City == 1 && h.RoomZH.Count > 0
                                   select new HotelViewModel
                                   {
                                       ID = h.ID,
                                       Name = h.Name,
                                       //City = h.City,
                                       Game = h.Game,
                                       Sell = h.RoomZH.Min(o => o.Sell),
                                       Tel = h.Tel,
                                       LinkUrl = h.LinkUrl
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Keelung = new HotelModel().MinPirceOfCalendar(Keelung, CheckInDate);
                    ViewBag.Keelung = RenderImages(Keelung);
                    #endregion

                    #region ** 台北市 **
                    var Taipei = (from h in _db.HotelZH
                                  where h.City == 2 && h.RoomZH.Count > 0
                                  select new HotelViewModel
                                  {
                                      ID = h.ID,
                                      Name = h.Name,
                                      //City = h.City,
                                      Game = h.Game,
                                      Sell = h.RoomZH.Min(o => o.Sell),
                                      Tel = h.Tel,
                                      LinkUrl = h.LinkUrl
                                  }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Taipei = new HotelModel().MinPirceOfCalendar(Taipei, CheckInDate);
                    ViewBag.Taipei = RenderImages(Taipei);
                    #endregion

                    #region ** 新北市 **
                    var NewTaipei = (from h in _db.HotelZH
                                     where h.City == 3 && h.RoomZH.Count > 0
                                     select new HotelViewModel
                                     {
                                         ID = h.ID,
                                         Name = h.Name,
                                         //City = h.City,
                                         Game = h.Game,
                                         Sell = h.RoomZH.Min(o => o.Sell),
                                         Tel = h.Tel,
                                         LinkUrl = h.LinkUrl
                                     }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    NewTaipei = new HotelModel().MinPirceOfCalendar(NewTaipei, CheckInDate);
                    ViewBag.NewTaipei = RenderImages(NewTaipei);
                    #endregion

                    #region ** 桃園 **
                    var Taoyuan = (from h in _db.HotelZH
                                   where h.City == 4 && h.RoomZH.Count > 0
                                   select new HotelViewModel
                                   {
                                       ID = h.ID,
                                       Name = h.Name,
                                       //City = h.City,
                                       Game = h.Game,
                                       Sell = h.RoomZH.Min(o => o.Sell),
                                       Tel = h.Tel,
                                       LinkUrl = h.LinkUrl
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    Taoyuan = new HotelModel().MinPirceOfCalendar(Taoyuan, CheckInDate);
                    ViewBag.Taoyuan = RenderImages(Taoyuan);
                    #endregion

                    #region ** 新竹市 **
                    var HsinchuCity = (from h in _db.HotelZH
                                       where h.City == 5 && h.RoomZH.Count > 0
                                       select new HotelViewModel
                                       {
                                           ID = h.ID,
                                           Name = h.Name,                                          
                                           Game = h.Game,
                                           Sell = h.RoomZH.Min(o => o.Sell),
                                           Tel = h.Tel,
                                           LinkUrl = h.LinkUrl
                                       }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    HsinchuCity = new HotelModel().MinPirceOfCalendar(HsinchuCity, CheckInDate);
                    ViewBag.HsinchuCity = RenderImages(HsinchuCity);
                    #endregion

                    #region ** 新竹縣 **
                    var HsinchuCountry = (from h in _db.HotelZH
                                          where h.City == 6 && h.RoomZH.Count > 0
                                          select new HotelViewModel
                                          {
                                              ID = h.ID,
                                              Name = h.Name,
                                              //City = h.City,
                                              Game = h.Game,
                                              Sell = h.RoomZH.Min(o => o.Sell),
                                              Tel = h.Tel,
                                              LinkUrl = h.LinkUrl
                                          }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                    HsinchuCountry = new HotelModel().MinPirceOfCalendar(HsinchuCountry, CheckInDate);
                    ViewBag.HsinchuCountry = RenderImages(HsinchuCountry);
                    #endregion
                }
                return View(model);
            }

            #endregion

           
            var CityModel = _db.CityZH.Where(o => o.Name.Contains(search.word)).FirstOrDefault();
            var City = CityModel == null ? 0 : CityModel.ID;
            if (CurrentLanguage.Equals("us"))
            {
                #region *** 英文搜尋結果 **
                var hotel = (from h in _db.HotelEN
                             where
                             (City == 0 || h.City == City) ||
                             (string.IsNullOrEmpty(search.word) || h.Name.Contains(search.word))
                             && (string.IsNullOrEmpty(search.Game) || h.Game.Contains(search.Game))
                             select h).OrderBy(x => Guid.NewGuid()).ToList();
                var checkInDate = search.Begin == DateTime.MinValue ? DateTime.Now : search.Begin;
                foreach (var h in hotel)
                {
                    
                    var Room = _db.RoomEN.Where(o => o.HOTELID == h.ID).Select(o => o.ID).ToList();
                    #region *** 搜尋後的價格 ***
                    if (Room != null && Room.Count > 0)
                    {

                        var HasRoomPrice = _db.RoomPrice.Where(
                            o => Room.Contains(o.ROOMID) && o.SaleOff == true && (
                             DateTime.Compare(o.Date, checkInDate) == 0)
                            ).OrderBy(o => o.Price).FirstOrDefault();
                        if (HasRoomPrice == null)
                        {
                            model.Add(new HotelViewModel
                            {
                                ID = h.ID,
                                Name = h.Name,
                                Game = h.Game,
                                Sell = h.RoomEN.Min(o => o.Sell),
                                Tel = h.Tel,
                                LinkUrl = h.LinkUrl
                            });
                        }
                        else
                        {
                            if (HasRoomPrice.SaleOff == true)
                            {
                                model.Add(new HotelViewModel
                                {
                                    ID = h.ID,
                                    Name = h.Name,
                                    Game = h.Game,
                                    Sell = HasRoomPrice.Price,
                                    Tel = h.Tel,
                                    LinkUrl = h.LinkUrl
                                });
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region ** 中文搜尋結果 **
                var hotel = (from h in _db.HotelZH
                             where
                             (City == 0 || h.City == City) ||
                             (string.IsNullOrEmpty(search.word) || h.Name.Contains(search.word))
                             && (string.IsNullOrEmpty(search.Game) || h.Game.Contains(search.Game))
                             select h).OrderBy(x => Guid.NewGuid()).ToList();
                var checkInDate = search.Begin == DateTime.MinValue ? DateTime.Now : search.Begin;
                foreach (var h in hotel)
                {

                    //var Room = _db.RoomZH.Where(o => o.HOTELID == h.ID).OrderBy(o => o.Sell).FirstOrDefault();
                    var Room = _db.RoomZH.Where(o => o.HOTELID == h.ID).Select(o => o.ID).ToList();
                    #region *** 搜尋後的價格 ***
                    if (Room != null && Room.Count>0)
                    {

                        var HasRoomPrice = _db.RoomPrice.Where(
                            o => Room.Contains(o.ROOMID) && o.SaleOff == true && (
                             DateTime.Compare(o.Date, checkInDate) == 0)
                            ).OrderBy(o=>o.Price).FirstOrDefault();
                        if (HasRoomPrice == null)
                        {
                            model.Add(new HotelViewModel
                            {
                                ID = h.ID,
                                Name = h.Name,
                                Game = h.Game,
                                Sell = h.RoomZH.Min(o => o.Sell),
                                Tel = h.Tel,
                                LinkUrl = h.LinkUrl
                            });
                        }
                        else
                        {
                            if (HasRoomPrice.SaleOff == true)
                            {
                                model.Add(new HotelViewModel
                                {
                                    ID = h.ID,
                                    Name = h.Name,
                                    Game = h.Game,
                                    Sell = HasRoomPrice.Price,
                                    Tel = h.Tel,
                                    LinkUrl = h.LinkUrl
                                });
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            
            
          
            if(CurrentLanguage.Equals("us")){
                foreach (var m in model)
                {

                    var Image = _db.ImageStore.Where(o => o.ReferIdUS == m.ID).FirstOrDefault();
                    if (Image != null)
                    {
                        m.Image = Image.ID;
                    }
                }
            }
            else
            {
                foreach (var m in model)
                {
                    var image = _db.ImageStore.Where(o => o.ReferIdZH == m.ID).FirstOrDefault();
                    m.Image = image == null ? 0 : image.ID;
                }
            }

          
          

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            
           
            object obj = null;

            if (CurrentLanguage.Equals("us"))
            {


                obj = _db.HotelEN.Find(id);
            }
            else
            {
                obj = _db.HotelZH.Find(id);
            }


            var model = new AutoMapped.AutoMapped().Mapper<HotelZH>(obj);

            if (model == null)
            {
                return RedirectToAction("Index","Home");
            }

            var detail = new HotelDetail();
            detail.ID = model.ID;
            object objImgs = null;
            if (CurrentLanguage.Equals("us"))
            {
                objImgs = _db.ImageStore.Where(o => o.ReferIdUS == model.ID && o.Type == "Hotel").ToList();
            }
            else
            {
                objImgs =  _db.ImageStore.Where(o => o.ReferIdZH == model.ID && o.Type == "Hotel").ToList();
            }
            detail.Images = (List<ImageStore>)objImgs;
            detail.LinkUrl = model.LinkUrl;
            detail.Name = model.Name;
            detail.Tel = model.Tel;
            detail.Address = model.Address;
            detail.Feature = model.Features;
            var Facilities = model.Facilities.Split(',').Select(Int32.Parse).ToList();

            object ObjFacility = null;
            if (CurrentLanguage.Equals("us"))
            {
                ObjFacility =  _db.FacilityEN.Where(o => Facilities.Contains(o.ID)).Select(p => p.Name).ToList();
            }
            else
            {
                ObjFacility =  _db.FacilityZH.Where(o => Facilities.Contains(o.ID)).Select(p => p.Name).ToList();
            }

            detail.Facilities = (List<string>)ObjFacility;

            string StrCity = string.Empty ;

            if (CurrentLanguage.Equals("us"))
            {
                StrCity = _db.CityEN.Where(o => o.ID == model.City).FirstOrDefault().Name;
            }
            else
            {
                StrCity = _db.CityEN.Where(o => o.ID == model.City).FirstOrDefault().Name;
            }

            detail.City = StrCity;





            var RoomsOfHotel = _db.RoomZH.Where(o=>o.HOTELID == model.ID).ToList();
            var rooms = RoomsOfHotel.Select(o => o.ID).ToList();
            List<RoomViewList> Rooms = new List<RoomViewList>();
            if (CurrentLanguage.Equals("us"))
            {
                Rooms = (from r in _db.RoomEN
                         where rooms.Contains(r.ID)
                         select new RoomViewList
                         {
                             Feature = r.Feature,
                             Notice = r.Notice,
                             ID = r.ID,
                             BedType = r.BedType,
                             LinkUrl = model.LinkUrl,
                             //Images = _basedb.ImageStore.Where(o => o.ID == r.ID && o.Type == "Room").ToList(),
                             Name = r.Name,
                             Quantiy = r.Quantiy,
                             RoomType = r.RoomType,
                             Sell = r.Sell,
                             HasBreakfast = r.HasBreakfast.Value
                         }).ToList();
            }
            else
            {
                Rooms = (from r in _db.RoomZH
                         where rooms.Contains(r.ID)
                         select new RoomViewList
                         {
                             Feature = r.Feature,
                             Notice = r.Notice,
                             ID = r.ID,
                             BedType = r.BedType,
                             LinkUrl = model.LinkUrl,
                             //Images = _basedb.ImageStore.Where(o => o.ID == r.ID && o.Type == "Room").ToList(),
                             Name = r.Name,
                             Quantiy = r.Quantiy,
                             RoomType = r.RoomType,
                             Sell = r.Sell,
                             HasBreakfast = r.HasBreakfast.Value
                         }).ToList();
            }


            detail.Rooms = Rooms;


            
            
            foreach (var r in detail.Rooms)
            {
               if (CurrentLanguage.Equals("us"))
               {
                  r.Images = _db.ImageStore.Where(o => o.ReferIdUS == r.ID && o.Type == "Room").ToList();
                }
               else
               {
                   r.Images = _db.ImageStore.Where(o => o.ReferIdZH== r.ID && o.Type == "Room").ToList();
               }
               
            }

            var gamesites = model.Game.Split(',').ToList();

            var NearHotels = new List<HotelDetail>();

            if (CurrentLanguage.Equals("us"))
            {
                NearHotels = _db.HotelEN.Where(o => gamesites.Contains(o.Game) && o.ID != model.ID)
                .Select(o => new HotelDetail
                {
                    ID = o.ID,
                    Address = o.Address,
                    Name = o.Name,
                    ParentId = o.ParentId == null ? 0 : o.ParentId.Value
                }).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            }
            else
            {
                NearHotels = _db.HotelZH.Where(o => gamesites.Contains(o.Game) && o.ID != model.ID)
               .Select(o => new HotelDetail
               {
                   ID = o.ID,
                   Address = o.Address,
                   Name = o.Name,
                   ParentId = o.ParentId == null ? 0 : o.ParentId.Value
               }).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            }

               

            

            foreach (var item in NearHotels)
            {
                var ReferIdZH = item.ID;
                if (CurrentLanguage.Equals("us"))
                {
                    item.Images = _db.ImageStore.Where(o => o.ReferIdUS == ReferIdZH).ToList();
                }
                else
                {
                    item.Images = _db.ImageStore.Where(o => o.ReferIdZH == ReferIdZH).ToList();
                }

            }
            ViewBag.NearHotels = NearHotels;
            return View(detail);
        }

        public ActionResult HotelFirstImage(int id)
        {
            var image = _db.ImageStore.Where(o=>o.ID == id).FirstOrDefault();
            
            byte[] img = image == null ? new ImageDAO().EmptyImageForHotel() : image.Image;
            var Extension = image == null ? "jpg" : image.Extension.Replace(".", "");
            var imgtype = string.Format("image/{0}", Extension);
            return File(img,imgtype);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}