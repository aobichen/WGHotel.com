using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class LanguageViewModel
    {
        public int ID { get; set; }
        public string LanguZH { get; set; }
        public string LanguEN { get; set; }

        public bool Deleted { get; set; }

        public void Edit()
        {
            using (var db = new WGHotelsEntities())
            {
                if (ID > 0)
                {
                    var model = db.Language.Find(ID);
                    model.LanguZH = LanguZH;
                    model.LanguEN = LanguEN;
                    db.SaveChanges();
                }
                else
                {
                    db.Language.Add(new Language { LanguZH = LanguZH, LanguEN = LanguEN, Deleted = false });
                    db.SaveChanges();
                }
            }
        }
    }

    public class LanguageModel
    {
        public int ID { get; set; }
        public string Langu { get; set; }

        public List<SelectListItem> ListItems { get { return GetSelectListItem(); } }

        private List<SelectListItem> GetSelectListItem(List<int> Selected = null)
        {
            using (var db = new WGHotelsEntities())
            {
                var SelectListItem = new List<SelectListItem>();
                var items = db.Language.ToList();
                var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
                foreach (var item in items)
                {
                    if (lang == "us")
                    {
                        SelectListItem.Add(new SelectListItem
                        {
                            Text = item.LanguEN,
                            Value = item.ID.ToString(),
                            Selected = Selected == null ? false : Selected.Contains(item.ID)
                        });
                    }
                    else if (lang == "zh")
                    {
                        SelectListItem.Add(new SelectListItem
                        {
                            Text = item.LanguZH,
                            Value = item.ID.ToString(),
                            Selected = Selected == null ? false : Selected.Contains(item.ID)
                        });
                    }
                }
                return SelectListItem;
            }

        }
    }
}