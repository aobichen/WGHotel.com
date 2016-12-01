using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Models
{
    public class PRDate
    {
        public PRDate()
        {
            SetDefault();
        }
        public string Begin { get; set; }
        public string End { get; set; }

        public void Edit()
        {
            using (var db = new WGHotelsEntities())
            {
                var data = db.SysSetting.Where(o => o.Code.Equals("RPDate")).ToList();
                var BeginId = data.Where(o => o.Remark == "Begin").FirstOrDefault().ID;
                var EndId = data.Where(o => o.Remark == "End").FirstOrDefault().ID;
                var BeginDate = db.SysSetting.Where(o => o.ID == BeginId).FirstOrDefault();
                BeginDate.Value = Begin;

                var EndDate = db.SysSetting.Where(o => o.ID == EndId).FirstOrDefault();
                EndDate.Value = End;
                db.SaveChanges();
            }
        }

        private void SetDefault(){
            using (var db = new WGHotelsEntities())
            {
                var data = db.SysSetting.Where(o => o.Code.Equals("RPDate")).ToList();
                Begin = data.Where(o => o.Remark == "Begin").FirstOrDefault().Value;
                End = data.Where(o => o.Remark == "End").FirstOrDefault().Value;
            }
        }
    }

    public class RoomCanEditDate
    {
        public RoomCanEditDate()
        {
            SetDefault();
        }
        public string Begin { get; set; }
        public string End { get; set; }

        public void Edit()
        {
            using (var db = new WGHotelsEntities())
            {
                var data = db.SysSetting.Where(o => o.Code.Equals("RoomCanEditDate")).ToList();
                var BeginId = data.Where(o => o.Name == "Begin").FirstOrDefault().ID;
                var EndId = data.Where(o => o.Name == "End").FirstOrDefault().ID;
                var BeginDate = db.SysSetting.Where(o => o.ID == BeginId).FirstOrDefault();
                BeginDate.Value = Begin;

                var EndDate = db.SysSetting.Where(o => o.ID == EndId).FirstOrDefault();
                EndDate.Value = End;
                db.SaveChanges();
            }
        }

        private void SetDefault()
        {
            using (var db = new WGHotelsEntities())
            {
                var data = db.SysSetting.Where(o => o.Code.Equals("RoomCanEditDate")).ToList();
                Begin = data.Where(o => o.Name == "Begin").FirstOrDefault().Value;
                End = data.Where(o => o.Name == "End").FirstOrDefault().Value;
            }
        }
    }
}