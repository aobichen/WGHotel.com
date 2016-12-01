using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;
using System.Transactions;
namespace WGHotel.Areas.Backend.Models
{
    public class BedViewModel
    {
        public int ID { get; set; }
        public string NameZH { get; set; }
        public string NameEN { get; set; }

        public bool Deleted { get; set; }
        public int Creator { get; set; }

        public void Create()
        {
            var Today = DateTime.Now;
            using (var scope = new TransactionScope())
            {
                var _db = new WGHotelsEntities();

                var CodeFileZH = new CodeFileZH();
                CodeFileZH.ItemCode = "Bed";
                CodeFileZH.ItemType = "Bed";
                CodeFileZH.ItemDescription = NameZH;
                CodeFileZH.Created = Today;
                CodeFileZH.Creator = Creator;
                CodeFileZH.Modified = Today;
                CodeFileZH.Modify = Creator;
                CodeFileZH.Remark = "Bed Type";
                _db.CodeFileZH.Add(CodeFileZH);
                _db.SaveChanges();

                var CodeFileEN = new CodeFileEN();
                CodeFileEN.ItemCode = "Bed";
                CodeFileEN.ItemType = "Bed";
                CodeFileEN.ItemDescription = NameEN;
                CodeFileEN.Created = Today;
                CodeFileEN.Creator = Creator;
                CodeFileEN.Modified = Today;
                CodeFileEN.Modify = Creator;
                CodeFileEN.ParentId = CodeFileZH.ID;
                CodeFileEN.Remark = "Bed Type";
                _db.CodeFileEN.Add(CodeFileEN);
                _db.SaveChanges();
                scope.Complete();
            }
                
           
            
        }

        public void Edit()
        {
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {
                    var Today = DateTime.Now;

                    var ZH = _db.CodeFileZH.Where(o => o.ID == ID && o.ItemCode == "Bed").FirstOrDefault();
                    ZH.ItemCode = "Bed";
                    ZH.ItemType = "Bed";
                    ZH.ItemDescription = NameZH;

                    ZH.Modified = Today;
                    ZH.Modify = Creator;
                    ZH.Deleted = Deleted;
                    //_dbZH.CodeFile.Add(ZH);


                    var ParentId = ZH.ID;

                    var EN = _db.CodeFileEN.Where(o => o.ParentId == ParentId && o.ItemCode == "Bed").FirstOrDefault();
                    EN.ItemCode = "Bed";
                    EN.ItemType = "Bed";
                    EN.ItemDescription = NameEN;

                    EN.Modified = Today;
                    EN.Modify = Creator;
                    EN.Deleted = Deleted;


                    _db.SaveChanges();
                    scope.Complete();
                }
            }
        }
        
    }

    public class BedModel
    {
        public List<SelectListItem> SelectList(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            var _db = new WGHotelsEntities();
            object model = null;
            if (lang == "us")
            {
                model = _db.CodeFileEN.Where(o => o.ItemType == "Bed").ToList();

            }
            else
            {
                model = _db.CodeFileZH.Where(o => o.ItemType == "Bed").ToList();
            }
           
            
            var Beds = (List<CodeFileZH>)model;
            var SelectList = new List<SelectListItem>();
            foreach (var i in Beds)
            {
                SelectList.Add(item: new SelectListItem
                {
                    Text = i.ItemDescription,
                    Value = i.ID.ToString(),
                    Selected = Selected == null
                       ? false
                       : Selected.Contains(i.ID)
                });
            }



            return SelectList;
        }
    }

}