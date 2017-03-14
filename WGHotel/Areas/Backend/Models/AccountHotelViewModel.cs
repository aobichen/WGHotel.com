using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WGHotel.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity; // Maybe this one too
using System.Transactions;

namespace WGHotel.Areas.Backend.Models
{
    public class AccountHotelViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
        
        [Required(ErrorMessage="必填項目")]
        public string Namezh { get; set; }
        //[Required(ErrorMessage = "必填項目")]
        [Required(ErrorMessage = "必填項目")]
        public string Nameus { get; set; }
        public string Featurezh { get; set; }

        
        public string Featureus { get; set; }

        public string Addresszh { get; set; }

        public string Addressus { get; set; }


        public string LinkUrl { get; set; }
        [Required]
        public int City { get; set; }

        public string Area { get; set; }


        public string Tel { get; set; }

        public string Game { get; set; }

        public string Facilies { get; set; }

        public string Languages { get; set; }

        public string Certificate { get; set; }

        public int UserId { get; set; }
        //public List<CodeFile> HotelFacility { get { return new CodeFiles().GetHotelFacility(); } }

        public string ImgKey { get; set; }

        public List<string> HotelFacility { get; set; }
        public List<string> GameSite { get; set; }
        public List<string> Language { get; set; }
        public void Create()
        {
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {
                    var zhHotel = new HotelZH();
                    zhHotel.Name = Namezh;
                    zhHotel.Address = Addresszh;
                    zhHotel.Area = Area;
                    zhHotel.City = City;
                    zhHotel.Facilities = Facilies;
                    zhHotel.Features = Featurezh;
                    zhHotel.Enabled = true;
                    zhHotel.LinkUrl = LinkUrl;
                    zhHotel.Game = Game;
                    zhHotel.Tel = Tel;
                    zhHotel.UserId = UserId;
                    zhHotel.Language = Languages;
                    zhHotel.Certificate = Certificate;

                    _db.HotelZH.Add(zhHotel);
                    _db.SaveChanges();
                    // ZHdb.SaveChanges();
                    var ReferIdZH = zhHotel.ID;
                    var ReferIdUS = 0;


                    var HotelEN = new HotelEN
                    {
                        Name = Nameus,
                        Address = Addressus,
                        Area = Area,
                        City = City,
                        Facilities = Facilies,
                        Features = Featureus,
                        Enabled = true,
                        LinkUrl = LinkUrl,
                        Game = Game,
                        UserId = UserId,
                        Tel = Tel,
                        ParentId = zhHotel.ID,
                        Language = Languages,
                        Certificate = Certificate
                    };

                    _db.HotelEN.Add(HotelEN);
                    
                    _db.SaveChanges();
                    ReferIdUS = HotelEN.ID;
                    #region
                    var Session = HttpContext.Current.Session;

                    if (Session[ImgKey] != null)
                    {

                        var Now = DateTime.Now;
                        var images = (List<ImageViewModel>)Session[ImgKey];
                        foreach (var img in images)
                        {
                            var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                            _db.ImageStore.Add(new ImageStore
                            {
                                Created = Now,
                                Extension = img.Extension,
                                Deleted = false,
                                ReferIdUS = ReferIdUS,
                                ReferIdZH = ReferIdZH,
                                Type = "Hotel",
                                Name = fileName,
                                Image = img.Image
                            });
       
                        }
                        _db.SaveChanges();
                    }
                    #endregion

                   
                    scope.Complete();
                }
            }
        }

        public void Edit()
        {
            var Session = HttpContext.Current.Session;
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {
                    var zhHotel = _db.HotelZH.Find(ID);
                    zhHotel.Name = Namezh;
                    zhHotel.Address = Addresszh;
                    zhHotel.Area = Area;
                    zhHotel.City = City;
                    zhHotel.Facilities = Facilies;
                    zhHotel.Features = Featurezh;
                    zhHotel.Enabled = true;
                    zhHotel.LinkUrl = LinkUrl;
                    zhHotel.Game = Game;
                    zhHotel.Tel = Tel;
                    zhHotel.Language = Languages;
                    zhHotel.Certificate = Certificate;

                    var HotelEN = _db.HotelEN.Where(o => o.ParentId == zhHotel.ID).FirstOrDefault();
                    HotelEN.Name = Nameus;
                    HotelEN.Address = Addressus;
                    HotelEN.Area = Area;
                    HotelEN.City = City;
                    HotelEN.Facilities = Facilies;
                    HotelEN.Features = Featureus;
                    HotelEN.Enabled = true;
                    HotelEN.LinkUrl = LinkUrl;
                    HotelEN.Game = Game;
                    HotelEN.Tel = Tel;
                    HotelEN.ParentId = zhHotel.ID;
                    HotelEN.Language = Languages;
                    HotelEN.Certificate = Certificate;
                    #region # Images
                    if (Session[ImgKey] != null)
                    {

                        var Now = DateTime.Now;
                        var images = (List<ImageViewModel>)Session[ImgKey];
                        var dbImg = _db.ImageStore.Where(o => o.ReferIdZH == zhHotel.ID);
                        var ImgNames = dbImg.Select(o => o.Name).ToList();
                        images = images.Where(o => !ImgNames.Contains(o.Name)).ToList();
                        if (images.Count > 0)
                        {
                            HttpContext.Current.Session["HasNewImage"] = true;
                        }
                        foreach (var img in images)
                        {
                            var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                            _db.ImageStore.Add(new ImageStore
                            {
                                Created = Now,
                                Extension = img.Extension,
                                Deleted = false,
                                ReferIdUS = HotelEN.ID,
                                ReferIdZH = zhHotel.ID,
                                Type = "Hotel",
                                Name = fileName,
                                Image = img.Image
                            });
                        }

                        _db.SaveChanges();
                        scope.Complete();
                    }
                    #endregion
                }
            }
        }
    }
     

    public class HotelListViewModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }

        public string Tel { get; set; }
        public string Name { get; set; }
        
        //public string Feature { get; set; }
        
        //public string Address { get; set; }
       

        public string LinkUrl { get; set; }
        public string City { get; set; }

        //public string Area { get; set; }

        public string Game { get; set; }

       
    }
}