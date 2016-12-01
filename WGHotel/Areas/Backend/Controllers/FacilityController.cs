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
    public class FacilityController : BaseController
    {
        // GET: Backend/Facility
        public ActionResult Index(int Page=1)
        {
            var FacilityZH = _db.FacilityZH.ToList();
            var FacilityEN = _db.FacilityEN.ToList();
            var model = new List<FacilityModel>();
            foreach (var item in FacilityZH)
            {
                var EN = FacilityEN.Where(o => o.ParentId == item.ID).FirstOrDefault();
                model.Add(new FacilityModel {
                     ID = item.ID,
                     NameZH = item.Name,
                     NameUS = EN == null ? string.Empty : EN.Name,
                     Enabled = item.Enabled
                });
            }

            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            //currentPage = !string.IsNullOrEmpty(SearchString) ? 1 : currentPage;
           // var model = _basedb.Country.Where(o => string.IsNullOrEmpty(SearchString) || o.Name.Contains(SearchString)).ToList();

            var PageModel = model.ToPagedList(currentPage, PageSize);

            return View(PageModel);
        }

        // GET: Backend/Facility/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Backend/Facility/Create
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var model = new FacilityModel();
                var FacilityZH = _db.FacilityZH.Find(id);
                var FacilityEN = _db.FacilityEN.Where(o => o.ParentId == FacilityZH.ID).First();
                model.ID = FacilityZH.ID;
                model.NameZH = FacilityZH.Name;
                model.NameUS = FacilityEN == null ? string.Empty : FacilityEN.Name;
                model.Enabled = FacilityZH.Enabled;
                return View(model);
            }
            return View();
        }

        // POST: Backend/Facility/Create
        [HttpPost]
        public ActionResult Edit(FacilityModel model)
        {
            if (model.ID <= 0)
            {
                if (_db.FacilityZH.Any(o => o.Name == model.NameZH))
                {
                    ModelState.AddModelError("","資料重複");
                    return View();
                }
                model.Create();
                return RedirectToAction("","Facility");
            }
            else
            {
                model.Edit();
            }
            return View();
        }

       

    }
}
