using PosteroOrg.Mural.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOne.Security.Cryptography.BCrypt;
using System.Text.RegularExpressions;

namespace PosteroOrg.Mural.Controllers
{
    public class MainController : Controller
    {
        private Database db;

        public MainController()
        {
            db = new Database();
        }

        // GET: /
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                User user = db.Users.Find((Session["User"] as User).Username);
                return View(user.Notes.ToList());
            }
            return View(new List<Note>());
        }

        // POST: /login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                User user = db.Users.Find(username);
                if (user != null && BCryptHelper.CheckPassword(password, user.Password))
                {
                    Session["User"] = user;
                }
            }
            return RedirectToAction("");
        }

        // POST: /register
        [HttpPost]
        public ActionResult Register(string username, string password, string passwordCheck)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) && password == passwordCheck && db.Users.Find(username) == null)
            {
                if (Regex.IsMatch(username, @"^[a-zA-Z0-9_]{6,}$") && Regex.IsMatch(password, @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}"))
                {
                    User user = new User()
                    {
                        Username = username.Trim(),
                        Password = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt()),
                        DtRegister = DateTimeOffset.Now
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["User"] = user;
                }                
            }
            return RedirectToAction("");
        }

        // GET: /logout
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("");
        }

        // POST: /new
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult New(string note)
        {
            if (Session["User"] != null && !String.IsNullOrWhiteSpace(note))
            {
                User user = db.Users.Find((Session["User"] as User).Username);
                user.Notes.Add(new Note() {
                    PureContent = note,
                    DtNote = DateTimeOffset.Now
                });
                db.SaveChanges();
            }
            return RedirectToAction("");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}