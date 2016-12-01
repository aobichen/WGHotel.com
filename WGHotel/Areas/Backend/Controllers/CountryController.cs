using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Controllers;
using WGHotel.Models;
using PagedList;
namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize(Roles = "Admin,System")]
    public class CountryController : BaseController
    {
        // GET: Backend/Country
        public ActionResult Index(string SearchString = "", int Page = 1)
        {
           
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            currentPage = !string.IsNullOrEmpty(SearchString) ? 1 : currentPage;
            var model = _db.Country.Where(o => string.IsNullOrEmpty(SearchString) || o.Name.Contains(SearchString)).ToList();
            
            var PageModel = model.ToPagedList(currentPage, PageSize);
            //ViewBag.GameList = new GameSiteListModel().List();
            return View(PageModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var model = _db.Country.Find(id);
                return View(model);
            }
          
            //ViewBag.Country = _basedb.Country.ToList();
            return View(new Country());
        }

        [HttpPost]
        public ActionResult Edit(Country model)
        {
            if (model.ID != 0)
            {
                var result = _db.Country.Find(model.ID);
                result.Name = model.Name;
                result.NUSF = model.NUSF;
                result.Tel1 = model.Tel1;
                result.Tel2 = model.Tel2;
                result.Telmobile = model.Telmobile;
                result.Fax = model.Fax;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (_db.Country.Any(o => o.Name == model.Name))
            {
                ModelState.AddModelError("","資料已存在");
                return View();
            }

            _db.Country.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}