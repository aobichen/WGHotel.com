using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Helpers
{
    public class CodeFiles
    {
        static WGHotelsEntities _db = new WGHotelsEntities();

        //static string lang = "zh";

        
        public static string GetCodeFileDescription(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            var i = int.Parse(id);
            var Code = _db.CodeFileZH.Find(i);
            if (Code == null)
            {
                return string.Empty;
            }
            return Code.ItemDescription;
        }

        public static string GetCodeFileForBed(string id)
        {
            var lang = "zh";
            if (HttpContext.Current.Request.Cookies["lang"] != null && HttpContext.Current.Request.Cookies["lang"].Value.ToString().ToLower() != "zh")
            {
                lang = "us";
            }
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }
            var IDs = id.Split(',').Select(int.Parse).ToList();
            
            var Bed = string.Empty;

            if (lang.Equals("us"))
            {
                var CodesEN = _db.CodeFileEN.Where(o => IDs.Contains(o.ID) && o.ItemType == "Bed" && o.Deleted == false).Select(o=>o.ItemDescription).ToList();

                if (CodesEN == null)
                {
                    return string.Empty;
                }

                Bed = string.Join(",", CodesEN);
            }else
            {

                var CodesZH = _db.CodeFileZH.Where(o => IDs.Contains(o.ID) && o.ItemType == "Bed" && o.Deleted == false).Select(o=>o.ItemDescription).ToList();
                if (CodesZH == null)
                {
                    return string.Empty;
                }
              
                Bed = string.Join(",", CodesZH);
            }

            return Bed;
        }
    }
}