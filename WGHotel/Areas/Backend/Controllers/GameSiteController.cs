using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;
using PagedList;
namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize(Roles = "Admin,System")]
    public class GameSiteController : BaseController
    {
        // GET: Backend/GameSite
        public ActionResult Index(int Page =1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            var model = new GameSiteListModel().List();
            var PageModel = model.ToPagedList(currentPage, PageSize);
            //ViewBag.GameList = new GameSiteListModel().List();
            return View(PageModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var GameSiteZH = _db.GameSiteZH.Find(id);
                var GameSiteEN = _db.GameSiteEN.Where(o => o.ParentId == GameSiteZH.ID).FirstOrDefault();
                var model = new GameSiteListModel();
                model.RemarkUS = GameSiteEN.Remark;
                model.RemarkZH = GameSiteZH.Remark;
                model.SportUS = GameSiteEN.Sports;
                model.SportZH = GameSiteZH.Sports;
                model.TypeUS = GameSiteEN.Type;
                model.TypeZH = GameSiteZH.Type;
                model.VenueUS = GameSiteEN.Venue;
                model.VenueZH = GameSiteZH.Venue;
                model.IDZH = GameSiteZH.ID;
                model.IDEN = GameSiteEN.ID;
                return View(model);
            }
            return View(new GameSiteListModel());
        }

        [HttpPost]
        public ActionResult Edit(GameSiteListModel model)
        {
            if (model.IDZH >0)
            {
                model.Edit();
                return RedirectToAction("","GameSite");
            }

            if (_db.GameSiteZH.Any(o => o.Sports == model.SportZH && o.Type == model.TypeZH && o.Venue == model.VenueZH))
            {
                ModelState.AddModelError("","已有相同項目");
                return View();
            }
            model.Create();
            return RedirectToAction("","GameSite");
        }
    }
}