using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;
using System.Transactions;
namespace WGHotel.Areas.Backend.Models
{
    public class GameSiteListModel
    {
        public int IDZH { get; set; }
        public int IDEN { get; set; }
        public string SportZH { get; set; }
        public string TypeZH { get; set; }
        public string VenueZH { get; set; }
        public string RemarkZH { get; set; }

        public string SportUS { get; set; }
        public string TypeUS { get; set; }
        public string VenueUS { get; set; }
        public string RemarkUS { get; set; }

        public void Create()
        {
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {

                    var GameZH = new GameSiteZH();
                    GameZH.Remark = RemarkZH;
                    GameZH.Sports = SportZH;
                    GameZH.Type = TypeZH;
                    GameZH.Venue = VenueZH;
                    _db.GameSiteZH.Add(GameZH);
                    _db.SaveChanges();
                    var GameUS = new GameSiteEN();
                    GameUS.ParentId = GameZH.ID;
                    GameUS.Remark = RemarkUS;
                    GameUS.Sports = SportUS;
                    GameUS.Type = TypeUS;
                    GameUS.Venue = VenueUS;
                    _db.GameSiteEN.Add(GameUS);
                    _db.SaveChanges();
                }
            }
        }

        public void Edit()
        {
            using (var _db = new WGHotelsEntities())
            {
                var zh = _db.GameSiteZH.Find(IDZH);
                var us = _db.GameSiteEN.Where(o => o.ParentId == zh.ID).FirstOrDefault();
                zh.Remark = RemarkZH;
                zh.Sports = SportZH;
                zh.Type = TypeZH;
                zh.Venue = VenueZH;
                _db.SaveChanges();
                us.Venue = VenueUS;
                us.Remark = RemarkUS;
                us.Sports = SportUS;
                us.Type = TypeUS;
                _db.SaveChanges();
            }
        }

        public List<GameSiteListModel> List()
        {
            var GameZH = new List<GameSiteZH>();
            using (var db = new WGHotelsEntities())
            {
                GameZH = db.GameSiteZH.ToList();
            }

            var GameUS = new List<GameSiteEN>();
            using (var db = new WGHotelsEntities())
            {
                GameUS = db.GameSiteEN.ToList();
            }

            var Games = new List<GameSiteListModel>();

            for (var i =0 ;i<GameZH.Count;i++)
            {
                Games.Add(new GameSiteListModel 
                {
                    RemarkZH = GameZH[i].Remark,
                    RemarkUS= GameUS[i].Remark,
                    SportUS = GameUS[i].Sports,
                    SportZH = GameZH[i].Sports,
                    TypeUS = GameUS[i].Type,
                    TypeZH = GameZH[i].Type,
                    VenueUS = GameUS[i].Venue,
                    VenueZH = GameZH[i].Venue,
                    IDZH = GameZH[i].ID,
                    IDEN = GameUS[i].ID
                });
            }

            return Games;
        }

    }
    public class GameSiteViewModel
    {
        public int ID { get; set; }
        public bool Checked { get; set; }
        public string Value { get; set; }

       
    }

    public class GameSiteModel
    {
        private WGHotelsEntities _db = new WGHotelsEntities();
        public GameSiteModel()
        {
            if (_db == null)
            {
                _db = new WGHotelsEntities();
            }

        }
        public List<GameSiteViewModel> List(List<string> checkeds=null)
        {
            var Items = new List<GameSiteViewModel>();

           
                var Games = _db.GameSiteZH.ToList();
                
                foreach (var item in Games)
                {
                    var id = item.ID.ToString();
                    bool IsChecked = (checkeds == null || checkeds.Count <= 0) ? false : (checkeds.Contains(id) ? true : false);
                    Items.Add(new GameSiteViewModel { Value = item.Venue, ID = item.ID, Checked = IsChecked });
                }
            

            return Items;
        }

        public List<SelectListItem> SelectList(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();

            var Items = new List<GameSiteViewModel>();


            object Games = null;
            if (lang.Equals("zh"))
            {
                Games = _db.GameSiteZH.ToList();
            }
            else
            {
                Games = _db.GameSiteEN.ToList();
            }
            var SelectList = new List<SelectListItem>();
            if (lang.Equals("zh"))
            {
                foreach (var i in (List<GameSiteZH>)Games)
                {
                    SelectList.Add(item: new SelectListItem
                    {
                        Text = string.Format("{0}/{1}", i.Venue, i.Sports),
                        Value = i.ID.ToString(),
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }
            else
            {
                foreach (var i in (List<GameSiteEN>)Games)
                {
                    SelectList.Add(item: new SelectListItem
                    {
                        Text = string.Format("{0}/{1}", i.Venue, i.Sports),
                        Value = i.ID.ToString(),
                        Selected = Selected == null
                           ? false
                           : Selected.Contains(i.ID)
                    });
                }
            }

            

            return SelectList;
        }

        public List<SelectListItem> Citys(int Selected = 0)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            object City = null;

            if (lang.Equals("zh"))
            {
                City = _db.CityZH.ToList();
            }
            else
            {
                City = _db.CityEN.ToList();
            }

            var SelectList = new List<SelectListItem>();
           
               foreach (var i in (List<CityZH>)City)
               {
                   SelectList.Add(item: new SelectListItem
                   {
                       Text = i.Name,
                       Value = i.ID.ToString(),
                       Selected = Selected == 0
                          ? false
                          : Selected.Equals(i.ID)
                   });
               }
            

           // var List = new SelectList(city,"ID", "Name",id.ToString());
            return SelectList;
        }
        
    }

    public class GameSiteModels
    {
        public int ID { get; set; }
        public string Sports { get; set; }
        public string Type { get; set; }
        public string Venue { get; set; }
        public string Remark { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
}