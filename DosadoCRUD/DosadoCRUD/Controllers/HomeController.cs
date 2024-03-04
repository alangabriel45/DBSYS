using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DosadoCRUD.Controllers
{
    [Authorize(Roles = "User,Manager")] // Set filter to this controller (must login)
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            // Only authenticated user can Create | Update | Delete
            // Hide if user did not logged in
            // or var userList = _userRepo.GetAll();
            List<User> userList = _userRepo.GetAll();
            return View(userList);
        }
        [AllowAnonymous] // Override allow not authenticated user to access login
        public ActionResult Login()
        {
            // Check if already loged in no need to log in again, redirect to index
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User u)
        {
            _userRepo.Create(u);
            TempData["Msg"] = $"User {u.username} Added!";

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            return View(_userRepo.Get(id));
        }
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            return View(_userRepo.Get(id));
        }
        [HttpPost]
        public ActionResult Edit(User u)
        {
            _userRepo.Update(u.id, u);
            TempData["Msg"] = $"User {u.username} Updated!";

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            _userRepo.Delete(id);
            TempData["Msg"] = $"User Deleted!";

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User u)
        {
            // Same as Select * From User Where username = u.username limit 1 or top or default
            var user = _userRepo.Table().Where(m => m.username == u.username).FirstOrDefault();
            if (user != null)
            {
                // User is correct username and password
                FormsAuthentication.SetAuthCookie(u.username, false); // set cookie

                return RedirectToAction("Index");
            }
            // User does not exist or incorrect
            // add error to the form
            ModelState.AddModelError("", "User does not exist or incorrect password");

            return View(u);
        }
    }
}