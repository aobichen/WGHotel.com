using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WGHotel.Models;

namespace WGHotel.Controllers
{
    public class ImagesController : BaseController
    {
        [OutputCache(Duration = 7200, Location = OutputCacheLocation.Client, VaryByParam = "id")]
        public ActionResult ShowRoomImage(int id)
        {
            var image = _db.ImageStore.Where(o => o.ID == id && o.Type == "Room").FirstOrDefault();

            byte[] img = image == null ? new ImageDAO().EmptyImageForHotel() : image.Image;
            var Extension = image == null ? "jpg" : image.Extension.Replace(".", "");
            var imgtype = string.Format("image/{0}", Extension);
            return File(img, imgtype);
        }
        // GET: Images
        [OutputCache(Duration = 7200, Location = OutputCacheLocation.Client, VaryByParam = "id")]
        public ActionResult ShowHotelImage(int id)
        {
            var image = _db.ImageStore.Where(o => o.ID == id && o.Type == "Hotel").FirstOrDefault();

            byte[] img = image == null ? new ImageDAO().EmptyImageForHotel() : image.Image;
            var Extension = image == null ? "jpg" : image.Extension.Replace(".", "");
            var imgtype = string.Format("image/{0}", Extension);
            return File(img, imgtype);
        }
    }
}