using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Areas.Backend.Models;
using WGHotel.Controllers;

namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize(Roles = "Admin,System")]
    public class SystemController : BaseController
    {
        // GET: Backend/System
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult RoomPriceDate()
        {
           
            var PRDate = new PRDate();
            return View(PRDate);
        }

        [HttpPost]
        public ActionResult RoomPriceDate(PRDate model)
        {
            var BeginIsDate = IsDate(model.Begin);
            if (!BeginIsDate)
            {
                ModelState.AddModelError("Begin","日期格式錯誤");
                return View();
            }

            var EndIsDate = IsDate(model.End);
            if (!EndIsDate)
            {
                ModelState.AddModelError("End", "日期格式錯誤");
                return View();
            }
            model.Edit();
            return View();
        }

        public ActionResult RoomCanEditDate()
        {
            var model = new RoomCanEditDate();
            return View(model);
        }

        [HttpPost]
        public ActionResult RoomCanEditDate(RoomCanEditDate model)
        {
            var BeginIsDate = IsDate(model.Begin);
            if (!BeginIsDate)
            {
                ModelState.AddModelError("Begin", "日期格式錯誤");
                return View();
            }

            //var EndIsDate = IsDate(model.End);
            //if (!EndIsDate)
            //{
            //    ModelState.AddModelError("End", "日期格式錯誤");
            //    return View();
            //}
            model.Edit();
            return View();
        }

        private bool IsDate(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}