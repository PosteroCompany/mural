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
                    Alert("Você foi autenticado com sucesso.", type: "success");
                }
                else
                {
                    Alert("Note que ambos os campos diferenciam letras maiúsculas e minúsculas.", "Não reconhecemos suas credenciais.", "danger");
                }
            }
            else
            {
                Alert("Parece que você esqueceu de informar seu usuário ou sua senha.", "Ops.", "danger");
            }
            return RedirectToAction("");
        }

        // POST: /register
        [HttpPost]
        public ActionResult Register(string username, string password, string passwordCheck)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                if (password == passwordCheck)
                {
                    if (Regex.IsMatch(username, @"^[a-zA-Z0-9_]{6,}$") && Regex.IsMatch(password, @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}"))
                    {
                        if (db.Users.Find(username) == null)
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
                            Alert("Você foi cadastrado com sucesso.", "Bem vindo.", "success");
                        }
                        else
                        {
                            Alert("Parece que alguém já escolheu esse nome de usuário.", "Usuário já existe.", "danger");
                        }
                    }
                    else
                    {
                        Alert("Note que sua senha deve ter pelo menos um número, uma letra maiúscula, uma letra minúscula e 8 caractéres ou mais. E escolha seu nome de usuário utilizando apenas caractéres alfanuméricos e traço baixo, e deve ter pelo menos 6 caractéres.", "Informações inválidas.", "danger");
                    }
                }
                else
                {
                    Alert("Parece que você sua senha não confere com a confirmação.", "Ops.", "danger");
                }

            }
            else
            {
                Alert("Parece que você esqueceu de informar um nome de usuário ou uma senha.", "Ops.", "danger");
            }
            return RedirectToAction("");
        }

        // GET: /logout
        public ActionResult Logout()
        {
            Session["User"] = null;
            Alert("Você foi desconectado com sucesso.", "Desconectado.");
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
                user.Notes.Add(new Note()
                {
                    PureContent = note,
                    DtNote = DateTimeOffset.Now
                });
                db.SaveChanges();
                Alert("Sua nova nota foi registrada com sucesso.", "Salvo.", "success");
            }
            else
            {
                Alert("Parece que você esqueceu de escrever a nota.", "Ops.", "danger");
            }
            return RedirectToAction("");
        }

        protected void Alert(string message, string title = "", string type = "")
        {
            var alerts = TempData["Alerts"] != null ? (List<Models.Alert>)TempData["Alerts"] : new List<Models.Alert>();
            alerts.Add(new Models.Alert
            {
                Message = message,
                Title = title,
                Type = type
            });
            TempData["Alerts"] = alerts;
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