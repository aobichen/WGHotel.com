using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Models;
using System.Transactions;
namespace WGHotel.Areas.Backend.Models
{
    public class FacilityModel
    {
        public int ID { get; set; }

        public string NameZH { get; set; }

        public string NameUS { get; set; }

        public bool Enabled { get; set; }

        public void Create()
        {
            var FactitlyZH = new FacilityZH();
            var FacilityEN = new FacilityEN();
            FactitlyZH.Name = NameZH;
            FactitlyZH.Enabled = true;

            using (var scope = new TransactionScope())
            {

                using (var _db = new WGHotelsEntities())
                {
                    _db.FacilityZH.Add(FactitlyZH);

                    FacilityEN.Name = NameUS;
                    FacilityEN.Enabled = true;
                    FacilityEN.ParentId = FactitlyZH.ID;

                    _db.SaveChanges();

                    scope.Complete();
                }
            }

            
            
          
        }

        public void Edit()
        {
            var FactitlyZH = new FacilityZH();
            var FactitlyEN = new FacilityEN();
            using (var scope = new TransactionScope())
            {
                using (var _db = new WGHotelsEntities())
                {
                    FactitlyZH = _db.FacilityZH.Find(ID);
                    FactitlyZH.Name = NameZH;
                    FactitlyZH.Enabled = Enabled;
                    FactitlyEN = _db.FacilityEN.Where(o => o.ParentId == FactitlyZH.ID).First();
                    FactitlyEN.Name = NameUS;
                    FactitlyEN.Enabled = Enabled;
                    _db.SaveChanges();
                    scope.Complete();
                }
            }

           
            
        }

       
    }

    public class Facilities
    {
        public List<SelectListItem> Facility(List<int> Selected = null)
        {
            var lang = HttpContext.Current.Request.Cookies["lang"].Value.ToLower();
            var FacilityZH = new List<FacilityZH>();
            var SelectList = new List<SelectListItem>();
            
            var _db = new WGHotelsEntities();
            object model = _db.FacilityZH.ToList();
            //if (lang.Equals("us"))
            //{
            //    model = _db.FacilityEN.ToList();
            //}else
            //{
            //    model = _db.FacilityZH.ToList();
            //}

            var Facilities = (List<FacilityZH>)model;
            
            SelectList = new List<SelectListItem>();
                foreach (var i in Facilities)
                {
                    SelectList.Add(item: new SelectListItem
                    {
                        Text = i.Name,
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