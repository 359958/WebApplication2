using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication2.Controllers
{
    public class BookingController : Controller
    {

        string userid = "1013";// System.Web.HttpContext.Current.Session["CID"].ToString();
        // GET: Booking
        public ActionResult Booking()
        {
            var obj = MovieRunningNow();
            return View(obj);
        }

        [HttpGet]
        public ActionResult GetMoviePop(string Id)
        {
            
            var dates = GetMovieDate(Id);
            ViewBag.Date = dates.Distinct().Select(i => new SelectListItem() { Text = i.AllDays.ToString(), Value = i.AllDays.ToString() }).ToList();
            var moviesList = MovieRunningNow();
            var linq = moviesList.Where(x => x.MovieId == Id).Select(x => new { x.MovieId, x.MovieName }).ToList();
            MovieDetails obj = new MovieDetails();
            foreach (var item in linq)
            {
                obj.MovieID = item.MovieId;
                obj.MovieName = item.MovieName;
            }


            ViewBag.Classid = DropDown.Classid();
            ViewBag.Showid = DropDown.Showid();
            ViewBag.NoTickets = DropDown.NoTickets();
            return PartialView("Ticketbookpopup", obj);
        }

        [HttpPost]
        public ActionResult GetMoviePop(BookMovie obj)
        {
            //if (Session["CID"].ToString() is null)
            //{
            //    Session["CID"] = "1015";
            //    Session["UserID"] = "Guest";
            //}
            obj.CUSTOMERID = 1013;// Convert.ToInt32(Session["CID"].ToString());
            string json = BookMovieTic(obj);
            var res = json.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
            JObject Jobj = JObject.Parse(res);
            int Bookid = (int)Jobj["id"];

            TempData["bookid"] = Bookid;
            if (Bookid > 0)
            {
                return RedirectToAction("Mybookings", "Booking");
            }
            else
            {

                TempData["shortMessage"] = "Sorry!!!Seleted show HouseFull";

                return RedirectToAction("Booking");
            }
        }
        public List<Showdates> GetMovieDate(string MovieID)
        {

            List<Showdates> showdate = new List<Showdates>();
            return HttpCalls.HttpclientListcall(showdate, "BookMovie", "GetMovieDate"+ "?MovieId=" + MovieID);

            //var list = new List<BookMovieTicket>();

            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage response;
            //response = httpClient.GetAsync(webapiurl + "/BookMovie/GetMovieDate?MovieId=" + MovieID).Result;
            //response.EnsureSuccessStatusCode();
            //List<Showdates> stateList = response.Content.ReadAsAsync<List<Showdates>>().Result;
            ////if (response.IsSuccessStatusCode)
            ////{
            ////    using (HttpContent content = response.Content)
            ////    {
            ////        string jsonS = response.Content.ReadAsStringAsync().Result;
            ////        var ds = JsonConvert.DeserializeObject(jsonS);
            ////        return ds;
            ////    }
            ////}

            //if (!object.Equals(stateList, null))
            //{
            //    var states = stateList.ToList();
            //    return states;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public List<MovieDetailsdelete> MovieRunningNow()
        {
            List<MovieDetailsdelete> movielist = new List<MovieDetailsdelete>();
            return HttpCalls.HttpclientListcall(movielist, "Register", "GetAllMovie");
        }

        public string BookMovieTic(BookMovie obj)
        {

            return HttpCalls.Httpclientcall(obj, "BookMovie", "BookTicket");
        }

        public ActionResult Mybookings()
        {
            var output = MyTickets(userid);
            return View(output);
        }

        public List<BookMovieTicket> MyTickets(string uid)
        {
            List<BookMovieTicket> movieticket = new List<BookMovieTicket>();
            return HttpCalls.HttpclientListcall(movieticket, "BookMovie", "MyBooking"+ "?cusid="+uid);
        }

        [HttpGet]
        public ActionResult MyTicketDetails(string Id)
        {
            var output = TicketDetails(Id);
            return PartialView("TicketDetails", output);
        }
        public List<BookMovieTicket> TicketDetails(string bookingid)
        {
            
            List<BookMovieTicket> movieticket = new List<BookMovieTicket>();
            return HttpCalls.HttpclientListcall(movieticket, "BookMovie", "Get" + "?bookid=" + bookingid);
            
        }


        //--------------------------Complient---------------------------
        public ActionResult Complient()
        {
            ViewBag.IssueList = DropDown.IssueList();

            return View();
        }
        [HttpPost]
        public ActionResult Complient(ComplientStatus obj)
        {
            obj.cusid = 26; //Session["UserID"];
            string val = RaiseComplient(obj);

            if (val.Contains("Successfully"))
            {
                ViewBag.Message = "Successfully";
            }
            ViewBag.IssueList = DropDown.IssueList();

            return View();
        }


        public string RaiseComplient(ComplientStatus obj)
        {
            return HttpCalls.Httpclientcall(obj, "BookMovie", "Complient");
        }
        //--------------------------------------------------------------
    }
               
 }
   