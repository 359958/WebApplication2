using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class RegisterController : Controller
    {
        //--------------------
        string cookies = string.Empty;
        string JsonData = string.Empty;
        //--------------------
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsersLogin obj)
        {
            
            JsonData = ValidateUser(obj);
            var UserData = JsonConvert.DeserializeObject<IEnumerable<UserData>>(JsonData);
            foreach (var item in UserData)
            {
                Session["UserID"] = item.CName.ToString();
                Session["CID"] = item.CID.ToString();
                cookies = item.Cemail.ToString();
            }

            if (UserData.Count() > 0)
            {

                FormsAuthentication.SetAuthCookie(cookies, false);
                return RedirectToAction("Booking", "Booking");
            }
            else
            {
                ViewBag.Message = "UserName/Password invalid";
                return View(); }
                
        }


        public string ValidateUser(UsersLogin obj)
        {

            return HttpCalls.Httpclientcall(obj, "Register", "loginUser");
        }

        [HttpGet]
        public ActionResult NewUserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewUserRegister(UserData obj)
        {
            if (ModelState.IsValid)
            {
                var sus = AddUserDetails(obj);
                if (sus.Contains("Successfully"))
                {
                    ViewBag.Message = "Successfully";
                }
                else if (sus.Contains("Exist"))
                {
                    ViewBag.Message = "Email Id Exist already";
                }
                return View();
            }
            else {
                return View(obj);
            }
            
           
        }
        public string AddUserDetails(UserData obj)
        {
            return HttpCalls.Httpclientcall(obj, "Register","createUser");
        }


        public ActionResult MyProfile()
        {
            List<UserDetails> obj = updateuserProfile();
            UserData edit = new UserData();

            edit.CName = obj[0].CName;
            edit.CPhone = obj[0].CPhone;

            return View(edit);
        }

        [HttpPost]
        public ActionResult MyProfile(UserData obj)
        {
            var sus = UpdateUserDetails(obj);
            if (sus.Contains("Successfully"))
            {
                ViewBag.Message = "Profile Created";
            }
            else if (sus.Contains("Exist"))
            {
                ViewBag.Message = "Email Id Exist already";
            }
            return View();
        }

        public string UpdateUserDetails(UserData obj)
        {

            obj.CID = int.Parse(Session["CID"].ToString());

            return HttpCalls.Httpclientcall(obj, "Register", "UpdateUser");
        }
        public List<UserDetails> updateuserProfile()
        {
            string userid = Session["CID"].ToString();
            List<UserDetails> editobj = new List<UserDetails>();
            return HttpCalls.HttpclientListcall(editobj, "Register", "GetUserProfile"+ "?cusid="+userid);
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["UserID"] = "";
            Session["CID"] = "";
            return RedirectToAction("About", "Register");
        }

    }
}