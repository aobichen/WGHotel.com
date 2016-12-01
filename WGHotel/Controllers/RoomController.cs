using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Controllers
{
    public class RoomController : BaseController
    {
        // GET: Room
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoomImage(int id)
        {
            var image = _db.ImageStore.Where(o => o.ID == id && o.Type=="Room").FirstOrDefault();

            byte[] img = image == null ? new ImageDAO().EmptyImageForHotel() : image.Image;
            var Extension = image == null ? "jpg" : image.Extension.Replace(".", "");
            var imgtype = string.Format("image/{0}", Extension);
            return File(img, imgtype);
        }
    }
}