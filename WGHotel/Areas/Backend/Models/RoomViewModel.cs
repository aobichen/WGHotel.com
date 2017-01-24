using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;
using System.Transactions;
namespace WGHotel.Areas.Backend.Models
{
    public class RoomViewModel
    {
        public RoomViewModel()
        {
            if (_db == null)
            {
                _db = new WGHotelsEntities();
            }
            RoomTypes();
            Facility();
            BedTypes();

            if(!Directory.Exists(HttpContext.Current.Server.MapPath("/UserFolder")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/UserFolder"));
                };
        }

        private WGHotelsEntities _db { get; set; }
        //private WGHotelUSEntities db { get; set; }
        public int HOTELID { get; set; }

        public int IDZH { get; set; }
        public int IDEN { get; set; }
        public string NameZh { get; set; }
        public string NoticeZh { get; set; }

        public string NameUs { get; set; }
        public string NoticeUs { get; set; }

        public string FeatureUs { get; set; }
        public string FeatureZh { get; set; }

        public string RoomType { get; set; }
        public string BedType { get; set; }
        public List<string> Beds { get; set; }
        public string Facilities { get; set; }
        public bool HasBreakfast { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public decimal Sell { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public decimal? MaxPrice { get; set; }
        public int Quantiy { get; set; }
        public bool Enabled { get; set; }

        public string ImgKey { get; set; }
        public SelectList RoomTypeSelectList { get; set; }
        public SelectList BedTypeSelectList { get; set; }

        public List<RoomFacilitiesCheckList> FacilityList { get; set; }


        private void Facility()
        {
            var List = _db.CodeFileZH.Where(o => o.ItemType == "RF").ToList();
            List<RoomFacilitiesCheckList> RF = new List<RoomFacilitiesCheckList>();
            var strFacilities = Facilities != null ? Facilities.Split(',').ToList():new List<string>();
            FacilityList = new List<RoomFacilitiesCheckList>();
            foreach (var item in List)
            {
                var ischecked = List.Where(o => strFacilities.Contains(o.ID.ToString())).Count() > 0 ? true : false;
                FacilityList.Add(new RoomFacilitiesCheckList { Checked = ischecked, ID = item.ID, Name = item.ItemDescription });
            }
        }

        private void RoomTypes()
        {
           var types = _db.CodeFileZH.Where(o => o.ItemType == "Room").ToList();
            RoomTypeSelectList = new SelectList(types, "ID", "ItemDescription", RoomType);
            
        }

        private void BedTypes()
        {
            var types = _db.CodeFileZH.Where(o => o.ItemType == "Bed" && o.Deleted == false).ToList();
            BedTypeSelectList = new SelectList(types, "ID", "ItemDescription", BedType);            
        }

        public void Create()
        {
            var ZHID = 0;
            var USID = 0;
            var ZHHotelID = 0;
            var BedTypes = string.Empty;
            if (Beds != null && Beds.Count > 0)
            {
                BedTypes = string.Join(",", Beds);
            }

            using (var scope = new TransactionScope())
            {

                var RoomZH = new RoomZH();
                RoomZH.Name = NameZh;
                RoomZH.Notice = NoticeZh;
                RoomZH.Feature = FeatureZh;
                RoomZH.BedType = BedTypes;
                RoomZH.RoomType = RoomType;
                RoomZH.Sell = Sell;
                RoomZH.Enabled = true;
                RoomZH.HasBreakfast = HasBreakfast;
                RoomZH.HOTELID = HOTELID;
                RoomZH.Facilities = string.Empty;
                RoomZH.Quantiy = Quantiy;
                RoomZH.MaxPrice = MaxPrice;
                _db.RoomZH.Add(RoomZH);

                _db.SaveChanges();
                ZHID = RoomZH.ID;
                ZHHotelID = RoomZH.HOTELID;

                var RoomEN = new RoomEN();
                RoomEN.Name = NameUs;
                RoomEN.Feature = FeatureUs;
                RoomEN.Notice = NoticeUs;
                RoomEN.BedType = BedTypes;
                RoomEN.RoomType = RoomType;
                RoomEN.Sell = Sell;
                RoomEN.Enabled = true;
                RoomEN.HasBreakfast = HasBreakfast;
                RoomEN.HOTELID = HOTELID;
                RoomEN.MaxPrice = MaxPrice;
                RoomEN.Facilities = string.Empty;
                RoomEN.Quantiy = Quantiy;
                RoomEN.ParentId = ZHID;
                _db.RoomEN.Add(RoomEN);
                _db.SaveChanges();
                USID = RoomEN.ID;
                SaveImageStore(ZHID, USID);
                scope.Complete();
            }
                          
        }

        public void Edit()
        {
            
            var ZHModel = _db.RoomZH.Find(IDZH);
            if (ZHModel == null)
            {
                return;
            }
            var USModel = _db.RoomEN.Where(o=>o.ParentId == IDZH).FirstOrDefault();
            if (USModel == null)
            {
                return;
            }

            ZHModel.Name = NameZh;
            ZHModel.Notice = NoticeZh;

            USModel.Name = NameUs;
            USModel.Notice = NoticeUs;

            
            var BedTypes = string.Empty;
            if (Beds!=null && Beds.Count > 0){
                BedTypes = string.Join(",",Beds);
            }

            using (var scope = new TransactionScope())
            {
                ZHModel.BedType = BedTypes;
                ZHModel.RoomType = RoomType;
                ZHModel.Sell = Sell;
                ZHModel.Enabled = true;
                ZHModel.HasBreakfast = HasBreakfast;
                ZHModel.HOTELID = HOTELID;
                ZHModel.Facilities = string.Empty;
                ZHModel.Quantiy = Quantiy;
                ZHModel.Feature = FeatureZh;
                ZHModel.MaxPrice = MaxPrice;

                USModel.BedType = BedTypes;
                USModel.RoomType = RoomType;
                USModel.Sell = Sell;
                USModel.Enabled = true;
                USModel.HasBreakfast = HasBreakfast;
                USModel.Facilities = string.Empty;
                USModel.Quantiy = Quantiy;
                USModel.Feature = FeatureUs;
                USModel.MaxPrice = MaxPrice;

                _db.SaveChanges();

                SaveImageStore(ZHModel.ID, USModel.ID);
                scope.Complete();
            }
             
            
            
        }

        public void SaveImageStore(int ZHID, int USID)
        {
            var Session = HttpContext.Current.Session;

            if (Session[ImgKey] != null)
            {
                
                var Now = DateTime.Now;
                var images = (List<ImageViewModel>)Session[ImgKey];
                var dbimages = _db.ImageStore.Where(o => o.ReferIdZH == ZHID && o.ReferIdUS == USID).ToList();
                foreach (var img in images)
                {
                    if(!dbimages.Any(o=>o.Name==img.Name)){
                                           
                        var fileName = Guid.NewGuid().GetHashCode().ToString("x");
                        //var file = string.Format("/UserFolder/{0}{1}",fileName, img.Extension);
                        //var path = HttpContext.Current.Server.MapPath(file);
                        //File.WriteAllBytes(path, img.Image);
                        _db.ImageStore.Add(new ImageStore
                        {
                            // Path = file,
                            Created = Now,
                            Extension = img.Extension,
                            Deleted = false,
                            ReferIdUS = USID,
                            ReferIdZH = ZHID,
                            Type = "Room",
                            Name = fileName,
                            Image = img.Image
                        });
                     }
                }
                _db.SaveChanges();
            }
        }
    }

    public class RoomFacilitiesCheckList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

    public class RoomList{
        public int ID { get; set; }
        public string Name { get; set; }
        public string HotelName {get;set;}
        public int HOTELID { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }
        public int Quantiy { get; set; }
        public decimal Sell { get; set; }
    }
}