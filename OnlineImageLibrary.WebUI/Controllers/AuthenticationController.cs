using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace OnlineImageLibrary.WebUI.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public JsonResult Login(string password)
        {
            var authenticated = (ConfigurationManager.AppSettings["adminPassword"].ToString() == password);
            return Json(new { authenticated = authenticated, token = authenticated ? System.Guid.NewGuid().ToString() : null });
        }
    }
}
