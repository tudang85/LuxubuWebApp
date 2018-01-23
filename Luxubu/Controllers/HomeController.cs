using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Luxubu.Models;
using Luxubu.Types;
using System.Data.SqlClient;
using System.Data;
using Luxubu.Utils;
using NSoup;
using NSoup.Nodes;
using Luxubu.Iterfaces;
using Luxubu.Implements;

namespace Luxubu.Controllers
{
    public class HomeController : Controller
    {
        private readonly LuxubuContext objCtx;
        public HomeController(LuxubuContext context)
        {
            this.objCtx = context;
            
        }
        [HttpGet]
        public IActionResult Index()
        {
            Stories[] arrStories = objCtx.Stories.ToArray();
            StoriesModel objModel = new StoriesModel() { Stories = arrStories };
            return View(objModel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            var password = Security.GetHashSha256(login.Password).ToLower();

            SqlParameter retVal = new SqlParameter("@return_value", System.Data.SqlDbType.Int) { Direction = ParameterDirection.Output };
            object[] parameters =
            {
                 new SqlParameter("@Username", System.Data.SqlDbType.VarChar, 64) { Value = login.User},
                 new SqlParameter("@Password", System.Data.SqlDbType.VarChar, 1024) { Value = password},
                retVal
            };
            string command = string.Format("exec @return_value = dbo.Login @Username, @Password");
            var result = this.objCtx.Database.ExecuteSqlCommand(command, parameters);
            int intRetVal = (int)retVal.Value;
            if(intRetVal !=1 )
            {
                return View("Login");
            }
            Session.SetSession(this.HttpContext, new SessionData() { UserName = login.User });
            return RedirectToAction("Index");// View("Index");
        }

        [HttpPost]
        public IActionResult AddStory(string SourceUrl)
        {
            string strMsg = "OK";
            int intError = 0;
            IGetStory iGetStory = null;
            Stories objStory = null;
            try
            {
                if (SourceUrl.Contains("http://truyenfull.vn"))
                {
                    iGetStory = new GetTruyenFull(SourceUrl);
                }

                objStory = iGetStory.GetInfo();
                objCtx.Stories.Add(objStory);
                int intAdded = objCtx.SaveChanges();
                Chapters[] arrChapterObj = iGetStory.GetChapters(objStory.Id);
                objCtx.Chapters.AddRange(arrChapterObj);
                intAdded = objCtx.SaveChanges();
            }
            catch (Exception objEx)
            {
                strMsg = objEx.Message;
                intError = 1000;
            }
            var result = new
            {
                error = intError,
                msg = strMsg,
                story = objStory,
            };
            return new JsonResult(result);
        }

        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
