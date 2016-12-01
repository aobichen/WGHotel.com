using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WGHotel.Models
{
    public class ImageViewModel
    {
        public byte[] Image { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }

        public int ReferIdUS { get; set; }
        public int ReferIdZH { get; set; }

        public int ReferNameUS { get; set; }
        public int ReferNameZH { get; set; }
        public string Type { get; set; }

        public string SessionKey { get; set; }
    }

    public class ImageDAO
    {
        public byte[] FileToByte(HttpPostedFile file)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
        public void Save(List<ImageViewModel> models)
        {
            using (WGHotelsEntities db = new WGHotelsEntities())
            {
                var dbImg = db.ImageStore;
                var Now = DateTime.Now;
                foreach (var img in models)
                {
                    dbImg.Add(new ImageStore { 
                        ReferIdUS = img.ReferIdUS,
                        ReferIdZH = img.ReferIdZH, 
                        Created = Now, Image = img.Image,
                        Extension = img.Extension,
                        Deleted = false,
                        Path = null,
                        Name = img.Name,
                        Type = img.Type
                    });
                }
                db.SaveChanges();
            }
        }

        public void Delete(string name)
        {
            using (WGHotelsEntities db = new WGHotelsEntities())
            {
                var ImageFile = db.ImageStore.Where(o => o.Name == name).FirstOrDefault();
                if (ImageFile != null)
                {
                    db.ImageStore.Attach(ImageFile);
                    db.ImageStore.Remove(ImageFile);
                    db.SaveChanges();
                }
            }
        }

        public byte[] EmptyImageForHotel()
        {
            var ImagePath =HttpContext.Current.Server.MapPath("~/images/uploads/img.jpg");
            var bitmap = Image.FromFile(ImagePath);
            MemoryStream mr = new MemoryStream();
            bitmap.Save(mr, System.Drawing.Imaging.ImageFormat.Jpeg);
           
            mr.Dispose();
            bitmap.Dispose();
            return mr.ToArray();
        }
    }
}