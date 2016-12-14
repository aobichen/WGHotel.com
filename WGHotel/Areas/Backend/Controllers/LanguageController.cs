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
    public class LanguageController : BaseController
    {
        // GET: Backend/Language
        [Authorize(Roles = "Admin,System")]
        public ActionResult Index(int Page =1)
        {
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            var model = _db.Language.Select(o => new LanguageViewModel
            {
                ID = o.ID,
                LanguZH = o.LanguZH,
                Deleted = o.Deleted,
                LanguEN = o.LanguEN
            }).ToList();
            var PageModel = model.ToPagedList(currentPage, PageSize);
            //ViewBag.GameList = new GameSiteListModel().List();
            ViewData.Model = PageModel;
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var model = _db.Language.Where(o => o.ID == id).Select(o => new LanguageViewModel
                {
                    ID = o.ID,
                    LanguZH = o.LanguZH,
                    Deleted = o.Deleted,
                    LanguEN = o.LanguEN
                }).FirstOrDefault();
               ViewData.Model = model;
               return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(LanguageViewModel model)
        {
            //if(ModelState.IsValid){
                model.Edit();
            //}
            return View();
        }
    }
}