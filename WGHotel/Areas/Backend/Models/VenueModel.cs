using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;
using System.Transactions;
using System.ComponentModel;
using System.Web.Mvc;

namespace WGHotel.Areas.Backend.Models
{
    public class VenueModel
    {
        public int IDZH { get; set; }
        public int IDEN { get; set; }
        public string SportZH { get; set; }
        public string TypeZH { get; set; }
        public string VenueZH { get; set; }
        //public string RemarkZH { get; set; }

        public string SportEN { get; set; }
        public string TypeEN { get; set; }
        public string VenueEN { get; set; }

        public bool Deleted { get; set; }
        //public string RemarkUS { get; set; }

        public List<VenueModel> List()
        {
            var VenueZH = new List<VenueZH>();
            using (var db = new WGHotelsEntities())
            {
                VenueZH = db.VenueZH.ToList();
            }

            var VenueEN = new List<VenueEN>();
            using (var db = new WGHotelsEntities())
            {
                VenueEN = db.VenueEN.ToList();
            }

            var Venues = new List<VenueModel>();

            for (var i = 0; i < VenueEN.Count; i++)
            {
                Venues.Add(new VenueModel
                {
                    //RemarkZH = GameZH[i].Remark,
                    //RemarkUS = GameUS[i].Remark,
                    SportEN = VenueEN[i].Sport,
                    SportZH = VenueZH[i].Sport,
                    TypeEN = VenueEN[i].Type,
                    TypeZH = VenueZH[i].Type,
                    VenueEN = VenueEN[i].Venue,
                    VenueZH = VenueZH[i].Venue,
                    IDZH = VenueZH[i].ID,
                    IDEN = VenueEN[i].ID,
                    Deleted = !VenueZH[i].Deleted.HasValue ? false : VenueZH[i].Deleted.Value
                });
            }

            return Venues;
        }

        public void Edit()
        {
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {
                    var Venue_ZH = _db.VenueZH.Find(IDZH);
                    var Venue_EN = _db.VenueEN.Find(IDZH);

                    Venue_ZH.Sport = SportZH;
                    Venue_ZH.Type = TypeZH;
                    Venue_ZH.Venue = VenueZH;
                    Venue_ZH.Deleted = Deleted;
                    //_db.SaveChanges();

                    Venue_EN.Venue = VenueEN;
                    Venue_EN.Sport = SportEN;
                    Venue_EN.Deleted = Deleted;
                    Venue_EN.Type = VenueViewModel.GetTypeEN(TypeZH);
                    _db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void Create()
        {
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {

                    var Venue_ZH = new VenueZH();

                    Venue_ZH.Sport = SportZH;
                    Venue_ZH.Type = TypeZH;
                    Venue_ZH.Venue = VenueZH;
                    Venue_ZH.Deleted = Deleted;
                    _db.VenueZH.Add(Venue_ZH);
                    //_db.SaveChanges();

                    var Venue_EN = new VenueEN();                    
                    Venue_EN.Sport = SportEN;
                    Venue_EN.Type = VenueViewModel.GetTypeEN(TypeZH);
                    Venue_EN.Venue = VenueEN;
                    Venue_ZH.Deleted = Deleted;
                    _db.VenueEN.Add(Venue_EN);
                    _db.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public List<SelectListItem> SelectList(List<int> Selected = null)
        {
            
            var Items = new List<GameSiteViewModel>();
            var _db = new WGHotelsEntities();
            

            object Games = null;
            
            Games = _db.VenueZH.ToList();
            
            
            var SelectList = new List<SelectListItem>();
            
                foreach (var i in (List<VenueZH>)Games)
                {
                    SelectList.Add(item: new SelectListItem
                    {
                        Text = string.Format("{0}/{1}/{2}",i.Type, i.Venue, i.Sport),
                        Value = i.ID.ToString(),
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }



                _db.Dispose();

            return SelectList;
        }

        public List<DropDownListItem> SelectListItem(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();

            //var Items = new List<GameSiteViewModel>();
            var _db = new WGHotelsEntities();


            object Games = null;
            if (lang.Equals("zh"))
            {
                Games = _db.VenueZH.Where(o => o.Deleted != true).ToList();
            }
            else
            {
                Games = _db.VenueEN.Where(o => o.Deleted != true).ToList();
            }
            var SelectList = new List<DropDownListItem>();

            if (lang.Equals("zh"))
            {
                SelectList.Add(item: new DropDownListItem
                {
                    Text = "--請選擇--",
                    Value = "0",
                    DataAttr = "",
                    Selected = true
                });
                foreach (var i in (List<VenueZH>)Games)
                {
                   
                    SelectList.Add(item: new DropDownListItem
                    {
                        Text = string.Format("{0}/{1}", i.Venue, i.Sport),
                        Value = i.ID.ToString(),
                        DataAttr = i.Sport,
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }
            else
            {
                SelectList.Add(item: new DropDownListItem
                {
                    Text = "--Please select--",
                    Value = "0",
                    DataAttr = "",
                    Selected = true
                });
                foreach (var i in (List<VenueEN>)Games)
                {
                    
                    SelectList.Add(item: new DropDownListItem
                    {
                        Text = string.Format("{0}/{1}", i.Venue, i.Sport),
                        Value = i.ID.ToString(),
                        DataAttr = i.Sport,
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }



            return SelectList;
        }

        public List<DropDownListItem> SelectSportListItem(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();

            //var Items = new List<GameSiteViewModel>();
            var _db = new WGHotelsEntities();


            object Games = null;
            if (lang.Equals("zh"))
            {
                Games = _db.VenueZH.Where(o => o.Deleted != true).ToList();
            }
            else
            {
                Games = _db.VenueEN.Where(o => o.Deleted != true).ToList();
            }
            var SelectList = new List<DropDownListItem>();
            if (lang.Equals("zh"))
            {
                SelectList.Add(item: new DropDownListItem
                {
                    Text = "--請選擇--",
                    Value = "0",
                    DataAttr = "",
                    Selected = true
                });
                foreach (var i in (List<VenueZH>)Games)
                {
                    
                    SelectList.Add(item: new DropDownListItem
                    {

                        Text = string.Format("{0}",i.Sport),
                        Value = i.Sport,
                        DataAttr = i.Type,
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }
            else
            {
                SelectList.Add(item: new DropDownListItem
                {
                    Text = "--Please select--",
                    Value = "0",
                    DataAttr = "",
                    Selected = true
                });
                foreach (var i in (List<VenueEN>)Games)
                {
                    
                    SelectList.Add(item: new DropDownListItem
                    {
                        Text = string.Format("{0}", i.Sport),
                        Value = i.Sport,
                        DataAttr = i.Type,
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }



            return SelectList;
        }
    }

     
    public enum VenueType
    {
       
        None = 0,
       
        Competition,
       
        NonCompetition
    }

    public static class VenueViewModel 
    {
        public static string GetTypeEN(string type)
        {
            var TypeEN = string.Empty;
            switch (type)
            {
                case "競賽類":
                    TypeEN = VenueType.Competition.ToString();
                    break;
                case "非競賽類":
                    TypeEN = VenueType.NonCompetition.ToString();
                    break;
            }
            return TypeEN;
        }
        public static List<SelectListItem> VenueTypeList { get { return GetTypeList(); } }

        //public static List<SelectListItem> VenueSportList { get { return GetTypeList(); } }



        public static List<SelectListItem> GetTypeList(List<string> Selected)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            var SelectListItem = new List<SelectListItem>();
            var items = new string[] { "競賽類", "非競賽類" };
            var itemsEN = new string[] { "Competition", "NonCompetition" };
            if (lang.Equals("zh"))
            {
                
                foreach (var item in items)
                {
                    
                    SelectListItem.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item,
                        Selected = (Selected == null || Selected.Count <= 0) ? false :
                        Selected.Contains(item)
                    });
                }
            }
            else
            {
               
                foreach (var item in itemsEN)
                {

                    SelectListItem.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item,
                        Selected = (Selected == null || Selected.Count <= 0) ? false :
                        Selected.Contains(item)
                    });
                }
            }

            return SelectListItem;
        }

        public static List<SelectListItem> GetTypeList()
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            var SelectListItem = new List<SelectListItem>();
            var items = new string[] { "競賽類", "非競賽類" };
            var itemsEN = new string[] { "Competition", "NonCompetition" };
            if (lang.Equals("zh"))
            {
                SelectListItem.Add(new SelectListItem
                {
                    Text = "請選擇",
                    Value = "0",
                    Selected = true
                });
                foreach (var item in items)
                {

                    SelectListItem.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item,
                        Selected = false
                    });
                }
            }
            else
            {
                SelectListItem.Add(new SelectListItem
                {
                    Text = "Please select",
                    Value = "0",
                    Selected = true
                });
                foreach (var item in itemsEN)
                {

                    SelectListItem.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item,
                        Selected = false
                    });
                }
            }

            return SelectListItem;
        }

        public static List<SelectListItem> GetSportList()
        {
            var SelectListItem = new List<SelectListItem>();
            var items = new string[] { "競賽類", "非競賽類" };
            foreach (var item in items)
            {
                SelectListItem.Add(new SelectListItem { Text = item, Value = item });
            }

            return SelectListItem;
        }

       
    }
}