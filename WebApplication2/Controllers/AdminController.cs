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
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace WebApplication2.Controllers
{
    public class AdminController : Controller
    {

            /*-------------------------------------*/

        [HttpGet]
        public ActionResult AdminUserPage(string type) //ChangePrice
        {
            if (type == "AddTicket")
            {
                ViewBag.Screen = DropDown.ScreenList();
                ViewBag.Class = DropDown.Classid();
                return PartialView("AddSeat");
            }
            else if (type == "ChangePrice")
            {
                ViewBag.Class = DropDown.Classid();
                return PartialView("Changeticketprice");
            }

            return View();
        }
        /*-----------------------------------*/

        /*Add Movie*/
        public ActionResult AddMovie()
        {
            ViewBag.ScreenList = DropDown.ScreenList();
            return View();
        }

        [HttpPost]
        public ActionResult AddMovie(FormCollection form)
        {
            string Screen = form["Screen"];
            string Movie = form["Movie"];
            string From = form["From"];
            string RunningUpto = form["RunningUpto"];
            string filePath = string.Empty;
            string url = string.Empty;
            if (Request.Files.Count > 0)
            {
                int i = 0;
                CloudStorageAccount cloudStorageAccount = DropDown.GetConnectionString();
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
                cloudBlobContainer.CreateIfNotExists();
                foreach (string files in Request.Files)
                {
                    string fileName = Path.GetFileName(Request.Files[i].FileName);
                    CloudBlockBlob azureBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    azureBlockBlob.UploadFromStream(Request.Files[i].InputStream);

                    i++;
                    url = azureBlockBlob.Uri.ToString();

                }
            }
          
            
            movieDetailsList obj = new movieDetailsList();
            obj.Screen = Screen;
            obj.Movie = Movie;
            obj.From = Convert.ToDateTime(From);
            obj.RunningUpto = Convert.ToDateTime(RunningUpto);
            obj.path = url;
            var res = AddMoviemethod(obj);
            if (res.Contains("Successfully"))
            {
                ViewBag.Message = res;
            }
            ViewBag.ScreenList = DropDown.ScreenList();
            return View();
        }

        public string AddMoviemethod(movieDetailsList obj)
        {
            return HttpCalls.Httpclientcall(obj, "Admin", "AddMovie");
        }

        /*Add Movie*/


        /*---------------------------AddSeat--------------------------------------*/
        [HttpPost]
        public ActionResult AddSeats(AddSeats obj)
        {
            var res = AddSeatMethod(obj);
            if (res.Contains("Successfully"))
            {
                ViewBag.Message = res;
                ViewBag.Screen = DropDown.ScreenList();
                ViewBag.Class = DropDown.Classid();
            }
            return View("AdminUserPage");
        }

        public string AddSeatMethod(AddSeats obj)
        {
            return HttpCalls.Httpclientcall(obj, "Admin", "Updateseat");
        }
        /*---------------------------AddSeat--------------------------------------*/
        [HttpPost]
        public ActionResult ChangePrice(ChangePrice obj)
        {
            var res = ChangePricemethod(obj);
            if (res.Contains("Successfully"))
            {
                ViewBag.Message = res;
                ViewBag.Screen = DropDown.ScreenList();
                ViewBag.Class = DropDown.Classid();
            }
            return View("AdminUserPage");
        }


        public string ChangePricemethod(ChangePrice obj)
        {
            return HttpCalls.Httpclientcall(obj, "Admin", "UpdatePrice");
          
        }
        /*---------------------------------------------------------------------------------*/


        /**-------------------------------Complients-------------------------------------*/
        public ActionResult ViewFeedback()
        {
            var output = AdminComplientsDetailsmethod();
            return View(output);
        }


        public List<ComplientList> AdminComplientsDetailsmethod()
        {
            List<ComplientList> lstcomplient = new List<ComplientList>();
            return HttpCalls.HttpclientListcall(lstcomplient, "admin", "GetComplients");
           
        }
        /**-------------------------------Complients-------------------------------------*/

        /*---------------------Movie Info---------------------*/
        public ActionResult MovieInfo()
        {
            var output = AdminMovieDetails();
            return View(output);
        }

        public List<movieDetailsList> AdminMovieDetails()
        {
            List<movieDetailsList> movobj = new List<movieDetailsList>();
            return HttpCalls.HttpclientListcall(movobj, "admin", "GetMovieRunning");
        }
        /*---------------------Movie Info---------------------*/
    }
}