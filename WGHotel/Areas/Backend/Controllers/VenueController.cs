using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using PagedList;
using WGHotel.Controllers;

namespace WGHotel.Areas.Backend.Controllers
{
    public class VenueController : BaseController
    {
        // GET: Backend/Venue
        public ActionResult Index(int Page = 1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            var model = new VenueModel().List();
            var PageModel = model.ToPagedList(currentPage, PageSize);
            //ViewBag.GameList = new GameSiteListModel().List();
            ViewData.Model = PageModel;
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var Venue_ZH = _db.VenueZH.Find(id);
                var Venue_EN = _db.VenueEN.Find(id);
                var Venue = new VenueModel();
                
               
                Venue.SportEN = Venue_EN.Sport;
                Venue.SportZH = Venue_ZH.Sport;
                Venue.TypeEN = Venue_EN.Type;
                Venue.TypeZH = Venue_ZH.Type;
                Venue.VenueEN = Venue_EN.Venue;
                Venue.VenueZH = Venue_ZH.Venue;
                Venue.IDZH = Venue_ZH.ID;
                Venue.IDEN = Venue_EN.ID;
                ViewData.Model = Venue;
                return View();
            }

           // ViewData.Model = new VenueModel();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(VenueModel model)
        {
            if (model.IDZH > 0)
            {
                model.Edit();
                return RedirectToAction("", "Venue");
            }

            if (_db.VenueZH.Any(o => o.Sport == model.SportZH && o.Type == model.TypeZH && o.Venue == model.VenueZH))
            {
                ModelState.AddModelError("", "已有相同項目");
                return View();
            }
            model.Create();
            return RedirectToAction("", "Venue");
        }

    }
}