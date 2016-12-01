using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using WGHotel.Models;

namespace WGHotel.Areas.Backend.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // GET: Backend/Profile
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var id = User.Identity.GetUserId<int>();
                var account = UserManager.FindById(id);
                account.Email = model.NewPassword;
                var updateResult = UserManager.Update(account);
                return RedirectToAction("Login", "Account", new { area = ""});
            }

            ModelState.AddModelError("","密碼變更失敗");
            
            return View(model);
        }

        // GET: Backend/Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Backend/Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Profile/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Backend/Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Backend/Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Backend/Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Backend/Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
