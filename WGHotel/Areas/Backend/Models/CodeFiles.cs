using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WGHotel.Helpers;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class CodeFiles
    {
        protected WGHotelsEntities _db;
        public CodeFiles()
            : base()
        {
            if (_db == null)
            {
                _db = new WGHotelsEntities();
            }
        }


        public List<CodeFileZH> GetHotelFacilityList(List<int> Selected = null)
        {
            var CodeType = "HF";
            var Items = _db.CodeFileZH.Where(o => o.ItemType == CodeType).ToList();

            return Items;
        }

        public List<SelectListItem> GetHotelFacility(List<int> Selected=null)
        {
            var CodeType = "HF";
            var Items = _db.CodeFileZH.Where(o=>o.ItemType == CodeType).ToList();
            var SelectList = new List<SelectListItem>();
            foreach (var i in Items)
            {
                SelectList.Add(item: new SelectListItem { 
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