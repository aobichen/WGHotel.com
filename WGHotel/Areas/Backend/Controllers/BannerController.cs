using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Controllers;
using PagedList;
using WGHotel.Models;
using System.IO;
using System.Drawing;
namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize(Roles = "Admin,System")]
    public class BannerController : BaseController
    {
        // GET: Backend/Banner
        public ActionResult Index(int Page=1)
        {
            var model = _db.Banner.ToList();
            var currentPage = Page < 1 ? 1 : Page;
            var PageSize = 15;
            //currentPage = !string.IsNullOrEmpty(SearchString) ? 1 : currentPage;
            // var model = _basedb.Country.Where(o => string.IsNullOrEmpty(SearchString) || o.Name.Contains(SearchString)).ToList();

            var PageModel = model.ToPagedList(currentPage, PageSize);
            return View(PageModel);
        }

       

        // POST: Backend/Banner/Create
       

        // GET: Backend/Banner/Edit/5
        public ActionResult Edit()
        {
            ViewBag.ImgKey = Guid.NewGuid().GetHashCode().ToString("x");
            return View();
        }

        // POST: Backend/Banner/Edit/5
        [HttpPost]
        public ActionResult Edit(string key)
        {
            var models = (List<ImageViewModel>)Session[key];

           
                
                var Now = DateTime.Now;
                foreach (var img in models)
                {
                    var Folder = "/images/banners";
                    if (!Directory.Exists(Server.MapPath(Folder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Folder));
                    }
                    
                    var FileName = Guid.NewGuid().GetHashCode().ToString("x").ToUpper();

                    var Path = string.Format("{0}/{1}{2}", Folder, FileName, img.Extension);
                       
                         MemoryStream ms = new MemoryStream(img.Image);
                         Image returnImage = Image.FromStream(ms);
                         returnImage.Save(Server.MapPath(Path));

                     _db.Banner.Add(new Banner
                    {

                        Image = img.Image,
                        Extension = img.Extension,
                        Enabled = true,
                        Path = Path,
                        Name = img.Name,
                       
                    });

                    
                
                _db.SaveChanges();
            }
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public class BannerEnabled
        {
            public int id { get; set; }
            public bool enabled { get; set; }
        }
        // GET: Backend/Banner/Delete/5
        [HttpPost]
        public ActionResult Delete(BannerEnabled model)
        {
            if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("System")))
            {
                if (_db.Banner.Any(o => o.ID == model.id))
                {
                    var Banner = _db.Banner.Find(model.id);
                    Banner.Enabled = model.enabled;
                    _db.SaveChanges();
                }

                return Json(new { success=true });
            }
           

            return Json(new { success=false });
        }

        // POST: Backend/Banner/Delete/5
        
    }
}
