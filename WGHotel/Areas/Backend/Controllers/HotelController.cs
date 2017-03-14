using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WGHotel.Controllers;
using WGHotel.Areas.Backend.Models;
using WGHotel.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity; // Maybe this one too



namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize]
    public class HotelController : BaseController
    {
       
        public ActionResult Index(string SearchString="",int Page=1)
        {

            var IsHotel = User.IsInRole("Hotel");
            var IsAdmin = User.IsInRole("Admin");
            ViewBag.ViewMessage = string.IsNullOrEmpty(SearchString) ? "目前無任何資料":"沒有任何搜尋資訊";
           // var model = _dbzh.Hotel.ToList();
            if (!IsAdmin)
            {
                SearchString = string.Empty;
                Page = 1;
            }
            var model = (from h in _db.HotelZH
                         join c in _db.CityZH on h.City equals c.ID
                         where string.IsNullOrEmpty(SearchString) || h.Name.Contains(SearchString)
                         select new HotelListViewModel
                         {
                            City = c.Name,
                            Game = h.Game,
                            ID = h.ID,
                            Name = h.Name,
                            UserId = h.UserId,
                            Tel = h.Tel
                         }).OrderBy(o => o.ID).ToList();

                if (!IsAdmin)
                {
                    model = model.Where(o => o.UserId == CurrentUser.Id).ToList();
                }
                         
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;

            var PageModel = model.ToPagedList(currentPage, PageSize);

           
            return View(PageModel);

           
        }

        
         //GET: Backend/Admin
        [Authorize(Roles = "Admin,System")]
        public ActionResult Create(int? id)
        {
            
           
            var model = new AccountHotelViewModel();

            var AccountAndImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            model.Account = AccountAndImgKey.ToUpper();
            model.Password = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.HotelFacility = new Facilities().Facility();
            ViewBag.ImgKey = AccountAndImgKey;
            Session[AccountAndImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            //ViewBag.GameSites = new GameSiteModel().SelectList();
            ViewBag.GameSites = new VenueModel().SelectList();
            ViewBag.City = new GameSiteModel().Citys();
            ViewBag.Language = new LanguageModel().SelectListItem();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,System")]
        public ActionResult Create(AccountHotelViewModel model)
        {
            ViewBag.HotelFacility = new Facilities().Facility();
            ViewBag.ImgKey = model.ImgKey;
            Session[model.ImgKey] = new List<ImageViewModel>();
            //model.ImgKey = AccountAndImgKey;
            ViewBag.GameSites = new GameSiteModel().SelectList();
            ViewBag.City = new GameSiteModel().Citys();
            if (!ModelState.IsValid)
            {
                return View();
            }
            
          
            model.Facilies = (model.HotelFacility == null || model.HotelFacility.Count <= 0) ? string.Empty : string.Join(",", model.HotelFacility);
           
            model.Game = (model.GameSite == null || model.GameSite.Count <= 0) ? string.Empty : string.Join(",", model.GameSite);


            model.Languages = (model.Language == null || model.Language.Count <= 0) ? string.Empty : string.Join(",", model.Language);

           if (ModelState.IsValid)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser { UserName = model.Account, Email = model.Password };
                ApplicationRoleManager _roleManager =  HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                var CurrentRole = "Hotel";
          
               UserManager.Create(user, model.Password);
               UserManager.AddToRole(user.Id, CurrentRole);
               var id = UserManager.FindByName(model.Account).Id;
                model.UserId = id;
                model.Create();
               return RedirectToAction("Index");
                
            }


          
            ModelState.AddModelError("", "錯誤!請檢查資料");
            return View();
        }

        [Authorize(Roles = "Admin,System")]
        public ActionResult Edit(int id)
        {
            //var zh = _dbzh.Hotel.Find(id);
            //var us = _dbus.Hotel.Find()
            var HotelZH = _db.HotelZH.Find(id);
            var HotelEN = _db.HotelEN.Where(o => o.ParentId == HotelZH.ID).FirstOrDefault();

            if (HotelZH == null || HotelEN == null)
            {
                return View("Index");
            }

            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var UserName = Manager.FindById(HotelZH.UserId).UserName;

            var model = new AccountHotelViewModel();
            model.Account = UserName;
            model.Addressus = HotelEN.Address;
            model.Addresszh = HotelZH.Address;
            model.Area = HotelZH.Area;
            model.Featureus = HotelEN.Features;
            model.Featurezh = HotelZH.Features;
            model.Tel = HotelZH.Tel;
            model.Nameus = HotelEN.Name;
            model.Namezh = HotelZH.Name;
            model.LinkUrl = HotelZH.LinkUrl;
            model.Certificate = HotelZH.Certificate;

            var sessionkey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.ImgKey = sessionkey;
            
            var Imgs = _db.ImageStore.Where(o => o.ReferIdZH == HotelZH.ID && o.Type == "Hotel").Select(o => new ImageViewModel
            {
                ReferIdZH = o.ReferIdZH.Value,
                Extension = o.Extension,
                Image = o.Image,
                Name = o.Name,
                SessionKey = sessionkey,
                Type = o.Type
                
            }).ToList();

            Session[sessionkey] = Imgs;
            //model.Facilies = 
            var Facilies = string.IsNullOrEmpty(HotelZH.Facilities) ? null : HotelZH.Facilities.Split(',').Select(int.Parse).ToList();
            var GameSite = string.IsNullOrEmpty(HotelZH.Game)?null : HotelZH.Game.Split(',').Select(int.Parse).ToList();
            ViewBag.HotelFacility = new Facilities().Facility(Facilies);
            ViewBag.GameSites = new VenueModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(HotelZH.City);
            var Language = string.IsNullOrEmpty(HotelZH.Language) ? null : HotelZH.Language.Split(',').Select(int.Parse).ToList();
            ViewBag.Language = new LanguageModel().SelectListItem(Language);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,System")]
        public ActionResult Edit(AccountHotelViewModel model)
        {
           
            var HotelZH = _db.HotelZH.Find(model.ID);
            var HotelEN = _db.HotelEN.Where(o => o.ParentId == HotelZH.ID).FirstOrDefault();

            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var u = Manager.FindById(HotelZH.UserId).UserName;


            model.Facilies = (model.HotelFacility == null || model.HotelFacility.Count <= 0) ? string.Empty : string.Join(",", model.HotelFacility);
          

            model.Game = (model.GameSite == null || model.GameSite.Count <= 0) ? string.Empty : string.Join(",", model.GameSite);
            model.Languages = (model.Language == null || model.Language.Count <= 0) ? string.Empty : string.Join(",", model.Language);
            model.Edit();

            if (Session["HasNewImage"] != null && ((bool)Session["HasNewImage"]) == true)
            {
                var url = Url.Action("ShowHotelImage", "Images", new { Area = string.Empty, id = model.ID });
                HttpResponse.RemoveOutputCacheItem(url);
            }
                
            return RedirectToAction("Edit", new { id = model.ID });
        }

        public ActionResult MyHotel(int id)
        {
            var HotelZH = _db.HotelZH.Find(id);

            if (CurrentUser.Id != HotelZH.UserId)
            {
                return RedirectToAction("","Hotel");
            }

            var HotelEN = _db.HotelEN.Where(o => o.ParentId == HotelZH.ID).FirstOrDefault();


            var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var UserName = Manager.FindById(HotelZH.UserId).UserName;

            var model = new AccountHotelViewModel();
            model.Account = UserName;
            model.Addressus = HotelEN.Address;
            model.Addresszh = HotelZH.Address;
            model.Area = HotelZH.Area;
            model.Featureus = HotelEN.Features;
            model.Featurezh = HotelZH.Features;
            model.Tel = HotelZH.Tel;
            model.Nameus = HotelEN.Name;
            model.Namezh = HotelZH.Name;
            model.LinkUrl = HotelZH.LinkUrl;
            model.City = HotelZH.City;
            var sessionkey = Guid.NewGuid().GetHashCode().ToString("x");
            ViewBag.ImgKey = sessionkey;

            var Imgs = _db.ImageStore.Where(o => o.ReferIdZH == HotelZH.ID && o.Type == "Hotel").Select(o => new ImageViewModel
            {
                ReferIdZH = o.ReferIdZH.Value,
                Extension = o.Extension,
                Image = o.Image,
                Name = o.Name,
                SessionKey = sessionkey,
                Type = o.Type

            }).ToList();

            Session[sessionkey] = Imgs;
            //model.Facilies = 
            var Facilies = HotelZH.Facilities.Split(',').Select(int.Parse).ToList();
            var GameSite = HotelZH.Game.Split(',').Select(int.Parse).ToList();
            ViewBag.HotelFacility = new Facilities().Facility(Facilies);
            ViewBag.GameSites = new VenueModel().SelectList(GameSite);
            ViewBag.City = new GameSiteModel().Citys(HotelZH.City);
            var Language = string.IsNullOrEmpty(HotelZH.Language) ? null : HotelZH.Language.Split(',').Select(int.Parse).ToList();
            ViewBag.Language = new LanguageModel().SelectListItem(Language);
            return View(model);
            
        }
        
    }
}