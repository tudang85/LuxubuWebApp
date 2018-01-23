using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Luxubu.Controllers
{
    public class LuxubuController : Controller
    {
        public IActionResult Index()
        {
            
            HttpContext.Session.SetString("hgf", "Rick");

            return View();
        }
    }
}