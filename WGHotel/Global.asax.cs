using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace WGHotel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    RouteData routeData = RouteTable.Routes.GetRouteData(
        //        new HttpContextWrapper(HttpContext.Current));
        //    var action = routeData.GetRequiredString("action");
        //    var controller = routeData.GetRequiredString("controller");
        //    if (action == "Home" && controller == "Home")
        //    {
        //        HttpContext ctx = HttpContext.Current;
        //        ctx.Response.Redirect("/"); 
        //    }
        //}
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            
        }

        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }


        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
          
            System.Web.HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
        }

        protected void Application_AcquireRequestState()
        {
            HttpCookie langCookie = Request.Cookies["lang"];
           
        }

        void Application_Error(object sender, EventArgs e) 
    { 
        // 發生未處理錯誤時執行的程式碼
        // At this point we have information about the error
        HttpContext ctx = HttpContext.Current;

        Exception exception = ctx.Server.GetLastError();

        string errorInfo =
           "Offending URL: " + ctx.Request.Url.ToString() +
           "Source: " + exception.Source +
           "Message: " + exception.Message +
           "Stack trace: " + exception.StackTrace;


        if (exception.StackTrace.Contains("ValidateString"))
        {   
     //因為我所要攔截的訊息會有「ValidateString」字眼，所以我將這事件另外導到其他頁面
            ctx.Response.Redirect("~/Xss.html");            
        }
        else
        {
            ctx.Response.Write(errorInfo);
            //ctx.Response.Redirect("~/404.html");
        }

        // --------------------------------------------------
        // To let the page finish running we clear the error
        // --------------------------------------------------
        ctx.Server.ClearError();
       

    }
    }
}
